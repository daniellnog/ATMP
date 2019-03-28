using POC_SqLite.Enumerators;
using POC_SqLite.SqliteFile;
using POC_SqLite.TxtFile;
using POC_SqLite.XlsFile;
using System.Configuration;

namespace POC_SqLite.Output
{
    class OutputFactory
    {
        public void CreateFile(int IdTestData, EnumTipoOutputFile output)
        {
            Output outputFile = null;

            switch (output)
            {
                case EnumTipoOutputFile.SqliteFile:
                    outputFile = new SqliteFileOutput();
                    outputFile.CreateFile(IdTestData, output);
                    break;

                case EnumTipoOutputFile.XlsFile:
                    outputFile = new XlsFileOutput();
                    outputFile.CreateFile(IdTestData, output);
                    break;

                case EnumTipoOutputFile.TxtFile:
                    outputFile = new TxtFileOutput();
                    outputFile.CreateFile(IdTestData, output);
                    break;

                default:
                    outputFile = new SqliteFileOutput();
                    outputFile.CreateFile(IdTestData, output);
                    break;
            }
        }
    }
}
