using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaefazWeb.Models.AppSettingsKey
{
    public static class AppSettingsKey
    {
        public static string QuantidadeQuebraExcel { get; }
        public static string Encadeamento1 { get; }
        public static string Encadeamento2 { get; set; }
        public static string HistoricoUsuario { get; set; }
        public static string ListaExecucaoTosca { get; set; }
        public static string CaminhoArquivoTCS { get; set; }
        public static string DiretorioRelatorio { get; set; }
        public static string hostJenkins { get; set; }
        public static string portJenkins { get; set; }
        public static string VDI_141 { get; set; }
        public static string VDI_219 { get; set; }
        public static string VDI_220 { get; set; }
        public static string VDI_237 { get; set; }

        private static string GetAppSettingsKey(string key)
        {
            return "";
        }
    }
}