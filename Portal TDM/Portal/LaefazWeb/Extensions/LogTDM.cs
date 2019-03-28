using LaefazWeb.Controllers;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using TDMWeb.Extensions;

namespace LaefazWeb.Extensions
{


    public class LogTDM : ILogTDM
    {

        private static ILog log;
        private static StringBuilder Mylog;

        public LogTDM(Type type)
        {
            log = log4net.LogManager.GetLogger(type);
            Mylog = new StringBuilder();
        }

        public void Debug(object message)
        {
            log.Debug(message);
            WriteLine(message, MethodBase.GetCurrentMethod().Name.ToUpper());
        }


        public void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
            WriteLine(message, MethodBase.GetCurrentMethod().Name.ToUpper());
            WriteLine(exception, MethodBase.GetCurrentMethod().Name.ToUpper());
        }

        public void DebugObject(Object obj)
        {
            try
            {
                string objectName = obj.GetType().BaseType.Name;
                log.Debug(objectName + ": " + ToString(obj));
                WriteLine(objectName + ": " + ToString(obj), MethodBase.GetCurrentMethod().Name.ToUpper());

            }
            catch (Exception e)
            {
                Warn("Não foi possível detalhar um objeto.");
            }
        }

        public void Error(object message)
        {
            log.Error(message);
            WriteLine(message, MethodBase.GetCurrentMethod().Name.ToUpper());
        }

        public void Error(Exception ex)
        {
            log.Error(ex);
            WriteLine(ex, MethodBase.GetCurrentMethod().Name.ToUpper());
        }

        public void Error(object text, Exception ex)
        {
            log.Error(text, ex);
            WriteLine(text, MethodBase.GetCurrentMethod().Name.ToUpper());
            WriteLine(ex, MethodBase.GetCurrentMethod().Name.ToUpper());
        }

        public void Fatal(object message)
        {
            log.Fatal(message);
            WriteLine(message, MethodBase.GetCurrentMethod().Name.ToUpper());
        }

        public void Fatal(Exception ex)
        {
            log.Fatal(ex);
            WriteLine(ex, MethodBase.GetCurrentMethod().Name.ToUpper());
        }


        public void Fatal(object text, Exception ex)
        {
            log.Fatal(text, ex);
            WriteLine(text, MethodBase.GetCurrentMethod().Name.ToUpper());
            WriteLine(ex, MethodBase.GetCurrentMethod().Name.ToUpper());
        }

        public void Info(object message)
        {
            log.Info(message);
            WriteLine(message, MethodBase.GetCurrentMethod().Name.ToUpper());
        }


        public void Info(object message, Exception exception)
        {
            log.Info(message, exception);
            WriteLine(message, MethodBase.GetCurrentMethod().Name.ToUpper());
            WriteLine(exception, MethodBase.GetCurrentMethod().Name.ToUpper());
        }

        public void Warn(object message)
        {
            log.Warn(message);
            WriteLine(message, MethodBase.GetCurrentMethod().Name.ToUpper());
        }

        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
            WriteLine(message, MethodBase.GetCurrentMethod().Name.ToUpper());
            WriteLine(exception, MethodBase.GetCurrentMethod().Name.ToUpper());

        }

        private void WriteLine(Object message, String Level)
        {
            string DateTimeNow = DateTime.Now.ToString(@"dd-MMMM-yyyy HH:mm:ss", new CultureInfo("PT-pt"));
            Console.WriteLine(DateTimeNow + " [" + Level.PadRight(5, ' ') + "] " + message);
            AppendLog(DateTimeNow + " [" + Level.PadRight(5, ' ') + "] " + message);
        }

        private void AppendLog(string message)
        {
            Mylog.AppendLine(message + Environment.NewLine);
        }

        public StringBuilder GetMyLog()
        {
            return Mylog;
        }
        private String ToString(object obj, StringBuilder sb = null)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
            }

            sb.Append(obj.GetType().Name + "[" + "{");

            foreach (var item in obj.GetType().GetProperties())
            {
                List<String> noPrint = new List<string>();
                noPrint.Add("QueryTosca");
                noPrint.Add("ToscaInput");

                //Console.WriteLine(item.ToString());
                if (!item.ToString().Contains("Model"))
                {
                    if (noPrint.Contains(item.Name))
                        sb.Append("'" + item.Name + "':'" + item.GetValue(obj).ToString().Substring(0, 10) + "...',");
                    else
                        sb.Append("'" + item.Name + "':'" + item.GetValue(obj) + "',");
                }
                else
                {
                    object objeto = item.GetValue(obj);
                    if (objeto != null)
                    {
                        //ToString(objeto, new StringBuilder(sb.ToString()));
                        sb.Append("'Object':'"+ item.Name + "',");
                    }
                    else
                    {
                        sb.Append("'" + item.Name + "':'NULL',");
                    }
                }
            }

            sb.Append("}]");
            return sb.ToString();
        }
    }
}