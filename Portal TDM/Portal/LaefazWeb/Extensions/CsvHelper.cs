using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Text;

namespace TDMWeb.Extensions
{
    /// <summary>
    /// Classe para geração de arquivos CSV
    /// </summary>
    public class CsvHelper<T> where T : class, new()
    {
        #region Variáveis

        private List<T> _lstItens;
        private SortedList<string, string> _mapaColunas;

        //Variável receberá os tipos que não devem ser verificados internamente pelo Reflection
        List<Type> lstType;

        #endregion

        #region Métodos Privados

        private void InicializarVariaveis()
        {
            lstType = new List<Type>();
            lstType.Add(typeof(string));
            lstType.Add(typeof(int));
            lstType.Add(typeof(int?));
            lstType.Add(typeof(double));
            lstType.Add(typeof(float));
            lstType.Add(typeof(decimal));
            lstType.Add(typeof(char));
            lstType.Add(typeof(long));
            lstType.Add(typeof(short));
            lstType.Add(typeof(DateTime));
            lstType.Add(typeof(DateTime?));
            lstType.Add(typeof(bool));
            lstType.Add(typeof(Boolean));

        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Gera o arquivo Csv
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo que será gerado</param>
        public void Gerar(string nomeArquivo)
        {
            bool _isEmpty = false;

            if (_lstItens.FirstOrDefault() == null)
            {
                _lstItens.Add(new T());
                _isEmpty = true;
            }

            Type type = _lstItens.FirstOrDefault().GetType();
            StringWriter sw = new StringWriter();
            int iColCount = type.GetProperties().Count();

            //Loop para poder inserir o nome das colunas no arquivo
            for (int i = 0; i < iColCount; i++)
            {
                PropertyInfo coluna = type.GetProperties().ElementAt(i);

                //É obrigatório a verificação do Contains da string, pois será dessa forma que poderão ser visualizadas as propriedades
                //de outras entidades
                if (coluna.Name == "ExtensionData" || _mapaColunas.Keys.Where(p => p.Contains(coluna.Name)).ToList().Count == 0)
                    continue;

                if (!lstType.Contains(coluna.PropertyType))
                {
                    //Busca o nome da chave no mapa de colunas referente ao nome da propriedade do objeto
                    var colunaChave = _mapaColunas.Keys.Where(p => p.Contains(coluna.Name)).ToList();

                    if (colunaChave.Count > 0)
                    {
                        //Grava o nome da coluna, o nome da coluna deve estar obrigatoriamente entre aspas
                        sw.Write(string.Format("\"{0}\"", _mapaColunas[colunaChave[0]]));
                    }
                    else
                    {
                        //Grava o nome da coluna, o nome da coluna deve estar obrigatoriamente entre aspas
                        sw.Write(string.Format("\"{0}\"", string.Empty));
                    }
                }
                else
                {
                    //Grava o nome da coluna, o nome da coluna deve estar obrigatoriamente entre aspas
                    sw.Write(string.Format("\"{0}\"", _mapaColunas[coluna.Name]));
                }

                if (i < iColCount - 1)
                    sw.Write(";");
            }

            //Pula a linha
            sw.Write(sw.NewLine);

            if (!_isEmpty)
            {
                //Loop para adicionar os itens da lista
                foreach (var item in _lstItens)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        PropertyInfo coluna = type.GetProperties().ElementAt(i);

                        if (coluna.Name == "ExtensionData" || _mapaColunas.Keys.Where(p => p.Contains(coluna.Name)).ToList().Count == 0)
                            continue;

                        object valor = string.Empty;

                        //Caso for uma entidade irá procurar o valor da coluna dentro de suas propriedades
                        if (!lstType.Contains(coluna.PropertyType))
                        {
                            PropertyInfo[] propriedades = coluna.GetValue(item, null).GetType().GetProperties();

                            //Loop para procurar internamente o valor a ser adicionado no arquivo
                            foreach (var prop in propriedades)
                            {
                                if (coluna.Name == "ExtensionData" || !_mapaColunas.Keys.Contains(string.Format("{0}.{1}", type.GetProperties().ElementAt(i).Name, prop.Name)))
                                    continue;

                                valor = prop.GetValue(coluna.GetValue(item, null), null);
                            }
                        }
                        else
                            valor = coluna.GetValue(item, null);

                        //Verifica se há valor a ser gravado, se não existir deverá inserir um nulo
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(valor)))
                            sw.Write(string.Format("\"{0}\"", Convert.ToString(valor)));
                        else
                            sw.Write(string.Format("\"{0}\"", string.Empty));

                        //Verifica se é necessário inserir o ponto e vírgula
                        if (i < iColCount - 1)
                            sw.Write(";");
                    }

