using LaefazWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.Jenkins.PosBuild
{
    class test
    {

        static void Main(string[] args)
        {

             // Instancia a Entity
            DbEntities db = new DbEntities();

            // Query para buscar o id do TestData,passando o id do Datapool que será executado.
            var tdQuery =
            (from td1 in db.TestData
            join dtp in db.DataPool on td1.IdDataPool equals dtp.Id
            where dtp.Id == 9 // alterar o dado fixo pelo id do datapool na tela
            select td1.Id).ToList();

            foreach (var tdata in tdQuery)
            {
                Console.WriteLine("Query = " + tdata);
            }
            

            Console.ReadLine();

        }

        public static void CriaArquivoQUERY(string cmd)
        {

            //Passa o texto para uma variável
            string[] lines = { cmd };

            System.IO.File.WriteAllLines(@"C:\Tosca_Projects\Tosca_Workspaces\testdm\Testes\querytosca.txt", lines);
        }

    

}
}
