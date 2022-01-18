using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System;

namespace Web.Infrastructure
{
    public class Database
    {
        private const string _CONNSTR = "Server=localhost\\SQLEXPRESS;Database=BrainWare;Integrated Security=SSPI";

        public IEnumerable<T> ExecuteReader<T>(string sql, SqlParameter[] qryParams, Func<IDataRecord, T> copyRowData){
            using (var con = new SqlConnection(_CONNSTR)) 
            using (var sqlQuery = new SqlCommand(sql, con)){
                con.Open();

                if (qryParams != null)
                    sqlQuery.Parameters.AddRange(qryParams);

                using (var rdr = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection)) {
                    while (rdr.Read()) 
                        yield return copyRowData(rdr);
                }
            }
        }

        public int ExecuteNonQuery(string query) {
            using (SqlConnection con = new SqlConnection(_CONNSTR)) {
                var sqlQuery = new SqlCommand(query, con);
                try {
                    int res = sqlQuery.ExecuteNonQuery();
                    return res;
                }
                finally {
                    con.Close();
                }
            }
        }
    }
}