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
    internal class DefInjectedManager
    {
        readonly List<DefInjectedType> types = new List<DefInjectedType>();
        readonly DirectoryInfo directory;

        public DefInjectedManager ( DirectoryInfo directory )
        {
            this.directory = directory;
        }

        public DefInjectedType GetDefType ( string defType )
        {
            var find = types.Find( t => t.DefType == defType );
            if ( find == null )
            {
                find = new DefInjectedType( defType, directory.CreateSubdirectory( defType ) );
                types.Add( find );
            }
            return find;
        }

        public DefInjectedType this[string defName] => this.GetDefType( defName );

        public void SaveAll()
        {
            for ( int i = 0; i < types.Count; i++ )
            {
                types[i].SaveAll();
            }
        }
    }
}