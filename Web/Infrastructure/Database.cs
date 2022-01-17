using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System;

namespace Web.Infrastructure
{
    public class Database
    {
        private const string CON_STR = "Server=localhost\\SQLEXPRESS;Database=BrainWare;Integrated Security=SSPI";
        public Database(){}

        public IEnumerable<T> ExecuteReader<T>(string sql, SqlParameter[] qryParams, Func<IDataRecord, T> copyRowData){
            using (var con = new SqlConnection(CON_STR)) 
            using (var sqlQuery = new SqlCommand(sql, con)){
                con.Open();

                if (qryParams != null)
                    sqlQuery.Parameters.AddRange(qryParams);

                using (var rdr = sqlQuery.ExecuteReader()) {
                    while (rdr.Read()) {
                        yield return copyRowData(rdr);
                    }
                }
            }
        }

        public int ExecuteNonQuery(string query){
            using (SqlConnection con = new SqlConnection(CON_STR)) {
                var sqlQuery = new SqlCommand(query, con);
                int res = sqlQuery.ExecuteNonQuery();
                con.Close();
                return res;
            }
        }
    }
}