                    //Pula a linha
                    sw.Write(sw.NewLine);
                }
            }

            byte[] byteArray = Encoding.Unicode.GetBytes(sw.ToString());
            MemoryStream s = new MemoryStream(byteArray);
            StreamReader sr = new StreamReader(s, Encoding.Unicode);

            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", nomeArquivo));
            curContext.Response.Charset = "";
            curContext.Response.ContentEncoding = Encoding.Unicode;
            curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            curContext.Response.ContentType = "text/comma-separated-values";
            curContext.Response.Write(sr.ReadToEnd());
            curContext.Response.End();
        }

        public MemoryStream GerarMemoryStream(string nomeArquivo)
        {
            bool _isEmpty = false;

            if (_lstItens.FirstOrDefault() == null)
            {
                _lstItens.Add(new T());
                _isEmpty = true;
            }

            Type type = _lstItens.FirstOrDefault().GetType();
            StringWriter sw = new StringWriter();
            int iColCount = type.GetProperties().Count();

            //Loop para poder inserir o nome das colunas no arquivo
            for (int i = 0; i < iColCount; i++)
            {
                PropertyInfo coluna = type.GetProperties().ElementAt(i);

                //É obrigatório a verificação do Contains da string, pois será dessa forma que poderão ser visualizadas as propriedades
                //de outras entidades
                if (coluna.Name == "ExtensionData" || _mapaColunas.Keys.Where(p => p.Contains(coluna.Name)).ToList().Count == 0)
                    continue;

                if (!lstType.Contains(coluna.PropertyType))
                {
                    //Busca o nome da chave no mapa de colunas referente ao nome da propriedade do objeto
                    var colunaChave = _mapaColunas.Keys.Where(p => p.Contains(coluna.Name)).ToList();

                    if (colunaChave.Count > 0)
                    {
                        //Grava o nome da coluna, o nome da coluna deve estar obrigatoriamente entre aspas
                        sw.Write(string.Format("\"{0}\"", _mapaColunas[colunaChave[0]]));
                    }
                    else
                    {
                        //Grava o nome da coluna, o nome da coluna deve estar obrigatoriamente entre aspas
                        sw.Write(string.Format("\"{0}\"", string.Empty));
                    }
                }
                else
                {
                    //Grava o nome da coluna, o nome da coluna deve estar obrigatoriamente entre aspas
                    sw.Write(string.Format("\"{0}\"", _mapaColunas[coluna.Name]));
                }

                if (i < iColCount - 1)
                    sw.Write(";");
            }

            //Pula a linha
            sw.Write(sw.NewLine);

            if (!_isEmpty)
            {
                //Loop para adicionar os itens da lista
                foreach (var item in _lstItens)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        PropertyInfo coluna = type.GetProperties().ElementAt(i);

                        if (coluna.Name == "ExtensionData" || _mapaColunas.Keys.Where(p => p.Contains(coluna.Name)).ToList().Count == 0)
                            continue;

                        object valor = string.Empty;

                        //Caso for uma entidade irá procurar o valor da coluna dentro de suas propriedades
                        if (!lstType.Contains(coluna.PropertyType))
                        {
                            PropertyInfo[] propriedades = coluna.GetValue(item, null).GetType().GetProperties();

                            //Loop para procurar internamente o valor a ser adicionado no arquivo
                            foreach (var prop in propriedades)
                            {
                                if (coluna.Name == "ExtensionData" || !_mapaColunas.Keys.Contains(string.Format("{0}.{1}", type.GetProperties().ElementAt(i).Name, prop.Name)))
                                    continue;

                                valor = prop.GetValue(coluna.GetValue(item, null), null);
                            }
                        }
                        else
                            valor = coluna.GetValue(item, null);

                        //Verifica se há valor a ser gravado, se não existir deverá inserir um nulo
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(valor)))
                            sw.Write(string.Format("\"{0}\"", Convert.ToString(valor)));
                        else
                            sw.Write(string.Format("\"{0}\"", string.Empty));

                        //Verifica se é necessário inserir o ponto e vírgula
                        if (i < iColCount - 1)
                            sw.Write(";");
                    }

                    //Pula a linha
                    sw.Write(sw.NewLine);
                }
            }

            byte[] byteArray = Encoding.Unicode.GetBytes(sw.ToString());
            MemoryStream s = new MemoryStream(byteArray);

            return s;
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Contrutor da classe CsvHelper
        /// </summary>
        /// <param name="lstItens">Lista de itens que será inserido no arquivo Csv</param>
        /// <param name="filtroColunas">Nome das colunas da lista que devem ser inseridas no arquivo Csv</param>
        /// <param name="nomeCabecalho">Nome a ser inserido nos cabeçalhos da coluna</param>
        public CsvHelper(List<T> lstItens, SortedList<string, string> mapaColunas)
        {
            this._lstItens = lstItens;
            this._mapaColunas = mapaColunas;

            InicializarVariaveis();
        }

        #endregion
    }
}