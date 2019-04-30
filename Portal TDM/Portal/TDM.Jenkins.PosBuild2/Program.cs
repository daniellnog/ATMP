using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TDM.Jenkins.PosBuild;
using LaefazWeb.Models;
using System.Text.RegularExpressions;
using LaefazWeb.Enumerators;
using System.IO.Compression;
using System.Security.Permissions;
using System.Threading;
using System.Globalization;
using System.Configuration;
using LaefazWeb.Extensions;

namespace TDM.Jenkins.PosBuild2
{
    class Program
    {


        private static string Ip = "";
        private static DadosExecucao CarregaDadosExecucao;
        //Entidade de banco de dados
        private static DbEntities db = new DbEntities();
        private static List<TestData> TestDataGeneratedSuccessfully = new List<TestData>();
        private static List<TestData> TestDataGeneratedFailed = new List<TestData>();
        private static List<TestData> TestDataNoRun = new List<TestData>();
        private static List<TestData> TestDataGeneratedSolicited;
        private static string[] lines;
        private static LogTDM Log = new LogTDM(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            Log.Info("################################# Iniciando Processo de Pós Build 2 #######################################");
            try
            {
                //Exclusão do arquivo de query da execução
                Log.Info("//Exclusão do arquivo de query da execução");
                Log.Debug("Diretório da Query: " + ConfigurationSettings.AppSettings["QueryTxtFolder"]);

                string QueryDir = ConfigurationSettings.AppSettings["QueryTxtFolder"];

                foreach (string f in Directory.EnumerateFiles(QueryDir, "*.txt"))
                {
                    File.Delete(f);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Não foi possível apagar os arquivos de query da execução.");
                Log.Error("Erro: " + ex.Message);
            }

            //Console Log
            Log.Info("Criada a instância do DbEntities.");

            //Console Log
            Log.Info("Capturando Ip local da máquina de execução.");

            Ip = GetIpLocal();

            //Console Log
            Log.Info("Ip local da máquina recuperado com sucesso.");
            Log.Info("Processo de Pós Build 2 Iniciado na máquina " + Ip + ".");

            //Recebendo string com diretório completo do arquivo de Report
            string path = ConfigurationSettings.AppSettings["DirReport"];
            string dir = ConfigurationSettings.AppSettings["DirReport"];

            Log.Debug("Path arquivo de report:" + path);

            //Pasta compartilhada da VDI que corresponde a pasta C:\\EvidenceTDM\\ da VDI da aplicação
            string PathSaveEvidence = ConfigurationSettings.AppSettings["PathSaveEvidence"];

            Log.Debug("Path da pasta compartilhada de evidência:" + PathSaveEvidence);

            int? IdDataPool = null;

            try
            {
                // Console Log
                Log.Info("Entrada no Try Catch.");

                // Console Log
                Log.Info("Iniciando a leitura do arquivo do log report.");
                //Ler todas as linhas do arquivo e preenche um array

                lines = File.ReadAllLines(dir);

                // Console Log
                Log.Info("O arquivo de report foi acessado com sucesso.");

                //Criação das listas para realizar as atualizações do pos Build 2
                List<string> TestDataReport = new List<string>();
                List<string> ExecutionReport = new List<string>();

                // Console Log
                Log.Info("Entrada no report do Tosca.");

                //Capturando todas os TestData da execução e o diretório de suas respectivas evidências
                Array.ForEach(lines, element =>
                {
                    // Console Log
                    Log.Debug("[LOG_REPORT_TOSCA] - " + element);

                    if (element.Contains("ID_TEST_DATA"))
                        TestDataReport.Add(element);
                    else if (element.Contains("executionFolder"))
                        ExecutionReport.Add(element);
                });

                // Console Log
                Log.Debug("Foram encontrados no report registro de " + TestDataReport.Count() + " execuções.");

                //Valida que foram retornadas a quantidade correta de path de evidências para cada TestData
                Log.Info("Validando que foram retornadas a quantidade correta de path de evidências para cada TestData");
                if (TestDataReport.Count() > 0)
                {
                    Log.Info("Foram retornadas a quantidade correta de path de evidências para cada TestData");
                    // Console Log
                    Log.Info("Iniciando itaracão de execuções.");

                    //Iterando pelos reports do TestData
                    for (int i = 0; i < TestDataReport.Count(); i++)
                    {
                        string @CaminhoEvidence = ConfigurationSettings.AppSettings["CaminhoEvidence"];

                        Log.Debug("path da evidência:" + @CaminhoEvidence);

                        //Regex para pegar as informações das linhas
                        string delimiter = "[^\"|,]+";
                        MatchCollection TestDataAttributes = Regex.Matches(TestDataReport[i], delimiter);
                        MatchCollection ExecutionAttributes = Regex.Matches(ExecutionReport[i], delimiter);

                        // Console Log
                        Log.Info("Os dados do log report foram interpretados conforme esperado.");

                        int IdTestData = Int32.Parse(TestDataAttributes[1].Value);
                        string StatusTestData = TestDataAttributes[2].Value.Equals("Passed") ? "DISPONÍVEL" : "ERRO";
                        string StatusExecucao = TestDataAttributes[2].Value.Equals("Passed") ? "Sucesso" : "Falha";
                        string Evidencia = ExecutionAttributes[1].Value;
                        DateTime ExecutionDate = DateTime.Now;

                        // Console Log
                        Log.Info("Iniciando carregamento das entidades do banco.");

                        //Carregando entidades do model
                        Execucao Execucao = db.Execucao.Where(x => x.IdTestData == IdTestData).Where(x => x.SituacaoAmbiente == (int)EnumSituacaoAmbiente.EmUso).FirstOrDefault();

                        Log.DebugObject(Execucao);

                        //Atualizando status da execução para Aguardando processamento do Tosca

                        Execucao.IdStatusExecucao = (int)EnumStatusExecucao.ProcessandoLogTosca;

                        db.Execucao.Attach(Execucao);
                        //Prepara a entidade para uma Edição
                        db.Entry(Execucao).State = System.Data.Entity.EntityState.Modified;
                        // informa que o obejto será modificado
                        db.SaveChanges();

                        Log.DebugObject(Execucao);

                        //Carregando entidades do model
                        Execucao = db.Execucao.Where(x => x.IdTestData == IdTestData).Where(x => x.SituacaoAmbiente == (int)EnumSituacaoAmbiente.EmUso).FirstOrDefault();

                        Log.Info("Exibindo execuções após atualização.");
                        Log.DebugObject(Execucao);

                        // Console Log
                        Log.Info("Entidade Execução carregada com sucesso.");

                        TestData TestData = db.TestData.FirstOrDefault(x => x.Id == IdTestData);

                        Log.DebugObject(TestData);

                        // Pegando o número de execuções agendadas
                        if (TestDataGeneratedSolicited == null)
                        {
                            DataPool dt = db.DataPool.Where(x => x.Id == TestData.IdDataPool).FirstOrDefault();
                            TestDataGeneratedSolicited = db.TestData.Where(x => x.IdDataPool == dt.Id).Where(x => x.IdStatus == (int)EnumStatusTestData.EmGeracao).Where(x => x.Execucao.Script_CondicaoScript_Ambiente.AmbienteVirtual.IP == Ip).ToList();
                        }


                        // Console Log
                        Log.Info("Entidade TestData carregada com sucesso.");

                        StatusExecucao statusExecucao = db.StatusExecucao.FirstOrDefault(x => x.Descricao == StatusExecucao);
                        // Console Log
                        Log.Info("Entidade StatusExecução carregada com sucesso.");

                        Status statusTestData = db.Status.FirstOrDefault(x => x.Descricao == StatusTestData);
                        // Console Log
                        Log.Info("Entidade Status carregada com sucesso.");

                        Log.Debug("AUT = " + TestData.DataPool.AUT.Descricao);
                        Log.Debug("Demanda = " + TestData.DataPool.Demanda.Descricao);
                        Log.Debug("TipoFaseTeste = " + Execucao.TipoFaseTeste.Descricao);

                        string AUT = TestData.DataPool.AUT.Descricao;
                        string Demanda = TestData.DataPool.Demanda.Descricao;
                        string TipoFaseTeste = Execucao.TipoFaseTeste.Descricao;
                        // Console Log
                        Log.Info("Informações de AUT, Demanda e TipoFaseTeste carregados com sucesso.");

                        //STC-ETS-PRJ00007149_ENT00003722-06_04_2018_10_47
                        string ZipExtension = ".ZIP";
                        string FolderEvidenceName = AUT + "-E" + TipoFaseTeste + "-" + Demanda + "-" + String.Format("{0:dd_MM_yyyy_hh_mm_ss}", ExecutionDate);
                        string DirectoryTemp = Evidencia + "\\" + FolderEvidenceName;

                        // Console Log
                        Log.Info("Iniciando carregamento dos arquivos da pasta de evidências de testData.");

                        //Carregando os arquivos da pasta de evidência do teste Data
                        string[] files = System.IO.Directory.GetFiles(Evidencia);

                        // Console Log
                        Log.Info("Arquivos da pasta de TestData carregados com sucesso.");

                        //Criando um diretório temporario para colocar todos os arquivos e zipar
                        Directory.CreateDirectory(DirectoryTemp);

                        //Copiando os arquivos da pasta de evidencia para o diretório temporario
                        Array.ForEach(files, file =>
                        {
                            string fileName = System.IO.Path.GetFileName(file);
                            string destFile = System.IO.Path.Combine(DirectoryTemp, fileName);
                            System.IO.File.Copy(file, destFile, true);
                        });

                        // Console Log
                        Log.Info("Arquivos copiados com sucesso para a pasta temp.");

                        //Zipando a pasta com os arquivos de evidência do TestData
                        ZipFile.CreateFromDirectory(DirectoryTemp, DirectoryTemp + ZipExtension);

                        // Console Log
                        Log.Info("Arquivos zipado com sucesso para a pasta temp.");

                        //Monta o diretório com a data e hora da execução
                        string Now = String.Format("{0:dd_MM_yyyy}", ExecutionDate);
                        // Console Log
                        Log.Info("Entrada para verificação se a pasta do dia já existe.");

                        string folderTdmEvidencesRoot = System.IO.Directory.GetDirectories(PathSaveEvidence, Now).FirstOrDefault();

                        //Cria o caminho final da evidência
                        string folderTdmEvidences = PathSaveEvidence + "\\" + Now + "\\" + FolderEvidenceName + ZipExtension;

                        //Verifica se já existe pasta de excução do dia 
                        //Caso não exista cria uma
                        if (folderTdmEvidencesRoot == null)
                            Directory.CreateDirectory(PathSaveEvidence + "\\" + Now);

                        //Copiando a evidência para a pasta de Evidencias do portal
                        System.IO.File.Copy(DirectoryTemp + ZipExtension, folderTdmEvidences, true);
                        // Console Log
                        Log.Info("Copia do Zip realizada com sucesso para a pasta C:\\Evidences\\");

                        //Atualizando o registro de TestData
                        CaminhoEvidence += "/" + Now + "/" + FolderEvidenceName + ZipExtension;
                        TestData.IdStatus = statusTestData.Id;
                        TestData.CaminhoEvidencia = CaminhoEvidence;
                        //TestData.TerminoExecucao = DateTime.Now;

                        // anexar objeto ao contexto
                        db.TestData.Attach(TestData);
                        //Prepara a entidade para uma Edição
                        db.Entry(TestData).State = System.Data.Entity.EntityState.Modified;
                        // informa que o obejto será modificado
                        db.SaveChanges();

                        // Console Log
                        Log.Info("Atualização do objeto TestData realizado com sucesso!");

                        //Atualizando o registro de Execução
                        Execucao.IdStatusExecucao = statusExecucao.Id;
                        Execucao.SituacaoAmbiente = (int)EnumSituacaoAmbiente.Disponivel;
                        Execucao.ToscaOutput = string.Join("", lines);
                        Execucao.TerminoExecucao = DateTime.Now;
                        lines = null;
                        // anexar objeto ao contexto
                        db.Execucao.Attach(Execucao);
                        //Prepara a entidade para uma Edição
                        db.Entry(Execucao).State = System.Data.Entity.EntityState.Modified;
                        // informa que o obejto será modificado
                        db.SaveChanges();

                        // Console Log
                        Log.Info("Atualização do objeto Execução realizado com sucesso!");

                        //Preenchendo a lista de TestData para relatório
                        if (TestData.Status.Descricao == "DISPONÍVEL")
                            TestDataGeneratedSuccessfully.Add(TestData);
                        else
                            TestDataGeneratedFailed.Add(TestData);

                        IdDataPool = IdDataPool == null ? TestData.IdDataPool : IdDataPool;

                        //Sleep para garantir que as evidências não fiquem com Data e hora iguais
                        Thread.Sleep(1000);
                    }

                    // Console Log
                    Log.Info("Verificando se foi encontrado o Id do DataPool da execução.");

                }
                else
                {
                    // Console Log
                    Log.Error("Houve um erro na captura de parâmetros do relatório gerado pelo Tosca. Não foi possível encontrar as pastas de executionFoder para todos os testData.");
                    throw new Exception("Houve um erro na captura de parâmetros do relatório gerado pelo Tosca. Não foi possível encontrar as pastas de executionFoder para todos os testData.");
                }
            }

            catch (Exception ex)
            {

                Log.Error("\tMessage: " + ex.Message);
                Log.Error("\tInnerException: " + ex.InnerException);

                CarregaDadosExecucao = (
                  from exec in db.Execucao
                  join sca in db.Script_CondicaoScript_Ambiente on exec.IdScript_CondicaoScript_Ambiente equals sca.Id
                  join ambv in db.AmbienteVirtual on sca.IdAmbienteVirtual equals ambv.Id
                  join td in db.TestData on exec.IdTestData equals td.Id
                  join dtp in db.DataPool on td.IdDataPool equals dtp.Id
                  join dmd in db.Demanda on dtp.IdDemanda equals dmd.Id
                  join scs in db.Script_CondicaoScript on sca.IdScript_CondicaoScript equals scs.Id
                  join scr in db.Script on scs.IdScript equals scr.Id
                  where exec.SituacaoAmbiente == (int)EnumSituacaoAmbiente.EmUso && ambv.IP == Ip && exec.IdStatusExecucao != (int)EnumStatusExecucao.AguardandoProcessamento

                  select new DadosExecucao
                  {
                      IdTestData = exec.IdTestData
                      ,
                      IdExecucao = exec.Id
                      ,
                      DatapoolName = scr.Descricao
                      ,
                      DemandaName = dmd.Descricao

                  }).FirstOrDefault();

                //Eviando a Mensagem para o Telegran
                //SMS.Enviar(message, Canal, TokenBotTelegram);
            }
            finally
            {

                //Eviando a Mensagem para o Telegran
                //SMS.Enviar(Message, Canal, TokenBotTelegram);


                //Liberando as massas que ficaram com o status em Geração
                if (IdDataPool == null && CarregaDadosExecucao != null)
                {
                    //IdDataPool = db.TestData.Where(x => x.Id == exec.IdTestData).FirstOrDefault().IdDataPool;
                    //List<TestData> TestDataList = db.TestData.Where(x => x.IdDataPool == IdDataPool).Where(x => x.IdStatus == (int)EnumStatusTestData.EmGeracao).ToList();
                    List<TestData> TestDataList = db.TestData.Where(x => x.IdExecucao == CarregaDadosExecucao.IdExecucao).ToList();

                    TestDataList.ForEach(element =>
                    {
                        element.IdStatus = (int)EnumStatusTestData.Falha;
                        // anexar objeto ao contexto
                        db.TestData.Attach(element);
                        //Prepara a entidade para uma Edição
                        db.Entry(element).State = System.Data.Entity.EntityState.Modified;
                        // informa que o obejto será modificado
                        db.SaveChanges();

                        Execucao Execucao = db.Execucao.Where(x => x.Id == element.IdExecucao).FirstOrDefault();
                        //Liberando o ambiente
                        Execucao.SituacaoAmbiente = (int)EnumSituacaoAmbiente.Disponivel;
                        Execucao.IdStatusExecucao = element.IdStatus == (int)EnumStatusTestData.Falha ? (int)EnumStatusExecucao.Falha : (int)EnumStatusExecucao.Sucesso;
                        Execucao.TerminoExecucao = DateTime.Now;
                        if(lines!=null)
                            Execucao.ToscaOutput = string.Join("", lines);

                        db.Execucao.Attach(Execucao);
                        //Prepara a entidade para uma Edição
                        db.Entry(Execucao).State = System.Data.Entity.EntityState.Modified;
                        // informa que o obejto será modificado
                        db.SaveChanges();

                    });
                }
                else
                {
                    Log.Fatal("Ocorreu um erro durante a execução do processo de Pós BUILD 2 e não foi possível atualizar a base, liberar ambiente, falhar a execução e atualizar o TestData.");
                }

                try
                {
                    string DirReport = ConfigurationSettings.AppSettings["DirReport"];

                    int counter = 0;
                    string line;

                    // Read the file and display it line by line.  
                    System.IO.StreamReader file =
                        new System.IO.StreamReader(DirReport);

                    Log.Info("Iniciando a leitura do relatório do Tosca");

                    while ((line = file.ReadLine()) != null)
                    {
                        Log.Info(line);
                        //System.Console.WriteLine(line);
                        counter++;
                    }
                    Log.Info("Fim da leitura do relatório do tosca.");
                    file.Close();
                    //System.Console.WriteLine("There were {0} lines.", counter);
                    // Suspend the screen.  
                    //System.Console.ReadLine();

                    Log.Info("Iniciando método para exclusão do relatório do Tosca.");
                    Log.Info("Excluindo Arquivo: " + DirReport);
                    File.Delete(DirReport);
                    Log.Info("Arquivo deletado com sucesso!");

                }
                catch (Exception e)
                {
                    Log.Fatal("Ocorreu um erro ao tentar excluir o report do Tosca!");
                    Log.Fatal("InnerException: " + e.InnerException);
                    Log.Fatal("Message: " + e.Message);
                }

            }
        }

        public static string GetIpLocal()
        {

            //var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            //string ip_local = "Não foi possível identificar o ip da máquina";

            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        ip_local = ip.ToString();
            //    }
            //}

            //return ip_local;

            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            string ip_local = "Não foi possível identificar o ip da máquina";
            int i = 1;

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ip_local = ip.ToString();
                    Log.Info("IP" + i + ": " + ip_local);
                    i++;
                }
            }

