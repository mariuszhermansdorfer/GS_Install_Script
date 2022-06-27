using System;
using System.IO;
using System.Xml;

namespace GS_Install_Script
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string directory = Path.Combine(appDataPath, @"Grasshopper\Libraries\GreenScenario_2_0");
            
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);

            // Iterate over Rhino versions 6-9
            for (int i = 6; i <= 9; i++)
            {
                
                string fileLocation = Path.Combine(appDataPath, @"McNeel\Rhinoceros\" + i + @".0\settings\settings-Scheme__Default.xml");

                if (!File.Exists(fileLocation))
                    continue;

                XmlDocument doc = new XmlDocument();
                doc.Load(fileLocation);
                XmlNode node = doc.SelectSingleNode("//child[@key='Options']");
                if (node != null)
                {
                    XmlNode child = node.SelectSingleNode("//child[@key='PackageManager']");
                    if (child != null)
                    {
                        XmlNode entry = child["entry"];
                        if (entry != null && !entry.InnerText.Contains("GreenScenario"))
                        {
                            // if entry already exists, add our installation path
                            entry.InnerText += ";\\\\ramhhafile10\\Data\\10-Projekte\\GreenScenario";
                        }
                    }
                    else
                    {
                        // create entry if it doesn't exist
                        XmlElement newChild = doc.CreateElement("child");
                        newChild.SetAttribute("key", "PackageManager");
                        XmlElement newEntry = doc.CreateElement("entry");
                        newEntry.SetAttribute("key", "Sources");
                        newEntry.InnerText = "https://yak.rhino3d.com;\\\\ramhhafile10\\Data\\10-Projekte\\GreenScenario";
                        newChild.AppendChild(newEntry);
                        node.AppendChild(newChild);
                    }
                }
                doc.Save(fileLocation);
            }
        }
    }
}
