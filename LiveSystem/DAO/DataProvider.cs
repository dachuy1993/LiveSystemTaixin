using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LiveSystem.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataProvider();
                return instance;
            }
            private set => instance = value;
        }

        /// <summary>
        /// MySQL Execute Query
        /// </summary>
        public DataTable MySqlExecuteQuery(string str, string query, object[] parameter = null)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection con = new MySqlConnection(str))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Connection.Open();
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(item, MySqlDbType.VarChar).Value = parameter[i];
                            i++;
                        }
                    }
                }
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            return dt;
        }

        public int MySqlExecuteNonQuery(string str, string query, object[] parameter = null)
        {
            int a = 0;
            using (MySqlConnection con = new MySqlConnection(str))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Connection.Open();
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(item, MySqlDbType.VarChar).Value = parameter[i];
                            i++;
                        }
                    }
                }
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr[0] != null)
                        {
                            a++;
                        }
                    }
                }
                con.Close();
            }
            return a;
        }

        public List<string> MySqlGetList(string str, string query, object[] parameter = null)
        {
            List<string> list = new List<string>();
            using (MySqlConnection con = new MySqlConnection(str))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new MySqlParameter(item, MySqlDbType.VarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int i = 0;
                    list.Add(dr.GetString(0));
                    i += 1;
                }
                con.Close();
            }
            return list;
        }

        /// <summary>
        /// SQL Server Execute Query
        /// </summary>
        public DataTable executeQuery(string str, string query, object[] parameter = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }  
            return dt;
        }
        public DataTable ExecuteSP(string str, string query, object[] parameter = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                string[] listpara = query.Split(' ');
                SqlCommand cmd = new SqlCommand(listpara[0], con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameter != null)
                {
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            return dt;
        }

        public int ExecuteNonQuery(string str, string query, object[] parameter = null)
        {
            int a = 0;
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                a = cmd.ExecuteNonQuery();
                con.Close();
            }
            return a;
        }

        public object ExecuteScalar(string str, string query, object[] parameter = null)
        {
            object a = 0;
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                a = cmd.ExecuteScalar();
                con.Close();
            }
            return a;
        }

        public List<string> GetList(string str, string query, object[] parameter = null)
        {
            List<string> list = new List<string>();
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int i = 0;
                    list.Add(dr.GetString(0));
                    i += 1;
                }
                con.Close();
            }
            return list;
        }
    }
}
