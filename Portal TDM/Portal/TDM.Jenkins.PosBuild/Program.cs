using LaefazWeb.Enumerators;
using LaefazWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.Threading;
using System.IO;
using System.Globalization;
using LaefazWeb.Extensions;
using LaefazWeb.Models.VOs;
using System.Data.SqlClient;
using OfficeOpenXml;
using TDMWeb.Extensions;

namespace TDM.Jenkins.PosBuild
{
    class Program
    {

        private static string Canal = ConfigurationSettings.AppSettings["CanalTelegram"];
        private static string TokenBotTelegram = ConfigurationSettings.AppSettings["TokenBotTelegram"];
        private static string Message = "";
        private static string Ip = "";
        private static string AmbienteVirtual = "";
        private static DadosExecucaoVO ExecucaoEmProcessamento;
        private static List<DadosExecucaoVO> ExecucaoAguardandoProcessamento;
        private static List<TestData> TestDataGeneratedSuccessfully = new List<TestData>();
        private static List<TestData> TestDataGeneratedFailed = new List<TestData>();
        private static List<TestData> TestDataNoRun = new List<TestData>();
        private static List<TestData> TestDataGeneratedSolicited = new List<TestData>();
        private static int? IdDataPool = null;
        private static bool EnvioTelegram;
        private static LogTDM Log = new LogTDM(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            DbEntities DbEntities = new DbEntities();
            try
            {
                #region Debug
                Log.Info("Iniciando processamento do Build 1.");
                Log.Info("Carregando Query do Tosca");
                #endregion

                DbEntities db = new DbEntities();

                #region Debug
                Log.Info("Carregamento do DbEntities realizado com sucesso!");
                #endregion

                Ip = GetIpLocal();
#if DEBUG
                Ip = "10.43.6.219";
#endif
                #region Debug
                Log.Info("IP recuperado com sucesso.");
                Log.Debug("Inciando processo do Build 1 na VDI " + Ip + ".");
                #endregion

                #region Debug
                Log.Info("Instanciando objetos de EncadeamentoVOs e ExecucaoVOs...");
                #endregion

                List<EncadeamentoVO> EncadeamentoVOs = new List<EncadeamentoVO>();
                List<ExecucaoVO> ExecucaoVOs = new List<ExecucaoVO>();

                #region Debug
                Log.Debug("Objetos Instanciados com sucesso...");
                Log.Debug("Iniciando criação dos paramEncad...");
                #endregion

                SqlParameter[] paramEncad =
                {
                    new SqlParameter("@IpVdi", Ip),
                };

                #region Debug
                Log.Debug("Objetos Instanciados com sucesso...");
                Log.Debug("Preparando para ");
                #endregion

                //Executa a PROC que retorna os encadeamentos
                EncadeamentoVOs = db.Database.SqlQuery<EncadeamentoVO>("EXEC PR_LISTAR_ENCADEAMENTO @IpVdi ", paramEncad).ToList();

                Encadeamento Encadeamento = null;
                bool ExecucaoEncadeada = false;

                //Percorrendo os encademanentos
                foreach (EncadeamentoVO EncadeamentoVO in EncadeamentoVOs)
                {
                    try
                    {
                        //Definindo parâmetros para passar na PROC de execução
                        SqlParameter[] paramExec =
                        {
                        new SqlParameter("@IpVdi", Ip),
                        new SqlParameter("@IdEncadeamento", EncadeamentoVO.IdEncadeamento),
                        };

                        ExecucaoEncadeada = EncadeamentoVO.IdEncadeamento != null;

                        //Verifica se existe encadeamento 
                        if (!ExecucaoEncadeada)
                            paramExec[1].Value = DBNull.Value;

                        #region Debug
                        Log.Info("Iniciando o carregamento dos registros da tabela de execuções...");
                        #endregion
                        //Executa a PROC que retorna as Execuções
                        ExecucaoVOs = db.Database.SqlQuery<ExecucaoVO>("EXEC PR_LISTAR_EXECUCAO @IpVdi, @IdEncadeamento ", paramExec).ToList();

                        #region Debug
                        Log.Info("Carregamento realizado com sucesso!");
                        Log.Debug("Foram recuperadas " + ExecucaoVOs.Count + " registros de Execuções.");
                        Log.Info("Entrando na Iteração das execuções!");

                        Log.Info("Verificando opção de Envio Telegram.");
                        #endregion

                        #region Verifica Envio Telegram
                        //Verifica opção de Envio de Telegram
                        EnvioTelegram = ExecucaoVOs.FirstOrDefault().EnvioTelegram;

                        string opcao;
                        if (EnvioTelegram)
                            opcao = "Enviar Notificação no Telegram.";
                        else
                            opcao = "Não Enviar Notificação no Telegram.";
                        #endregion

                        #region Debug
                        Log.Info("Para esse bloco de execuções foi escolhida a opção de " + opcao + "!");
                        #endregion

                        if (ExecucaoEncadeada)
                            Encadeamento = new Encadeamento();

                        foreach (ExecucaoVO item in ExecucaoVOs)
                        {
                            #region Debug
                            Log.Info("######################################## INICIANDO EXECUCAO DO TESTDATA " + item.IdTestData + " ###################################");
                            Log.Debug("Dados da Execução - ID Execução: " + item.Id);
                            Log.Debug("Dados da Execução - Nome da query do Tosca:" + item.NomeQueryTosca);
                            Log.Debug("Dados da Execução - Ambiente Virtual:" + item.AmbienteVirtual);
                            Log.Debug("Dados da Execução - Envio Telegram:" + item.EnvioTelegram);


                            if (item.Encadeado)
                                Log.Info("Dados da Execução - O TestData faz parte de uma execução encadeada.");

                            Log.DebugObject(item);
                            #endregion

                            //verifica se execução foi cancelada
                            int idStatusExecucao = db.Execucao.Find(item.Id).IdStatusExecucao;
                            if (idStatusExecucao == (int)EnumStatusExecucao.Cancelada)
                            {
                                int? Idusuario = db.TestData.Find(item.Id).IdUsuario;
                                Usuario usuario = db.Usuario.Find(Idusuario);
                                Log.Info("A execução de ID: " + item.Id + " foi cancelada pelo usuário '" + usuario.Login + "'");
                                break;
                            }


                            #region Define início execução
                            Execucao exec = db.Execucao.FirstOrDefault(x => x.Id == item.Id);
                            exec.InicioExecucao = DateTime.Now;
                            db.Execucao.Attach(exec);
                            db.Entry(exec).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            #endregion

                            #region Debug
                            Log.Info("Atualizando TestData com o ID da execução corrente.");
                            #endregion

                            TestData td = db.TestData.FirstOrDefault(x => x.Id == item.IdTestData);

                            #region Debug
                            Log.DebugObject(td);
                            #endregion

                            td.IdExecucao = item.Id;
                            db.TestData.Attach(td);
                            db.Entry(td).State = System.Data.Entity.EntityState.Modified;
                            //db.SaveChanges();

                            int IdScriptCondicaoScript = td.IdScript_CondicaoScript;

                            //Verifica se existem dados de saída de outras execuções
                            if (ExecucaoEncadeada)
                                Encadeamento.CarregarParametosEntrada(td.Id, IdScriptCondicaoScript);

                            #region Debug
                            Log.Info("Id de execução inserido com sucesso na tabela de TestData.");
                            Log.DebugObject(td);
                            #endregion

                            if (IdDataPool == null)
                                IdDataPool = td.IdDataPool;

                            if (AmbienteVirtual == "")
                                AmbienteVirtual = item.AmbienteVirtual;

                            #region Debug
                            Log.Info("Carregando Query do Tosca...");
                            Log.Info("Criando Arquivo de Query do Tosca...");
                            #endregion

                            #region Execução aplicativo TOSCA
                            if (item != null)
                            {
                                CriaArquivoQUERY(item.NomeQueryTosca, item.QueryTosca);

                                #region Debug
                                Log.Info("Query encontrada no banco de dados com sucesso!");
                                #endregion
                            }
                            else
                            {
                                Log.Info("Query não encontrada no banco de dados");
                                Console.ReadLine();
                            }

                            if (td != null)
                            {
                                Log.Info("TestData localizado.");
                                ExcluirPlanilha(td);
                                CriarPlanilhaExecucao(td);
                            }
                            else
                            {
                                Log.Info("TestData não localizado.");
                                Console.ReadLine();
                            }

                            //criação do script tcs que será usado pelo tcshell do tosca
                            Log.Info("Criando Arquivo TCS do Tcshell");
                            string tcs = ConfigurationSettings.AppSettings["ArquivoTCS"];

                            tcs = tcs.Replace("@QuebraLinha ", Environment.NewLine);
                            tcs = tcs.Replace("@LimpaLog", "\"Clear Log\"");
                            tcs = tcs.Replace("@ListaExecucaoTosca", "\"" + item.ListaExecucaoTosca + "\"");
                            tcs = tcs.Replace("@NomeRelatorio", "\"Print Report ... TDMREPORT\"");
                            tcs = tcs.Replace("@DiretorioRelatorio", "\"" + item.DiretorioRelatorio + "\"");

                            CriaArquivoTCS(tcs, item.CaminhoArquivoTCS);

                            #region Debug
                            Log.Debug("tcs = " + tcs);

                            //Executando o Kill do Agente do Tosca
                            Log.Info("Executando o Kill do Agente do Tosca...");
                            #endregion

                            KillTricentisAutomationAgent();

                            List<string> strcmdtext = new List<string>();
                            strcmdtext.Add(@"tcshell -workspace");
                            //strcmdtext.Add(@" ""C:\Tosca_Projects\Tosca_Workspaces\Workspace_TRG_Fev\portal_remote\portal_remote.tws"" ");
                            strcmdtext.Add(@ConfigurationSettings.AppSettings["ToscaWorkspace"]);

                            Log.Debug("Workspace escolhido = " + @ConfigurationSettings.AppSettings["ToscaWorkspace"]);

                            strcmdtext.Add(" -executionmode");
                            strcmdtext.Add(@" """ + item.CaminhoArquivoTCS + "\" ");

                            Log.Info("Criando Comando para executar o Tcshell");

                            var comando = string.Join(string.Empty, strcmdtext.ToArray());

                            Log.Debug("Comando = " + comando);
                            Process cmd = new Process();
                            cmd.StartInfo.FileName = "cmd.exe";
                            cmd.StartInfo.RedirectStandardInput = true;
                            cmd.StartInfo.RedirectStandardOutput = true;
                            cmd.StartInfo.CreateNoWindow = true;
                            cmd.StartInfo.UseShellExecute = false;
                            cmd.StartInfo.RedirectStandardError = true;
                            cmd.Start();

                            cmd.StandardInput.WriteLine(@"cd\");
                            //cmd.StandardInput.WriteLine(@"cd C:\Tosca_Projects\Tosca_Workspaces\Workspace_TRG_Fev\portal_remote\remote_exec");
                            cmd.StandardInput.WriteLine(@"cd " + ConfigurationSettings.AppSettings["RemoteExecReport"]);

                            #region Debug
                            Log.Debug("Dir Remote Execution Report: " + ConfigurationSettings.AppSettings["RemoteExecReport"]);
                            Log.Info("Executando o Tcshell");
                            #endregion

                            cmd.StandardInput.WriteLine(comando);
                            cmd.StandardInput.Flush();
                            cmd.StandardInput.Close();

                            Log.Info("Atualizando status da Execução...");
                            exec = null;
                            exec = db.Execucao.Where(x => x.Id == item.Id).FirstOrDefault();

                            //Log.DebugObject(exec);
                            exec.IdStatusExecucao = (int)EnumStatusExecucao.EmProcessamento;
                            db.Execucao.Attach(exec);
                            db.Entry(exec).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            #region Debug
                            Log.Info("Atualizando status da Execução...");

                            Log.DebugObject(exec);
                            #endregion

                            #region Debug
                            Log.Info("Recuperando ScriptCondicao_Script do TestData atual.");
                            #endregion

                            Script_CondicaoScript scs = db.Script_CondicaoScript.FirstOrDefault(x => x.Id == td.IdScript_CondicaoScript);

                            #region Debug       
                            Log.Info("ScriptCondicao_Script do TestData atual recuperado com sucesso.");
                            Log.DebugObject(scs);
                            Log.Info("Aguardando o Tosca Terminar a execução do script.");
                            #endregion

                            VerificaStatusTosca(scs, exec);

                            Log.Info(cmd.StandardOutput.ReadToEnd());
                            string output = cmd.StandardOutput.ReadToEnd().ToString();
                            Console.WriteLine("output: " + output);
                            string outputerro = cmd.StandardError.ReadToEnd().ToString();
                            Console.WriteLine("erro: " + outputerro);

#if DEBUG
                            Tosca tosca = new Tosca();
                            tosca.processa(IdScriptCondicaoScript, td.Id);
#endif
                            //Verifica se existem dados de saída do script e os carregam no Dictionary de parametros
                            if (ExecucaoEncadeada)
                                Encadeamento.CarregarParametrosSaida(td.Id, IdScriptCondicaoScript);

                            //Chamada do pos build 2
                            string UrlBuild2 = GetUrlAmbiente(item.AmbienteVirtual);
                            Log.Debug("Iniciando chamada do Pós Build 2 na url: " + UrlBuild2);

                            if (UrlBuild2 != null)
                                runJobJenkinsRemote(UrlBuild2);
                            //runJobJenkinsLocal(UrlBuild2, "brucilin.de.gouveia", "brucilin.de.gouveia");
                            else
                                throw new Exception("Não foi possível encontrar o ambiente virtual para realizar a chamada do PosBuild2.");
                            #endregion

                            Execucao execucao = db.Execucao.Where(x => x.Id == item.Id).FirstOrDefault();

                            #region Debug
                            Log.DebugObject(execucao);
                            #endregion

                            //Aguardando o status da execução atual ser atualizado pelo pós build 2
                            while (execucao.IdStatusExecucao == (int)EnumStatusExecucao.EmProcessamento || execucao.IdStatusExecucao == (int)EnumStatusExecucao.ProcessandoLogTosca)
                            {
                                Thread.Sleep(4000);

                                #region Debug
                                Log.Info("Aguardando execução do Pós build 2 para atualização da execução de ID " + exec.Id);
                                #endregion

                                db = new DbEntities();
                                execucao = db.Execucao.Where(x => x.Id == item.Id).FirstOrDefault();
                            }

                            #region Debug
                            Log.DebugObject(execucao);

                            Log.Info("Execução de Id " + exec.Id + " concluida com sucesso!");
                            #endregion

                            db = new DbEntities();
                            TestData TestData = db.TestData.FirstOrDefault(x => x.Id == item.IdTestData);

                            #region Salva Output do TestData no Agendamento
                            if (TestData.Execucao.IdAgendamento != null)
                            {
                                Agendamento ag = db.Agendamento.FirstOrDefault(x => x.Id == TestData.Execucao.IdAgendamento);
                                ag.Output += Log.GetMyLog().ToString();

                                db.Agendamento.Attach(ag);
                                //Prepara a entidade para uma Edição
                                db.Entry(ag).State = System.Data.Entity.EntityState.Modified;
                                // informa que o obejto será modificado
                                db.SaveChanges();

                            }
                            #endregion

                            #region Debug
                            Log.DebugObject(TestData);
                            #endregion

                            TestDataGeneratedSolicited.Add(TestData);

                            #region Debug
                            Log.DebugObject(TestDataGeneratedSolicited);
                            #endregion

                            switch (TestData.IdStatus)
                            {
                                case (int)EnumStatusTestData.Falha:
                                    TestDataGeneratedFailed.Add(TestData);

                                    #region Debug
                                    Log.DebugObject(TestDataGeneratedFailed);
                                    #endregion

                                    //Verifica se a execução faz parte de um encadeamento
                                    if (ExecucaoEncadeada)
                                        Encadeamento.InterromperEncadeamento(TestData);

                                    break;

                                case (int)EnumStatusTestData.Disponivel:
                                    TestDataGeneratedSuccessfully.Add(TestData);
                                    Log.DebugObject(TestDataGeneratedSuccessfully);
                                    break;
                            }

                            #region Debug
                            Log.Debug("Processando dados da execução...");
                            Log.Debug("Dados da Execução - ID Execução: " + item.Id);
                            Log.Debug("Dados da Execução - Nome da query do Tosca:" + item.NomeQueryTosca);
                            Log.Debug("Dados da Execução - Ambiente Virtual:" + item.AmbienteVirtual);
                            Log.Debug("Dados da Execução - Envio Telegram:" + item.EnvioTelegram);
                            Log.Info("######################################## FINALIZANDO EXECUCAO DO TESTDATA " + item.IdTestData + " ##################################");
                            Log.Info(Environment.NewLine);
                            Log.Info(Environment.NewLine);
                            #endregion
                        }



                    }
                    catch (EncadeamentoException ex)
                    {
                        LiberarExecucoesEmProcessamento(ex, EncadeamentoVO.IdEncadeamento);
                        LiberarExecucoesAguardandoProcessamento();
                    }

                }

                Message = GetExecutionSuccessMessage(IdDataPool, AmbienteVirtual);

            }
            catch (Exception ex)
            {

                LiberarExecucoesEmProcessamento(ex);
            }
            finally
            {
                LiberarExecucoesAguardandoProcessamento();
            }
        }

        #region Liberar Execuções Em Processamento
        private static void LiberarExecucoesEmProcessamento(Exception ex, int? IdEncadeamento = null)
        {
            #region Debug
            Log.Error("Erro: " + ex.Message);
            Console.ReadLine();

            Log.Error("\tMessage: " + ex.Message);
            Log.Error("\tInnerException: " + ex.InnerException);
            Log.Error(ex);
            #endregion
            DbEntities DbEntities = new DbEntities();

            ExecucaoEmProcessamento = new DadosExecucaoVO();
            ExecucaoAguardandoProcessamento = new List<DadosExecucaoVO>();

            SqlParameter[] paramEp =
            {
                new SqlParameter("@IpVdi", Ip),
                new SqlParameter("@IdEncadeamento", DBNull.Value),
                new SqlParameter("@IdStatusExecucao", DBNull.Value),
            };

            SqlParameter[] paramAp =
            {
                new SqlParameter("@IpVdi", Ip),
                new SqlParameter("@IdEncadeamento", DBNull.Value),
                new SqlParameter("@IdStatusExecucao", DBNull.Value),
            };


            if (IdEncadeamento != null)
            {
                paramEp[1].Value = IdEncadeamento;
                paramAp[1].Value = IdEncadeamento;
            }

            paramEp[2].Value = (int)EnumStatusExecucao.EmProcessamento;
            paramAp[2].Value = (int)EnumStatusExecucao.AguardandoProcessamento;

            ExecucaoEmProcessamento = DbEntities.Database.SqlQuery<DadosExecucaoVO>("EXEC PR_LISTAR_DADOS_EXECUCAO @IpVdi, @IdEncadeamento, @IdStatusExecucao ", paramEp).FirstOrDefault();
            ExecucaoAguardandoProcessamento = DbEntities.Database.SqlQuery<DadosExecucaoVO>("EXEC PR_LISTAR_DADOS_EXECUCAO @IpVdi, @IdEncadeamento, @IdStatusExecucao ", paramAp).ToList();

            #region Debug
            Log.Info("Valor da Query Carregada Dados Execução DadosExecucao Em Processamento: ");
            Log.DebugObject(ExecucaoEmProcessamento);
            #endregion

            // Caso não tenha nenhuma execução em Processamento
            if (ExecucaoEmProcessamento != null)
            {
                #region Setar FALHA no TestData
                DbEntities db = new DbEntities();
                TestData TestData = db.TestData.FirstOrDefault(x => x.Id == ExecucaoEmProcessamento.IdTestData);

                Log.DebugObject(TestData);

                TestData.IdStatus = (int)EnumStatusTestData.Falha;
                // anexar objeto ao contexto
                db.TestData.Attach(TestData);
                //Prepara a entidade para uma Edição
                db.Entry(TestData).State = System.Data.Entity.EntityState.Modified;
                // informa que o obejto será modificado
                db.SaveChanges();
                #endregion

                #region Debug
                Log.DebugObject(TestData);
                #endregion

                if (TestDataGeneratedFailed.Where(x => x.Id == TestData.Id).ToList().Count() == 0)
                    TestDataGeneratedFailed.Add(TestData);

                #region Setar FALHA na Execução e Liberar Ambiente
                db = new DbEntities();
                Execucao Execucao = db.Execucao.FirstOrDefault(x => x.Id == ExecucaoEmProcessamento.IdExecucao);
                Execucao.IdStatusExecucao = (int)EnumStatusExecucao.Falha;
                Execucao.SituacaoAmbiente = (int)EnumSituacaoAmbiente.Disponivel;
                Execucao.TerminoExecucao = DateTime.Now;
                // anexar objeto ao contexto
                db.Execucao.Attach(Execucao);
                //Prepara a entidade para uma Edição
                db.Entry(Execucao).State = System.Data.Entity.EntityState.Modified;
                // informa que o obejto será modificado
                db.SaveChanges();
                #endregion

                #region Exception Message
                Message = "Ocorreu um erro na execução do Datapool: " + ExecucaoEmProcessamento.DatapoolName + ", referente ao " + ExecucaoEmProcessamento.DemandaName + ". Por favor contactar o administrador do sistema.";
                #endregion
            }
        }
        #endregion

        #region Liberar Execuções Aguardando Processamento
        private static void LiberarExecucoesAguardandoProcessamento()
        {
            #region Debug
            //Liberando as massas que ficaram com o status em Geração com Execução Em Processamento
            Log.Debug("IdDataPool: " + IdDataPool);
            Log.Info("CarregaDadosExecucao: " + ExecucaoEmProcessamento);
            #endregion

            DbEntities DbEntities = new DbEntities();

            // Caso tenha alguma execução em Processamento
            if (IdDataPool == null && ExecucaoEmProcessamento != null)
            {
                #region Debug
                Log.Info("Foi atendida a condição (IdDataPool == null && CarregaDadosExecucao != null) do IF.");
                #endregion

                Execucao exec = DbEntities.Execucao.FirstOrDefault(x => x.Id == ExecucaoEmProcessamento.IdExecucao);
                Log.DebugObject(exec);

                IdDataPool = DbEntities.TestData.Where(x => x.Id == exec.IdTestData).FirstOrDefault().IdDataPool;
                List<TestData> TestDataList = new List<TestData>();
                TestDataList = DbEntities.TestData.Where(x => x.IdDataPool == IdDataPool).Where(x => x.IdStatus == (int)EnumStatusTestData.EmGeracao).Where(x => x.Execucao.Script_CondicaoScript_Ambiente.AmbienteVirtual.IP == ExecucaoEmProcessamento.AmbienteVirtual).ToList();

                Log.DebugObject(TestDataList);

                foreach (TestData element in TestDataList)
                {

                    #region Debug
                    Log.Info("Serialize Object Before:");
                    Log.DebugObject(element);
                    #endregion

                    element.IdStatus = (int)EnumStatusTestData.Cadastrada;
                    // anexar objeto ao contexto
                    DbEntities.TestData.Attach(element);
                    //Prepara a entidade para uma Edição
                    DbEntities.Entry(element).State = System.Data.Entity.EntityState.Modified;

                    // informa que o obejto será modificado
                    DbEntities.SaveChanges();

                    #region Debug
                    Log.Info("Serialize Object after:");
                    Log.DebugObject(element);
                    #endregion

                    Execucao Execucao = DbEntities.Execucao.Where(x => x.Id == element.IdExecucao).FirstOrDefault();
                    //Liberando o ambiente

                    #region Debug
                    Log.Info("Serialize Object Before:");
                    Log.DebugObject(element);
                    #endregion

                    Execucao.SituacaoAmbiente = (int)EnumSituacaoAmbiente.Disponivel;
                    Execucao.IdStatusExecucao = element.IdStatus == (int)EnumStatusTestData.Falha ? (int)EnumStatusExecucao.Falha : (int)EnumStatusExecucao.Sucesso;
                    DbEntities.Execucao.Attach(Execucao);
                    //Prepara a entidade para uma Edição
                    DbEntities.Entry(Execucao).State = System.Data.Entity.EntityState.Modified;
                    // informa que o obejto será modificado
                    DbEntities.SaveChanges();
                    #region Debug
                    Log.Info("Serialize Object After:");
                    Log.DebugObject(element);
                    #endregion

                }

            }

            if (ExecucaoAguardandoProcessamento != null)
            {
                foreach (DadosExecucaoVO element in ExecucaoAguardandoProcessamento)
                {

                    TestData td = DbEntities.TestData.FirstOrDefault(x => x.Id == element.IdTestData);
                    td.IdStatus = (int)EnumStatusTestData.Cadastrada;
                    // anexar objeto ao contexto
                    DbEntities.TestData.Attach(td);
                    //Prepara a entidade para uma Edição
                    DbEntities.Entry(td).State = System.Data.Entity.EntityState.Modified;
                    // informa que o obejto será modificado
                    DbEntities.SaveChanges();

                    TestDataNoRun.Add(td);

                    Execucao exec = DbEntities.Execucao.FirstOrDefault(x => x.Id == element.IdExecucao);
                    exec.SituacaoAmbiente = (int)EnumSituacaoAmbiente.Disponivel;
                    exec.IdStatusExecucao = (int)EnumStatusExecucao.Falha;
                    // anexar objeto ao contexto
                    DbEntities.Execucao.Attach(exec);
                    //Prepara a entidade para uma Edição
                    DbEntities.Entry(exec).State = System.Data.Entity.EntityState.Modified;
                    // informa que o obejto será modificado
                    DbEntities.SaveChanges();

                }
            }

            //Eviando a Mensagem para o Telegram
            if (EnvioTelegram)
            {
                Log.Debug("Sended Message to Telegram canal: " + Message);
                SMS.Enviar(Message, Canal, TokenBotTelegram);
            }
        }
        #endregion

        private static void runJobJenkinsLocal(string url, string user, string pass)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string mergedCredentials = string.Format("{0}:{1}", user, pass);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            string base64Credentials = Convert.ToBase64String(byteCredentials);
            request.Headers.Add("Authorization", "Basic " + base64Credentials);
            request.Method = "POST";
            request.ContentType = "application/xml";

            request.GetRequestStream();
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            string result = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
        }


