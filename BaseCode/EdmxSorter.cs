using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BaseCode
{
    class EdmxSorter
    {
        public static void Sort(string FullFilePath)
        {
            try
            {
                //The path should always be just sqlproj files, but just in case
                if (FullFilePath.EndsWith(".edmx") && System.IO.File.Exists(FullFilePath))
                {
                    System.Threading.Thread.Sleep(200); // Avoid load fail

                    XDocument xmlDocument = XDocument.Load(FullFilePath);

                    foreach (var nameSpace in new List<String> { "{http://schemas.microsoft.com/ado/2009/11/edm/ssdl}", "{http://schemas.microsoft.com/ado/2009/11/edm}"
                        , "{http://schemas.microsoft.com/ado/2009/11/mapping/cs}" })
                    {
                        //Loop only the EntityContainer elements
                        foreach (var itemgroup in xmlDocument.Root.Descendants(nameSpace+"EntityContainer"))
                        {
                            //Order in file
                            //  EntitySet
                            //  AssociationSet
                            itemgroup.ReplaceNodes(from elem in itemgroup.Elements()
                                                   orderby elem.Name.LocalName descending, elem.Attribute("Name")?.Value
                                                   select elem);
                        }

                        //Loop only the Schema elements
                        foreach (var itemgroup in xmlDocument.Root.Descendants(nameSpace+"Schema"))
                        {
                            //Order in file
                            //  EntityType
                            //  Association
                            //  EntityContainer
                            itemgroup.ReplaceNodes(from elem in itemgroup.Elements()
                                                   orderby (elem.Name.LocalName.EndsWith("EntityType") ? 1 : (elem.Name.LocalName.EndsWith("Association") ? 2 : 3)), elem.Attribute("Name")?.Value
                                                   select elem);
                        }
                        foreach (var itemgroup in xmlDocument.Root.Descendants(nameSpace+"EntityContainerMapping"))
                        {
							//Order in file
							//  EntitySetMapping
							itemgroup.ReplaceNodes(from elem in itemgroup.Elements()
                                                   orderby elem.Name.LocalName descending, elem.Attribute("Name")?.Value
                                                   select elem);

							//  FunctionImportMapping
							itemgroup.ReplaceNodes(from elem in itemgroup.Elements()
												   orderby elem.Attribute("FunctionImportName")?.Value
												   select elem);
						}

					}



                    //Binary check if we made any changes
                    var originalBytes = File.ReadAllBytes(FullFilePath);
                    byte[] newBytes = null;

                    using (var memoryStream = new MemoryStream())
                    using (var textWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                    {
                        xmlDocument.Save(textWriter, SaveOptions.None);
                        newBytes = memoryStream.ToArray();
                    }

                    if (!AreEqual(originalBytes, newBytes))
                    {
                        //Save only if changes happened, especially because VS will ask to reload the project
                        xmlDocument.Save(FullFilePath);
                    }
                }
            }
            catch(Exception e)
            {
                var a = 0;
                // Make sure the user is not interrupted
            }
        }

        private static bool AreEqual(byte[] left, byte[] right)
        {
            if (left == null)
            {
                return right == null;
            }

            if (right == null)
            {
                return false;
            }

            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static List<ProjectItem> FindProjectItemInProject(ProjectItem project)
        {
            List<ProjectItem> projectItems = new List<ProjectItem>();

            if (project.Name.EndsWith(".edmx"))
            {
                projectItems.Add(project);
            }
            else
            {
                if (project.ProjectItems != null)
                {
                    foreach (ProjectItem itm in project.ProjectItems)
                    {
                        var tmp = FindProjectItemInProject(itm);
                        if (tmp != null && tmp.Count>0)
                        {
                            projectItems.AddRange(tmp);
                        }
                    }

                }
            }

            return projectItems;
        }

    }
}