            ip_local = @ConfigurationSettings.AppSettings["IpLocal"];

            return ip_local;
        }

        //private static void SerializeObject(Object objeto)
        //{
        //    try
        //    {
        //        Console.WriteLine("[DEBUG] - Imprimindo objeto: " + objeto.GetType());
        //        foreach (var item in objeto.GetType().GetProperties())
        //        {
        //            if (item.Name.Equals("ToscaInput") || item.Name.Equals("QueryTosca"))
        //                Console.WriteLine("[" + objeto.GetType().GetProperties() + " - Propriedade: " + item.Name + "; Valor: " + item.GetValue(objeto).ToString().Substring(0, 10) + "...");
        //            else if (item.Name.Contains("System"))
        //                Console.WriteLine("[" + objeto.GetType().GetProperties() + " - Propriedade: " + item.Name + "; Valor: System.Object");
        //            else
        //                Console.WriteLine("{" + objeto.GetType().GetProperties() + "] - Propriedade: " + item.Name + "; Valor: " + item.GetValue(objeto));
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("[WARNING] - Ocorreu um erro ao serializar o objeto.");
        //    }
        //}
    }

    internal class DadosExecucao
    {
        public string DatapoolName { get; internal set; }
        public string DemandaName { get; internal set; }
        public int IdExecucao { get; set; }
        public int? IdTestData { get; set; }
    }
}

