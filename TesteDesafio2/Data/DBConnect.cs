using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDesafio2.Model;

namespace TesteDesafio2.Data
{
    public class DBConnect
    {
        private string _ConnectString = "Server=ESN792N1249428\\SQLEXPRESS;Database=WiproBD;Trusted_Connection=True;";

        public void ImportarDadosMoeda(string path)
        {           
            string query = $"BULK INSERT DadosMoeda FROM '{path}' " +
                            "WITH( FiRSTROW = 2, FIELDTERMINATOR = ';')";
            
            //CORRECAO CAMPO DE DATA
            string query2 = "UPDATE DadosMoeda SET DATA_REF = SUBSTRING(DATA_REF, 1, 10)";
            try
            {
                using (SqlConnection conexao = new SqlConnection(_ConnectString))
                {
                    conexao.Execute("TRUNCATE TABLE DadosMoeda");
                    conexao.Execute(query);
                    conexao.Execute(query2);

                }
                System.Console.WriteLine("### Importação Realizada! ###");
            }
            catch (Exception ex){
                System.Console.WriteLine("### Falha na importação! Verique os banco de dados. ###");
                System.Console.WriteLine("Erro: ", ex.Message);
            }            
            
        }

        public void ImportarDadosCotacao(string path)
        {
            string query = $"BULK INSERT DadosCotacao from '{path}' " +
                            "WITH(FiRSTROW = 2, FIELDTERMINATOR = ';')";

            string query2 = "UPDATE DadosCotacao SET DAT_COTACAO = CONCAT ( SUBSTRING(DAT_COTACAO, 7, 4),'-', SUBSTRING(DAT_COTACAO, 4, 2), '-', SUBSTRING(DAT_COTACAO, 1, 2) )";

            try
            {
                using (SqlConnection conexao = new SqlConnection(_ConnectString))
                {
                    conexao.Execute("TRUNCATE TABLE DadosCotacao");
                    conexao.Execute(query);
                    conexao.Execute(query2);
                }

                System.Console.WriteLine("### Importação Realizada! ###");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("### Falha na importação! Verique os banco de dados. ###");
                System.Console.WriteLine("Erro: ", ex.Message);

            }



        }

        public List<DadosMoedaResultado> ObterCotacao(Moeda moeda)
        {
            string query = "SELECT " +
                           "TAB.ID_MOEDA,                                                                                                             " +
                           "TAB.DATA_REF,                                                                                                             " +
                           "TAB.COD_COTACAO,                                                                                                          " +
                           "(SELECT VLR_COTACAO FROM DadosCotacao WHERE TAB.COD_COTACAO = COD_COTACAO and TAB.DATA_REF = DAT_COTACAO) as VLR_COTACAO  " +
                           "       from( " +
                           "select " +
                           " ID_MOEDA, " +
                           " DATA_REF, " +
                           " (SELECT COD_COTACAO FROM DePara WHERE DM.ID_MOEDA = ID_MOEDA) as COD_COTACAO " +
                           "FROM DadosMoeda DM " +
                           "       ) TAB " +
                           "   WHERE " +
                           $"       (ID_MOEDA = '{moeda.moeda}' AND (DATA_REF BETWEEN '{moeda.data_inicio}' AND '{moeda.data_fim}')) " +
                           "       ";

            using (SqlConnection conexao = new SqlConnection(_ConnectString))
            {
                return conexao.Query<DadosMoedaResultado>(query).ToList();
            }

        }
    }
}
