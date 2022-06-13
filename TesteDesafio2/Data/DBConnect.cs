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
            string query2 = "UPDATE DadosMoeda SET DATA_REF = SUBSTRING(DATA_REF, 1, 10)";

            string query = $"BULK INSERT DadosMoeda from '{path}' " +
                            "with( " +
                                "FiRSTROW = 2," +
                                "FIELDTERMINATOR = ';'," +
                                "ROWTERMINATOR = '\n'" +
                            ")";


            using (SqlConnection conexao = new SqlConnection(_ConnectString))
            {
                conexao.Execute("truncate table DadosMoeda");
                conexao.Execute(query);
                conexao.Execute(query2);

            }

            System.Console.WriteLine("### Importação Realizada! ###");
        }

        public void ImportarDadosCotacao(string path)
        {
            string query = $"BULK INSERT DadosCotacao from '{path}' " +
                            "with( " +
                                "FiRSTROW = 2, " +
                                "FIELDTERMINATOR = ';', " +
                                "ROWTERMINATOR = '\n' " +
                            ")";

            string query2 = "update DadosCotacao set dat_cotacao = CONCAT ( SUBSTRING(dat_cotacao, 7, 4),'-', SUBSTRING(dat_cotacao, 4, 2), '-', SUBSTRING(dat_cotacao, 1, 2) )";


            using (SqlConnection conexao = new SqlConnection(_ConnectString))
            {
                conexao.Execute("truncate table DadosCotacao");
                conexao.Execute(query);
                conexao.Execute(query2);

            }

            System.Console.WriteLine("### Importação Realizada! ###");
        }

        public List<DadosMoedaResultado> ObterCotacao(Moeda moeda)
        {
            string query = "SELECT " +
                           "TAB.ID_MOEDA,                                                                                                             " +
                           "TAB.DATA_REF,                                                                                                             " +
                           "TAB.COD_COTACAO,                                                                                                          " +
                           "(select vlr_cotacao from DadosCotacao where TAB.cod_cotacao = cod_cotacao and TAB.DATA_REF = dat_cotacao) as vlr_cotacao  " +
                           "       from( " +
                           "select " +
                           " ID_MOEDA, " +
                           " DATA_REF, " +
                           " (select COD_COTACAO from De_Para where DM.ID_MOEDA = ID_MOEDA) as cod_cotacao " +
                           "from DadosMoeda DM " +
                           "       ) TAB " +
                           "   where " +
                           $"       (ID_MOEDA = '{moeda.moeda}' and(DATA_REF BETWEEN '{moeda.data_inicio}' AND '{moeda.data_fim}')) " +
                           "       ";

            using (SqlConnection conexao = new SqlConnection(_ConnectString))
            {
                return conexao.Query<DadosMoedaResultado>(query).ToList();
            }

        }
    }
}
