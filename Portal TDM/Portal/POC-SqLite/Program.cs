using LaefazWeb.Models;
using POC_SqLite.Output;
using System;
using System.Data.SQLite;

namespace POC_SqLite
{

    class Program
    {

        static void Main(string[] args)
        {
            OutputFactory output = new OutputFactory();
            output.CreateFile(12, Enumerators.EnumTipoOutputFile.SqliteFile);
        }
    }
}
