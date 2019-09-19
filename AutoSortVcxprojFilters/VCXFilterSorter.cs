using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AutoSortVcxprojFilters
{
    class VCXFilterSorter
    {
        public static void Sort(string FullFilePath)
        {
            try
            {
                if (FullFilePath.EndsWith(".sqlproj") && System.IO.File.Exists(FullFilePath))
                {
                    System.Threading.Thread.Sleep(200); // Avoid load fail

                    XDocument xmlDocument = XDocument.Load(FullFilePath);

                    

                    foreach (var itemgroup in xmlDocument.Root.Elements("{http://schemas.microsoft.com/developer/msbuild/2003}ItemGroup"))
                    {
                        itemgroup.ReplaceNodes(from elem in itemgroup.Elements()
                                               orderby elem.Attribute("Include")?.Value
                                               select elem);
                    }

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
                        //Save only if changes happened
                        xmlDocument.Save(FullFilePath);
                    }
                }
            }
            catch(Exception /*e*/)
            {
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
    }
}
