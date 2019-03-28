using POC_SqLite.Enumerators;
using POC_SqLite.Output;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace POC_SqLite.XlsFile
{
    class XlsFileOutput : Output.Output
    {
        private string Diretorio;

        public XlsFileOutput()
        {
            this.Diretorio = ConfigurationSettings.AppSettings["DiretorioSaidaXlsFile"];
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
