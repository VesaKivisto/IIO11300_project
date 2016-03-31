using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IIO11300project
{
    public static class DBHandler
    {
        public static DataTable GetChampionData(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Data Source=localhost; User Id=root; Password=; Initial Catalog=test"))
                {
                    string query = "SELECT name, image FROM champions WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable("Champions");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetRuneData(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Data Source=localhost; User Id=root; Password=; Initial Catalog=test"))
                {
                    string query = "SELECT image FROM runes WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable("Runes");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetMasteryData(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Data Source=localhost; User Id=root; Password=; Initial Catalog=test"))
                {
                    string query = "SELECT image FROM masteries WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable("Masteries");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetAllMasteries()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Data Source=localhost; User Id=root; Password=; Initial Catalog=test"))
                {
                    string query = "SELECT id, image FROM masteries";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable("Masteries");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}