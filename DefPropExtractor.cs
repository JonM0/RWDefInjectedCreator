using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

#nullable enable

namespace RWDefInjectedCreator
{
    internal class DefPropExtractor
    {
        private readonly DefInjectedManager manager;

        public bool removeFromOriginalDef = true;

        public DefPropExtractor ( DirectoryInfo defInjectedDirectory )
        {
            this.manager = new DefInjectedManager( defInjectedDirectory );
        }

        public void ExtractFromFile ( FileInfo defFile )
        {
            XmlDocument xmlDocument = new XmlDocument
            {
                PreserveWhitespace = true
            };

            using ( FileStream defStream = defFile.OpenRead() )
            {
                xmlDocument.Load( defStream );
            }

            foreach ( XmlNode def in xmlDocument.DocumentElement?.ChildNodes
                                     ?? throw new ArgumentException( "File xml DocumentElement was null", nameof( defFile ) ) )
            {
                ExtractFromNode( def, defFile.Name );
            }

            xmlDocument.Save( defFile.FullName );
        }

        private static readonly List<string> nodeNames = new List<string>
        {
            "label",
            "labelNoun",
            "description",
            "jobString",
        };

        private static bool NodeShouldBeInj ( XmlNode node )
        {
            return nodeNames.Contains( node.Name );
        }

        public void ExtractFromNode ( XmlNode defNode, string targetFileName )
        {
            string? defname = defNode.ChildNodes.Cast<XmlNode>().FirstOrDefault( n => n.Name == "defName" )?.InnerText;
            string defType = defNode.Name;

            if ( defname != null )
            {
                List<XmlNode> nodesToMove = new List<XmlNode>();

                foreach ( XmlNode xmlNode in defNode.ChildNodes )
                {
                    if ( NodeShouldBeInj( xmlNode ) )
                    {
                        nodesToMove.Add( xmlNode );
                    }
                }

                if ( nodesToMove.Count > 0 )
                {
                    var defInjDoc = manager.GetDefType( defType ).GetDocument( targetFileName );

                    defInjDoc.AppendComment( defname );

                    foreach ( var node in nodesToMove )
                    {
                        defInjDoc.Append( defname, node.Name, node.InnerText );

                        if ( removeFromOriginalDef )
                        {
                            while ( node.PreviousSibling?.NodeType == XmlNodeType.Whitespace )
                            {
                                defNode.RemoveChild( node.PreviousSibling );
                            }
                            defNode.RemoveChild( node );
                        }
                    }
                }
            }
        }

        public void Save ()
        {
            manager.SaveAll();
        }
    }
}