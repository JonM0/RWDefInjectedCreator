using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

#nullable enable

namespace RWDefInjectedCreator
{
    internal class Program
    {
        private static void Main ( string[] args )
        {
            // open needed folders

            Console.WriteLine( "Input Def folder" );
            string defFolderPath = "Common/Defs";// Console.ReadLine();

            DirectoryInfo defFolder = new DirectoryInfo( defFolderPath );
            if ( !defFolder.Exists )
            {
                Console.Error.WriteLine( "Could not open def folder." );
                return;
            }

            Console.WriteLine( "Input Language folder" );
            string languageFolderPath = "1.2/Languages";// Console.ReadLine();

            DirectoryInfo languageFolder = new DirectoryInfo( languageFolderPath );
            if ( !languageFolder.Exists )
            {
                Console.Error.WriteLine( "Could not open language folder." );
                return;
            }

            // make or open DefInjected folder

            DirectoryInfo defInjLangFolder = languageFolder.CreateSubdirectory( "English" ).CreateSubdirectory( "DefInjected" );

            // start extraction

            DefPropExtractor extractor = new DefPropExtractor( defInjLangFolder );

            foreach ( FileInfo file in defFolder.EnumerateFiles( "*.xml", SearchOption.AllDirectories ) )
            {
                try
                {
                    extractor.ExtractFromFile( file );
                }
                catch ( Exception exc )
                {
                    Console.Error.WriteLine( "Exception while extracting from " + file.FullName + "\n" + exc );
                }
            }

            extractor.Save();
        }
    }
}