        private static void runJobJenkinsRemote(string url)
        {
            WebRequest wrIniciaJob;

            wrIniciaJob = WebRequest.Create(url);

            wrIniciaJob.Method = "POST";

            WebResponse response = wrIniciaJob.GetResponse();

            response.Close();
        }

        private static void VerificaStatusTosca(Script_CondicaoScript scs, Execucao exec)
        {
            Log.Info("Execução:");
            Log.DebugObject(exec);

            Log.Info("Script_CondicaoScript:");
            Log.DebugObject(scs);

            Log.Info("Entrada no método VerificaStatusTosca.");

            DateTime InicioExecucao = exec.InicioExecucao ?? DateTime.Now;
            //DateTime TempoEstimadoExecucao = scs.TempoEstimadoExecucao ?? DateTime.Now;
            DateTime TempoEstimadoExecucao = scs.TempoEstimadoExecucao;
            int TempoEstimadoExec = Int32.Parse(String.Format("{0:mm}", TempoEstimadoExecucao));

            Log.Debug("Data Inicio da execução: " + InicioExecucao);
            Log.Debug("Tempo Estimado em Minutos: " + TempoEstimadoExec);

            DateTime TerminoEstimado = InicioExecucao.AddMinutes(TempoEstimadoExec);
            Log.Debug("Termino Estimado: " + TerminoEstimado);

            string dir = ConfigurationSettings.AppSettings["DirReport"];

            Log.Info("Condição do Loop:  while (!File.Exists(dir) || TerminoEstimado >= DateTime.Now)");
            Log.Debug("Execução iniciada em " + InicioExecucao);
            Log.Info("Aguardando o Tosca Terminar a execução...");

            while (!File.Exists(dir) && TerminoEstimado >= DateTime.Now)
            {
                Thread.Sleep(8000);
            }

            Log.Info("Saída do Loop de Execução.");
            Log.Info("Verifica se o arquivo do Tosca foi criado.");

            if (!File.Exists(dir))
            {

                Log.Warn("Entrada na condição. Foi verificado que o arquivo de report do Tosca não foi gerado.");

                Log.Warn("Encerrando execução do Tosca.");

                //
                KillTricentisAutomationAgent();
                Log.Warn("Aguardando a finalização da execução do Tosca...");
                Thread.Sleep(7000);
                Log.Info("Execução do Tosca finalizada com sucesso.");

                Log.Info("Criando arquivo de report da execução com erro.");

                string report;
                try
                {
                    foreach (string f in Directory.EnumerateFiles(ConfigurationSettings.AppSettings["Tosca_Evidences"], "*_" + exec.IdTestData + "_*"))
                        report = "\"executionFolder\",\"" + Path.GetFullPath(f) + "\"" + Environment.NewLine;
                }
                catch (Exception e)
                {
                    Log.Warn("A pasta da execução não foi gerada pelo Tosca Commander, favor verificar o problema!");
                }

                File.Create(dir).Close();

                Log.Info("Arquivo criado com sucesso: " + dir);

                report = "\"ID_TEST_DATA\",\"" + exec.IdTestData + "\",\"Failed\"" + Environment.NewLine;
                report += "A Execução foi abortada pois o Tosca não respondeu.";

                Log.Info("Texto do Relatório: " + report);

                File.WriteAllText(dir, report);
                Log.Info("Arquivo criado com sucesso.");

            }
            else
            {
                Log.Info("O arquivo de report foi gerado pelo ToscaCommamder com sucesso.");

            }

        }

