using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaefazWeb.Models.VOs
{
    public class TestDataParametroValorVO
    {
        // public TestData testData { get; set; }
        public int IdTestData { get; set; }
        public string DescricaoTestData { get; set; }
        public List<ParametroValorVO> parametroValor { get; set; }

    }
}