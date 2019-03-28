using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace TDMWeb.Extensions
{
    public static class EnumerableExtensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            System.ComponentModel.PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static string JoinStrings<T>(this IEnumerable<T> lista, string separador)
        {
            string header = String.Join(separador, typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x => x.Name).ToArray<string>()) + Environment.NewLine;

            string dados = header + String.Join(Environment.NewLine, lista.Select(x => x.ToString()).ToArray());

            return dados;
        }
    }
}