        private static void KillTricentisAutomationAgent()
        {
            Log.Info("KillTricentisAutomationAgent()");

            string comando2 = "killtosca";

            Process cmd2 = new Process();
            cmd2.StartInfo.FileName = "cmd.exe";
            cmd2.StartInfo.RedirectStandardInput = true;
            cmd2.StartInfo.RedirectStandardOutput = true;
            cmd2.StartInfo.CreateNoWindow = true;
            cmd2.StartInfo.UseShellExecute = false;
            cmd2.StartInfo.RedirectStandardError = true;
            cmd2.Start();

            cmd2.StandardInput.WriteLine(@"cd\");

            cmd2.StandardInput.WriteLine(comando2);
            cmd2.StandardInput.Flush();
            cmd2.StandardInput.Close();

            Thread.Sleep(5000);

        }
        private static string GetUrlAmbiente(string AmbienteVirtual)
        {
            return ConfigurationSettings.AppSettings[AmbienteVirtual];
        }

        public static void CriaArquivoTCS(string cmd, string caminho_arquivo_tcs)
        {
            // Passa o texto para uma variável.
            string[] lines = { cmd };

            //Escreve as linhas no arquivo e escolhe aonde salvar.
            System.IO.File.WriteAllLines(@caminho_arquivo_tcs, lines);
        }

        public static void ExcluirPlanilha(TestData td)
        {
            try
            {
                Log.Info("Entrada no método de exclusão da planilha.");

                string inputData = ConfigurationSettings.AppSettings["ToscaInputData"];
                string nomeTecnicoScript = td.Script_CondicaoScript.NomeTecnicoScript;

                foreach (string f in Directory.EnumerateFiles(inputData, nomeTecnicoScript + ".xls"))
                {
                    Log.Info("Arquivo " + f + " localizado e agora será deletado.");
                    File.Delete(f);
                    Log.Info("Arquivo " + f + " excluído.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao excluir planilha: " + ex.Message);
            }
        }
        public static void CriarPlanilhaExecucao(TestData td)
        {
            try
            {
                using (var excelPackage = new ExcelPackage())
                {
                    DbEntities db = new DbEntities();
                    Log.Info("Entrada do método de criar planilha pra execução.");
                    excelPackage.Workbook.Properties.Author = "AUTOMAÇÃO";
                    excelPackage.Workbook.Properties.Title = "Planilha de Massa - ATMP";

                    // Aqui simplesmente adiciono a planilha inicial
                    var sheet = excelPackage.Workbook.Worksheets.Add(@ConfigurationSettings.AppSettings["SheetName"]);
                    sheet.Name = @ConfigurationSettings.AppSettings["SheetName"];
                    Log.Info("Aba da planilha criada com sucesso.");

                    // Títulos
                    List<ParametroValorVO> Parametros = (from p in db.Parametro
                                                  join ps in db.ParametroScript on p.Id equals ps.IdParametro
                                                  join pv in db.ParametroValor on ps.Id equals pv.IdParametroScript
                                                  where pv.IdTestData == td.Id && ps.IdTipoParametro == (int)EnumTipoParametro.Input
                                                  select new ParametroValorVO
                                                  {
                                                      DescricaoParametro = p.Descricao,
                                                      Valor = pv.Valor
                                                  }
                                                  ).ToList();
                    Log.Info("Lista de Parametros carregada com sucesso. Foram carregados: " + Parametros.Count + " parametros para esse TestData");

                    Script_CondicaoScript scs = db.Script_CondicaoScript.Where(x => x.Id == td.IdScript_CondicaoScript).FirstOrDefault();

                    Log.Info("Script_CondicaoScript carregada");

                    for (int i = 1; i < Parametros.Count; i++)
                    {
                        sheet.Cells[1, i].Value = Parametros[i].DescricaoParametro;
                        sheet.Cells[1, i].Worksheet.Cells["A1:Z1"].Style.Font.Bold = true;
                        sheet.Cells[2, i].Value = Parametros[i].Valor;
                    }
                    Log.Info("Fim da criação dos títulos das colunas");

                    string path = @ConfigurationSettings.AppSettings["ToscaInputData"];
                    string nomeTecnicoScript = "\\" + scs.NomeTecnicoScript + ".xls";
                    path += nomeTecnicoScript;
                    File.WriteAllBytes(path, excelPackage.GetAsByteArray());
                    Log.Info("Arquivo criado com sucesso.");
                }

                Log.Info("O arquivo Excel foi criado com sucesso.");
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao criar Planilha de execução. Erro: " + ex.Message);
            }
        }

        public static void CriaArquivoQUERY(string queryname, string cmd)
        {
            //Passa o texto para uma variável
            string[] lines = { cmd };
            char separador = '/';
            string[] querynamearray = queryname.Split(separador);

            queryname = querynamearray.Last();

            Log.Debug("Nome da Query = " + queryname);

            System.IO.File.WriteAllLines(@ConfigurationSettings.AppSettings["QueryTxtFolder"] + "query_" + queryname + ".txt", lines);

            Log.Debug("Arquivo criado com sucesso. Caminho absoluto: " + @ConfigurationSettings.AppSettings["QueryTxtFolder"] + "query_" + queryname + ".txt");
        }

        public static string GetIpLocal()
        {
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

        private static string GetExecutionSuccessMessage(int? idDataPool, string AmbienteVirtual)
        {
            DbEntities DbEntities = new DbEntities();
            DataPool DataPool = DbEntities.DataPool.FirstOrDefault(x => x.Id == idDataPool);
            List<TestData> TestData = DbEntities.TestData.Where(x => x.IdDataPool == idDataPool).ToList();

            //Pegando dados do relatório
            string Demanda = DataPool.Demanda == null ? "" : DataPool.Demanda.Descricao;
            string Script = DataPool.Script_CondicaoScript.Script.Descricao;
            //string Data = String.Format("{0:dd MMMM yyyy hh:mm:ss}", DateTime.Now);
            string Data = DateTime.Now.ToString(@"dd/MM/yyyy HH:mm:ss", new CultureInfo("PT-pt"));
            decimal QtdMassaSolicitada = TestDataGeneratedSolicited.Count();
            int CtsMassaSolicitadas = TestDataGeneratedSolicited.Where(x => x.ClassificacaoMassa == (int)EnumClassificacaoMassa.CT).Count();
            int BackupMassaSolicitadas = TestDataGeneratedSolicited.Where(x => x.ClassificacaoMassa == (int)EnumClassificacaoMassa.Backup).Count();

            decimal QtdMassaSucesso = TestDataGeneratedSuccessfully.Count();
            int? CtsMassaSucesso = TestDataGeneratedSuccessfully.Where(x => x.ClassificacaoMassa == (int)EnumClassificacaoMassa.CT).Count();
            int? BackupMassaSucesso = TestDataGeneratedSuccessfully.Where(x => x.ClassificacaoMassa == (int)EnumClassificacaoMassa.Backup).Count();

            decimal QtdMassaErro = TestDataGeneratedFailed.Count();
            int? CtsMassaErro = TestDataGeneratedFailed.Where(x => x.ClassificacaoMassa == (int)EnumClassificacaoMassa.CT).Count();
            int? BackupMassaErro = TestDataGeneratedFailed.Where(x => x.ClassificacaoMassa == (int)EnumClassificacaoMassa.Backup).Count();

            decimal QtdMassaNoRun = TestDataNoRun.Count();
            int? CtsMassaNoRun = TestDataNoRun.Where(x => x.ClassificacaoMassa == (int)EnumClassificacaoMassa.CT).Count();
            int? BackupMassaNoRun = TestDataNoRun.Where(x => x.ClassificacaoMassa == (int)EnumClassificacaoMassa.Backup).Count();

            //Montando a mensagem
            //string textoPlantao = "{0}| Backlog Target        {1} = {2} / {3}";
            //textoPlantao = string.Format(textoPlantao, !true ? "\U00002705" : "\U0001F534", 5, 5, 5);
            string message = "<b>Status de execução:</b>" + Environment.NewLine;
            message += "<b>VDI: " + AmbienteVirtual + "</b>" + Environment.NewLine;
            message += "<b>" + Demanda + "</b>" + Environment.NewLine;
            message += "Script: " + Script + Environment.NewLine;
            message += "Data: " + Data + Environment.NewLine;
            message += Environment.NewLine;
            message += "Qtd massa solicitadas: " + QtdMassaSolicitada + Environment.NewLine;
            message += "Cts: " + CtsMassaSolicitadas + Environment.NewLine;
            message += "Backup: " + BackupMassaSucesso + Environment.NewLine;
            message += Environment.NewLine;
            message += "Geradas com sucesso: " + QtdMassaSucesso + " (" + Decimal.Round((QtdMassaSucesso / QtdMassaSolicitada * 100), 2) + "%)" + Environment.NewLine;
            message += "Cts: " + CtsMassaSucesso + Environment.NewLine;
            message += "Backup: " + BackupMassaSucesso + Environment.NewLine;
            message += Environment.NewLine;
            message += "Com falhas: " + QtdMassaErro + " (" + Decimal.Round((QtdMassaErro / QtdMassaSolicitada * 100), 2) + "%)" + Environment.NewLine;
            message += "Cts: " + CtsMassaErro + Environment.NewLine;
            message += "Backup: " + BackupMassaErro + Environment.NewLine;
            message += Environment.NewLine;
            message += "Não Executados: " + QtdMassaNoRun + " (" + Decimal.Round((QtdMassaNoRun / QtdMassaSolicitada * 100), 2) + "%)" + Environment.NewLine;
            message += "Cts: " + CtsMassaNoRun + Environment.NewLine;
            message += "Backup: " + BackupMassaNoRun + Environment.NewLine;

            return message;
        }

    }

    //internal class CountTestData
    //{
    //    public int IdExecucao { get; internal set; }
    //    public int? IdTestData { get; internal set; }
    //}

}



