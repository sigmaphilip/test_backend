using System.Xml;
using System;
using System.Xml.Linq;
using System.Text.Json.Nodes;

namespace API.Models.Helpers.XMLFormatter
{
    public class XMLFormatterHelper
    {
        public void XmlNodeToJSON(System.Xml.XmlNode node, JsonObject json)
        {
            if (node.HasChildNodes)
            {
                foreach (System.Xml.XmlNode child in node.ChildNodes)
                {
                    Console.WriteLine(child.Name);
                    if (child.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        if (!json.ContainsKey(child.Name))
                        {
                            json[child.Name] = new JsonObject();
                        }
                        JsonObject childObject = (JsonObject)json[child.Name];
                        XmlNodeToJSON(child, childObject);
                    }
                    else if (child.NodeType == System.Xml.XmlNodeType.Text)
                    {
                        json[node.Name] = child.InnerText;
                    }
                }
            }
        }
    }
}
