using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using TDMWeb.Extensions;

namespace LaefazWeb.Models.VOs
{
    public class HistoricoVO
    {
        private const string INSERT_ACTION = "INSERT";
        private const string UPDATE_ACTION = "UPDATE";
        private const string DELETE_ACTION = "DELETE";

        public static Historico CreateInsertHistorico(object newEntity)
        {
            Historico Historico = new Historico()
            {
                Acao = INSERT_ACTION,
                Date = DateTime.Now,
                Entidade = Regex.Replace(newEntity.GetType().Name, "_\\w+", ""),
                IdDataPool = GetIdDataPool(newEntity),
                IdTestData = GetIdTestData(newEntity),
                IdExecucao = GetIdExecucao(newEntity),
                IdUsuario = Util.GetUsuarioLogado().Id,
                Valores = ToString(newEntity)

            };

            return Historico;
        }

        public static Historico CreateDeleteHistorico(object newEntity)
        {
            
            Historico Historico = new Historico()
            {
                Acao = DELETE_ACTION,
                Date = DateTime.Now,
                Entidade = Regex.Replace(newEntity.GetType().Name, "_\\w+", ""),
                IdDataPool = GetIdDataPool(newEntity),
                IdTestData = GetIdTestData(newEntity),
                IdExecucao = GetIdExecucao(newEntity),
                IdUsuario = Util.GetUsuarioLogado().Id,
                Valores = ToString(newEntity)


            };

            return Historico;
        }

        public static Historico CreateUpdateHistorico(object originalEntity, object newEntity)
        {

            Historico Historico = new Historico()
            {
                Acao = UPDATE_ACTION,
                Date = DateTime.Now,
                Entidade = Regex.Replace(newEntity.GetType().Name, "_\\w+", ""),
                IdDataPool = GetIdDataPool(originalEntity),
                IdTestData = GetIdTestData(originalEntity),
                IdExecucao = GetIdExecucao(originalEntity),
                IdUsuario = Util.GetUsuarioLogado().Id,
                Valores = ToString(originalEntity)


            };

            return Historico;
        }

        private static string Serialize(object obj)
        {

            return SerializeJson(obj);
            //return SerializeXml(obj);
        }

        private static string SerializeXml(object obj)
        {

            XmlSerializer xs = new XmlSerializer(obj.GetType());
            using (MemoryStream buffer = new MemoryStream())
            {
                xs.Serialize(buffer, obj);
                return ASCIIEncoding.ASCII.GetString(buffer.ToArray());
            }
        }

        private static string SerializeJson(object obj)
        {

            using (MemoryStream buffer = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
                ser.WriteObject(buffer, obj);
                return ASCIIEncoding.ASCII.GetString(buffer.ToArray());
            }
        }
    


    public static int GetIdDataPool(object obj)
        {

            string name = Regex.Replace(obj.GetType().Name, "_\\w+","");

            if(name.Equals("DataPool"))
            {

                return ((DataPool)obj).Id;

            }else if(name.Equals("TestData"))
            {
                return ((TestData)obj).IdDataPool;
            }
            else if(name.Equals("Execucao"))
            {
                DbEntities db = new DbEntities();
                TestData td = db.TestData.Where(x => x.Id == ((Execucao)obj).IdTestData).FirstOrDefault();
               return td.IdDataPool;
            }

            return -1;
        }


        public static int? GetIdTestData(object obj)
        {
            string name = Regex.Replace(obj.GetType().Name, "_\\w+", "");
            if (name.Equals("DataPool"))
            {

                return null;

            }
            else if (name.Equals("TestData"))
            {

                return ((TestData)obj).Id;

            }
            else if (name.Equals("Execucao"))
            {

                return ((Execucao)obj).IdTestData;

            }

            return -1;
        }


        public static int? GetIdExecucao(object obj)
        {
            string name = Regex.Replace(obj.GetType().Name, "_\\w+", "");
            if (name.Equals("Execucao"))
            {

                return ((Execucao)obj).Id;

            }

            return null;
        }


        public static String ToString(object obj, StringBuilder sb = null)
        {
            if (sb == null)
            {

                sb = new StringBuilder();
            }

            sb.Append(Regex.Replace(obj.GetType().Name, "_\\w+", "") + "["+ Environment.NewLine + "{");

            foreach( var item in obj.GetType().GetProperties())
            {
                string pattern = "_\\w+";
                string replacement = "";
                Regex rgx = new Regex(pattern);
                string name = Regex.Replace(item.Name, pattern, replacement);

                
                if (!item.ToString().Contains("Model"))
                {
                    sb.Append("'" + name + "':'" + item.GetValue(obj) + "'," + Environment.NewLine);
                }
                else
                {
                    object objeto = item.GetValue(obj);
                    if(objeto != null)
                    {
                        ToString(objeto, new StringBuilder(sb.ToString()));
                    }else
                    {
                        sb.Append("'" + name + "':'NULL'," + Environment.NewLine);
                    }
                        
                }

            }
            sb.Append("}]");

            return sb.ToString();
        }






    }
}