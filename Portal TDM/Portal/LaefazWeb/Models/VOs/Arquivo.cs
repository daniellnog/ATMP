using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDMWeb.Models.VOs
{
    public class Arquivo
    {
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public string Path { get; set; }
        public Arquivo() { }
    }
}