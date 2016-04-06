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
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
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
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetRuneData(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT image, descr FROM runes WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable("Runes");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetAllMasteries()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT id, image, descr1, descr2, descr3, descr4, descr5 FROM masteries";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable("Masteries");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetSummonerSpells()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT id, image FROM summonerspells";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable("Masteries");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetItemIcon(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT image FROM items WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    string itemIcon = (string)cmd.ExecuteScalar();
                    conn.Close();
                    return itemIcon;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetProfileIcon(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT image FROM profileicons WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    string profileicon = (string)cmd.ExecuteScalar();
                    conn.Close();
                    return profileicon;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void InsertMatchToDatabase(Match match, Summoner summoner)
        {
            try
            {
                Dictionary<int, string> icons = new Dictionary<int, string>();

                int count = match.Items.Count();
                for (int index = 0; index < count; index++)
                {
                    icons.Add(index, match.Items[index].Icon);
                }

                count = icons.Count();
                if (count != 7)
                {
                    for (int index = count; index < 7; index++)
                    {
                        icons.Add(index, "");
                    }
                }

                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "INSERT IGNORE INTO " + 
                                   "matches(matchID, championID, spell1Icon, spell2Icon, gameMode, goldEarned, kills, deaths, assists, creepScore, result, " + 
                                   "timePlayed, creationDate, item1Icon, item2Icon, item3Icon, item4Icon, item5Icon, item6Icon, item7Icon, summonerID) " +
                                   "VALUES (@matchID, @championID, @spell1Icon, @spell2Icon, @gameMode, @goldEarned, @kills, @deaths, @assists, @creepscore, @result, " +
                                   "@timePlayed, @creationDate, @item1Icon, @item2Icon, @item3Icon, @item4Icon, @item5Icon, @item6Icon, @item7Icon, @summonerID)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    conn.Open();
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@matchID", match.ID);
                    cmd.Parameters.AddWithValue("@championID", match.ChampionID);
                    cmd.Parameters.AddWithValue("@spell1Icon", match.Spell1Icon);
                    cmd.Parameters.AddWithValue("@spell2Icon", match.Spell2Icon);
                    cmd.Parameters.AddWithValue("@gameMode", match.GameMode);
                    cmd.Parameters.AddWithValue("@goldEarned", match.GoldEarned);
                    cmd.Parameters.AddWithValue("@kills", match.Kills);
                    cmd.Parameters.AddWithValue("@deaths", match.Deaths);
                    cmd.Parameters.AddWithValue("@assists", match.Assists);
                    cmd.Parameters.AddWithValue("@creepScore", match.CreepScore);
                    cmd.Parameters.AddWithValue("@result", match.Result);
                    cmd.Parameters.AddWithValue("@timePlayed", match.TimePlayed);
                    cmd.Parameters.AddWithValue("@creationDate", match.CreationDateSQL);
                    cmd.Parameters.AddWithValue("@item1Icon", icons[0]);
                    cmd.Parameters.AddWithValue("@item2Icon", icons[1]);
                    cmd.Parameters.AddWithValue("@item3Icon", icons[2]);
                    cmd.Parameters.AddWithValue("@item4Icon", icons[3]);
                    cmd.Parameters.AddWithValue("@item5Icon", icons[4]);
                    cmd.Parameters.AddWithValue("@item6Icon", icons[5]);
                    cmd.Parameters.AddWithValue("@item7Icon", icons[6]);
                    cmd.Parameters.AddWithValue("@summonerID", summoner.ID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}