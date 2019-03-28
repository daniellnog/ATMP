using POC_SqLite.Enumerators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_SqLite.TxtFile
{
    class TxtFileOutput : Output.Output
    {
        private string Diretorio;

        public TxtFileOutput()
        {
            this.Diretorio = ConfigurationSettings.AppSettings["DiretorioSaidaTxtFile"];
        }

        public override void CreateFile(int IdTestData, EnumTipoOutputFile output)
        {
            throw new NotImplementedException();
        }

        public override void CreateTable(string ScriptName, List<string> Columns)
        {
            throw new NotImplementedException();
        }

        public override void DeleteData(int Id)
        {
            throw new NotImplementedException();
        }

        public override void InsertData(int IdTestData)
        {
            throw new NotImplementedException();
        }

        public override void SelectData()
        {
            throw new NotImplementedException();
        }
    }
}
