using POC_SqLite.Enumerators;
using System.Collections.Generic;

namespace POC_SqLite.Output
{
    abstract class Output
    {
        public string Path { get; set; }
        public abstract void CreateFile(int IdTestData, EnumTipoOutputFile output);
        public abstract void CreateTable(string ScriptName, List<string> Columns);
        public abstract void InsertData(int IdTestData);
        public abstract void DeleteData(int Id);
        public abstract void SelectData();
    }
}
