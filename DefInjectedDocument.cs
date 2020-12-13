using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System;

#nullable enable

namespace RWDefInjectedCreator
{
    internal class DefInjectedDocument
    {
        public const string LanguageDataNodeName = "LanguageData";

        private readonly XmlDocument xml = new XmlDocument();
        private readonly XmlNode root;

        private readonly FileInfo fileInfo;

        public string Name => fileInfo.Name;

        public DefInjectedDocument ( FileInfo fileInfo )
        {
            this.fileInfo = fileInfo;

            if ( !fileInfo.Exists )
            {
                XDocument doc = new XDocument( new XElement( LanguageDataNodeName ) );
                doc.Save( fileInfo.FullName );
            }

            using ( var fs = fileInfo.OpenRead() )
            {
                xml.Load( fs );
            }

            root = xml.ChildNodes.Cast<XmlNode>().FirstOrDefault( n => n.Name == LanguageDataNodeName )
                ?? throw new ApplicationException( "Invalid root node name in DefInjectedDocument " + Name );
        }

        //public void AppendWhitespace ()
        //{
        //    root.AppendChild( xml.CreateWhitespace( "" ) );
        //}

        public void AppendComment ( string? comment )
        {
            root.AppendChild( xml.CreateComment( comment ) );
        }

        public void Append ( string defName, string nodeName, string nodeValue )
        {
            var fullName = defName + "." + nodeName;

            var newNode = xml.CreateElement( fullName );
            newNode.InnerText = nodeValue;

            root.AppendChild( newNode );
        }

        public void Save ()
        {
            xml.Save( fileInfo.FullName );
        }
    }
}