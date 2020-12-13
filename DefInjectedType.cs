using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace RWDefInjectedCreator
{
    internal class DefInjectedType
    {
        private readonly List<DefInjectedDocument> documents = new List<DefInjectedDocument>();
        private readonly DirectoryInfo directory;

        public DefInjectedType ( string defType, DirectoryInfo directory )
        {
            this.DefType = defType;
            this.directory = directory;
        }

        public DefInjectedDocument GetDocument ( string name )
        {
            var find = documents.Find( d => d.Name == name );

            if ( find == null )
            {
                find = new DefInjectedDocument( new FileInfo( directory.FullName + "/" + name ) );
                documents.Add( find );
            }

            return find;
        }

        public string DefType { get; init; }

        public void SaveAll ()
        {
            for ( int i = 0; i < documents.Count; i++ )
            {
                documents[i].Save();
            }
        }
    }
}