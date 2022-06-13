using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TesteDesafio2.Data;
using TesteDesafio2.Model;

namespace Wipro.Desafio.Console
{
    class Program
    {        
        static string _diretorioCompleto = Environment.CurrentDirectory;
        static string _diretorioProjeto = Directory.GetParent(_diretorioCompleto).Parent.Parent.FullName;

        private static void Main(string[] args)
        {
            System.Console.WriteLine("########################################");
            System.Console.WriteLine("###########Iniciando Desafio2###########");
            System.Console.WriteLine("########################################\n");

            ImportarArquivos();

            while (true)
            {
                Task task1 = Task.Factory.StartNew(() => ConsularAPI());
                Task.WaitAll(task1);
                Task.Delay(120000).Wait();
            }
        }

        private static void ImportarArquivos()
        {
           
            string pathDadosMoeda = _diretorioProjeto + @"\DadosMoeda.csv";
            string pathDadosCotacao = _diretorioProjeto + @"\DadosCotacao.csv";            
            
            DBConnect con = new DBConnect();
            
            System.Console.WriteLine("### Realizando importação do arquivo: DadosMoeda.csv! ###");
            con.ImportarDadosMoeda(pathDadosMoeda);

            System.Console.WriteLine("### Realizando importação do arquivo: DadosCotacao.csv! ###");
            con.ImportarDadosCotacao(pathDadosCotacao);

        }

        private static void ExportarArquivos(Moeda moeda, string path)
        {
            
            List<string> outLines = new List<string>();

            DBConnect conn = new DBConnect();
            var resultado = conn.ObterCotacao(moeda);

            int count = 1;
            foreach (DadosMoedaResultado item in resultado)
            {

                if (count == 1)
                {
                    outLines.Add("ID_MOEDA; DATA_REF; VLR_COTACAO");
                }

                outLines.Add(item.id_moeda + ";" +
                             item.data_ref + ";" +
                             item.vlr_cotacao);
                System.Console.WriteLine(count + $") Dados Importados: {item.id_moeda} - {item.data_ref} - {item.vlr_cotacao}");
                count++;
            }

            System.IO.File.WriteAllLines(path, outLines.ToArray());

        }

        private static void ConsularAPI()
        {

            string JsonString = "";
            try
            {
                JsonString = getMoeda().Result;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("### ERRO - Não foi possivel comunicar com a API.");
                System.Console.WriteLine("> Verifique se o projeto da API está em execução.");
                System.Console.WriteLine("> Descrição: ", e.Message);
                
            }


            if (!String.IsNullOrEmpty(JsonString))
            {
                dynamic DynamicData = JsonConvert.DeserializeObject(JsonString);

                string moeda = DynamicData.moeda;
                string data_inicio = DynamicData.data_inicio;
                string data_fim = DynamicData.data_fim;

                if (String.IsNullOrEmpty(moeda))
                {
                    System.Console.WriteLine("### ATENÇÃO: API esta com fila zerada");
                }
                else
                {
                    System.Console.WriteLine("### Processando a moeda: " + moeda);

                    DateTime dateTime = DateTime.Now;

                    string pathDadosResposta = _diretorioProjeto + @"\Resultado_" + dateTime.ToString("yyyyMMdd_HHmmss") + ".csv";

                    Moeda _moeda = new Moeda();
                    _moeda.moeda = moeda;
                    _moeda.data_inicio = data_inicio;
                    _moeda.data_fim = data_fim;

                    ExportarArquivos(_moeda, pathDadosResposta);
                }

            }
        }

        public static async Task<string> getMoeda()
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7001/api/Moeda/GetItemFila");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

    }
}

