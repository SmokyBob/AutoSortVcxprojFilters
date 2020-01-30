//------------------------------------------------------------------------------
// <copyright file="SortAllCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace AutoSortSqlProj
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SortAllCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("9efcf168-e247-4de0-8edd-fae87ad59b6c");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AutoSortSqlProjPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortAllCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private SortAllCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package as AutoSortSqlProjPackage;

            OleMenuCommandService commandService = this.package.GetServiceAsync(typeof(IMenuCommandService)).Result as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SortAllCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new SortAllCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var projects = package.GetProjects();

            foreach (var proj in projects)
            {
                //... Should exclude non .sqlproj ... but the sort function already does that...
                SqlProjSorter.Sort(proj.FullName);
            }
        }
    }
}
