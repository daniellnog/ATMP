using LaefazWeb.Models;
using POC_SqLite.Enumerators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;

namespace POC_SqLite.SqliteFile
{
    class SqliteFileOutput : Output.Output
    {
        private DbEntities db;
        private string Diretorio;

        public SqliteFileOutput()
        {
            this.Diretorio = ConfigurationSettings.AppSettings["DiretorioSaidaSqliteFile"];
            db = new DbEntities();
        }

        public override void CreateFile(int IdTestData, EnumTipoOutputFile output)
        {
            TestData td = db.TestData.FirstOrDefault(x=> x.Id == IdTestData);
            string NomeTabela = td.Script_CondicaoScript.NomeTecnicoScript;

            AUT aut = db.AUT.FirstOrDefault(x=> x.Id == td.DataPool.IdAut);
            SQLiteConnection.CreateFile(Diretorio + NomeTabela);
            List<ParametroValor> pvs = db.ParametroValor.Where(x=> x.IdTestData == td.Id).ToList();

            //List<Parametro> parametros = db.Parametro.Where(x=> x.)

            //this.CreateTable(NomeTabala, LinkedList);

            //SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=C:\Users\brucilin.de.gouveia\Desktop\MyDatabase.sqlite;Version=3;");
            //m_dbConnection.Open(); 

            //string sql = "create table highscores (name varchar(20), score int)";

            //SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            //command.ExecuteNonQuery();

            //sql = "insert into highscores (name, score) values ('Me', 9001)";

            //command = new SQLiteCommand(sql, m_dbConnection);
            //command.ExecuteNonQuery();

            //m_dbConnection.Close();
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
