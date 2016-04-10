using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IIO11300project
{
    // These are some basic database queries. Most queries return DataTables. Nothing really special happening here.
    public static class DBHandler
    {
        // A function to get champion data.
        public static DataTable GetChampionData(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT name, title, image, loadingImage FROM champions WHERE id = @id";
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
        // A function to get rune data.
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
        // A function to get all mastery data.
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
        // A function to get summoner spell data.
        public static DataTable GetSummonerSpellData(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT name, descr, image FROM summonerspells WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
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
        // A function to get item data.
        public static DataTable GetItemData(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT name, descr, value, image FROM items WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
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
        // A function to get summoner profile icon data.
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
        // A function to insert matches to database.
        public static void InsertMatchesToDatabase(List<Match> matches, Summoner summoner)
        {
            try
            {
                // Gets all matchIDs from database to check if the match to insert already exists.
                List<string> matchIDs = GetMatchIDsFromDataBase(summoner.ID);
                foreach (var match in matches)
                {
                    // If the matchID isn't in the list (= isn't in database) inserts it then.
                    if (!matchIDs.Contains(match.ID))
                    {
                        // A list used to store item IDs.
                        List<string> ids = new List<string>();
                        int count = match.Items.Count();
                        for (int index = 0; index < count; index++)
                        {
                            ids.Add(match.Items[index].ID);
                        }
                        count = ids.Count();
                        // Check if the list has 7 items. If not, add some empty IDs there.
                        if (count != 7)
                        {
                            for (int index = count; index < 7; index++)
                            {
                                ids.Add("");
                            }
                        }

                        using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                        {
                            string query = "INSERT IGNORE INTO " +
                                           "matches(matchID, championID, spell1ID, spell2ID, gameMode, goldEarned, kills, deaths, assists, minions, neutralMinions, result, " +
                                           "timePlayed, creationDate, item1ID, item2ID, item3ID, item4ID, item5ID, item6ID, item7ID, summonerID) " +
                                           "VALUES (@matchID, @championID, @spell1ID, @spell2ID, @gameMode, @goldEarned, @kills, @deaths, @assists, @minions, @neutralMinions, @result, " +
                                           "@timePlayed, @creationDate, @item1ID, @item2ID, @item3ID, @item4ID, @item5ID, @item6ID, @item7ID, @summonerID)";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            conn.Open();
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@matchID", match.ID);
                            cmd.Parameters.AddWithValue("@championID", match.Champion.ID);
                            cmd.Parameters.AddWithValue("@spell1ID", match.Spells[0].ID);
                            cmd.Parameters.AddWithValue("@spell2ID", match.Spells[1].ID);
                            cmd.Parameters.AddWithValue("@gameMode", match.GameMode);
                            cmd.Parameters.AddWithValue("@goldEarned", match.GoldEarned);
                            cmd.Parameters.AddWithValue("@kills", match.Kills);
                            cmd.Parameters.AddWithValue("@deaths", match.Deaths);
                            cmd.Parameters.AddWithValue("@assists", match.Assists);
                            cmd.Parameters.AddWithValue("@minions", match.Minions);
                            cmd.Parameters.AddWithValue("@neutralMinions", match.NeutralMinions);
                            cmd.Parameters.AddWithValue("@result", match.Result);
                            cmd.Parameters.AddWithValue("@timePlayed", match.TimePlayed);
                            cmd.Parameters.AddWithValue("@creationDate", match.CreationDateSQL);
                            cmd.Parameters.AddWithValue("@item1ID", ids[0]);
                            cmd.Parameters.AddWithValue("@item2ID", ids[1]);
                            cmd.Parameters.AddWithValue("@item3ID", ids[2]);
                            cmd.Parameters.AddWithValue("@item4ID", ids[3]);
                            cmd.Parameters.AddWithValue("@item5ID", ids[4]);
                            cmd.Parameters.AddWithValue("@item6ID", ids[5]);
                            cmd.Parameters.AddWithValue("@item7ID", ids[6]);
                            cmd.Parameters.AddWithValue("@summonerID", summoner.ID);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        // A function to get all stored matches.
        public static DataTable GetMatchesFromDatabase(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT matchID, championID, spell1ID, spell2ID, gameMode, goldEarned, kills, deaths, assists, minions, neutralMinions, result, timePlayed, creationDate, item1ID, " +
                                   "item2ID, item3ID, item4ID, item5ID, item6ID, item7ID FROM matches WHERE summonerID = @summonerID ORDER BY creationDate DESC";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@summonerID", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable("Matches");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        // A function to get only stored matchIDs.
        public static List<string> GetMatchIDsFromDataBase(string id)
        {
            try
            {
                List<string> matchIDs = new List<string>();
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.Database))
                {
                    string query = "SELECT matchID FROM matches WHERE summonerID = @summonerID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@summonerID", id);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string matchID = reader.GetString(reader.GetOrdinal("matchID"));
                                matchIDs.Add(matchID);
                            }
                        }
                    }
                    return matchIDs;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}