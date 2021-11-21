using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using System.Linq;
using EnvDTE;
using System.Collections.Generic;

namespace VS2019
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SortEdmx
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4129;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("b180da1d-47cd-4261-bb69-1452e784b9d6");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private EnvDTE.DTE m_dte;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortEdmx"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private SortEdmx(AsyncPackage package, OleMenuCommandService commandService, EnvDTE.DTE dte)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
            m_dte = dte;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SortEdmx Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in SortEdmx's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            EnvDTE.DTE dte = await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            Instance = new SortEdmx(package, commandService,dte);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            var projects = m_dte.Solution.Projects
                .Cast<EnvDTE.Project>()
                .Where(x => { return x?.Object != null; })
                .ToArray(); ;

            foreach (Project proj in projects)
            {
                var nm = proj.Name;
                if (proj.ProjectItems != null)
                {
                    List<ProjectItem> projectItems = new List<ProjectItem>();

                    foreach (ProjectItem fl in proj.ProjectItems)
                    {
                        var tmp = BaseCode.EdmxSorter.FindProjectItemInProject(fl);
                        if (tmp != null && tmp.Count>0)
                        {
                            projectItems.AddRange(tmp);
                        }
                    }

                    foreach (var itm in projectItems)
                    {
                        BaseCode.EdmxSorter.Sort(itm.FileNames[0]);
                    }
                }
            }
        }
    }
}
