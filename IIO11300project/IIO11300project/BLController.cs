using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IIO11300project
{
    public class BLController
    {
        public static Summoner GetSummonerData(Summoner summoner)
        {
            if (String.IsNullOrEmpty(summoner.ID))
            {
                try
                {
                    JObject summonerData = RiotApiHandler.RequestSummonerData(summoner);
                    summoner.ID = summonerData[summoner.Name]["id"].ToString();
                    summoner.ProfileIcon = DBHandler.GetProfileIcon(summonerData[summoner.Name]["profileIconId"].ToString());
                    summoner.Name = summonerData[summoner.Name]["name"].ToString();
                    summoner.PlatformID = GetPlatformByRegion(summoner.Region);
                    return summoner;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return summoner;
            }
        }

        public static Summoner GetRankedData(Summoner summoner)
        {
            if (String.IsNullOrEmpty(summoner.Division))
            {
                try
                {
                    JObject rankedData = RiotApiHandler.RequestRankedData(summoner);
                    //MessageBox.Show(rankedData.ToString());
                    int temp;
                    bool parsed;

                    if (rankedData.ToString() != "{}")
                    {
                        parsed = int.TryParse(rankedData[summoner.ID][0]["entries"][0]["wins"].ToString(), out temp);
                        if (parsed)
                        {
                            summoner.Wins = temp;
                        }
                        else
                        {
                            MessageBox.Show("Cannot parse given value to integer!");
                        }

                        parsed = int.TryParse(rankedData[summoner.ID][0]["entries"][0]["losses"].ToString(), out temp);
                        if (parsed)
                        {
                            summoner.Losses = temp;
                        }
                        else
                        {
                            MessageBox.Show("Cannot parse given value to integer!");
                        }

                        summoner.LP = rankedData[summoner.ID][0]["entries"][0]["leaguePoints"].ToString();
                        summoner.LeagueName = rankedData[summoner.ID][0]["name"].ToString();
                        summoner.Tier = rankedData[summoner.ID][0]["tier"].ToString();

                        if (summoner.Tier != "CHALLENGER" && summoner.Tier != "MASTER")
                        {
                            summoner.Division = rankedData[summoner.ID][0]["entries"][0]["division"].ToString();
                            summoner.RankIcon = "/images/tier_icons/" + summoner.Tier.ToLower() + "_" + summoner.Division.ToLower() + ".png";
                        }
                        else
                        {
                            summoner.Division = "";
                            summoner.RankIcon = "/images/base_icons/" + summoner.Tier.ToLower() + ".png";
                        }
                    }
                    else
                    {
                        summoner.RankIcon = "/images/base_icons/provisional.png";
                        summoner.Division = "Unranked";
                    }

                    return summoner;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return summoner;
            }
        }

        public static List<Champion> GetChampionMastery(Summoner summoner, List<Champion> champions)
        {
            if (champions.Count == 0)
            {
                try
                {
                    JArray championMasteryData = RiotApiHandler.RequestChampionMastery(summoner);
                    foreach (var value in championMasteryData)
                    {
                        Champion champion = new Champion();
                        champion.ID = value["championId"].ToString();
                        champion.Level = value["championLevel"].ToString();
                        champion.TotalPoints = value["championPoints"].ToString();
                        champion.PointsToNextLevel = value["championPointsUntilNextLevel"].ToString();
                        DataTable table = DBHandler.GetChampionData(champion.ID);
                        foreach (DataRow row in table.Rows)
                        {
                            champion.Name = row["name"].ToString();
                            champion.Icon = row["image"].ToString();
                        }
                        champions.Add(champion);
                    }
                    return champions;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return champions;
            }
        }

        public static List<Masterypage> GetMasteryPages(Summoner summoner, List<Masterypage> masteryPages)
        {
            if (masteryPages.Count == 0)
            {
                try
                {
                    JObject masteryData = RiotApiHandler.RequestMasteryPages(summoner);
                    foreach (var pageData in masteryData[summoner.ID]["pages"])
                    {
                        List<Mastery> masteries = new List<Mastery>();
                        masteries = GetAllMasteryData();
                        if (pageData.Count() == 4)
                        {
                            int masteryCount = pageData["masteries"].Count();
                            for (int iter = 0; iter < masteryCount; iter++)
                            {
                                string id = pageData["masteries"][iter]["id"].ToString();
                                int index = masteries.FindIndex(mastery => mastery.ID.Equals(id, StringComparison.Ordinal));
                                masteries[index].Opacity = "1.0";
                                masteries[index].Rank = (int)pageData["masteries"][iter]["rank"];
                            }
                            Masterypage page = new Masterypage();
                            page.masteries = masteries;
                            masteryPages.Add(page);
                        }
                        else
                        {
                            Masterypage page = new Masterypage();
                            page.masteries = masteries;
                            masteryPages.Add(page);
                        }
                    }
                    return masteryPages;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return masteryPages;
            }
        }
        
        public static List<Runepage> GetRunePages(Summoner summoner, List<Runepage> runePages)
        {
            if (runePages.Count == 0)
            {
                try
                {
                    JObject runeData = RiotApiHandler.RequestRunePages(summoner);
                    foreach (var pageData in runeData[summoner.ID]["pages"])
                    {
                        Runepage page = new Runepage();
                        page.DescriptionCount = new Dictionary<string, int>();
                        if (pageData.Count() == 4)
                        {
                            page.Name = pageData["name"].ToString();
                            List<Rune> runes = new List<Rune>();
                            int runeCount = pageData["slots"].Count();
                            string descr = "";
                            int count = 0;
                            for (int iter = 0; iter < runeCount; iter++)
                            {
                                Rune rune = new Rune();
                                rune.ID = pageData["slots"][iter]["runeId"].ToString();
                                rune.SlotID = pageData["slots"][iter]["runeSlotId"].ToString();
                                DataTable table = DBHandler.GetRuneData(rune.ID);
                                foreach (DataRow row in table.Rows)
                                {
                                    rune.Icon = row["image"].ToString();
                                    rune.Descr = row["descr"].ToString();
                                    if (descr == row["descr"].ToString())
                                    {
                                        count++;
                                    }
                                    else if (descr != row["descr"].ToString() && count > 0)
                                    {
                                        page.DescriptionCount.Add(descr, count);
                                        descr = row["descr"].ToString();
                                        count = 1;
                                    }
                                    else if (descr != row["descr"].ToString())
                                    {
                                        descr = row["descr"].ToString();
                                        count++;
                                    }
                                }
                                rune.Position = GetRunePositionBySlotId(rune.SlotID);
                                runes.Add(rune);
                            }
                            if (!page.DescriptionCount.ContainsKey(descr))
                            {
                                page.DescriptionCount.Add(descr, count);
                            }
                            page.Runes = runes;
                            runePages.Add(page);
                        }
                        else
                        {
                            page.Name = pageData["name"].ToString();
                            page.DescriptionCount.Add("", 0);
                            runePages.Add(page);
                        }
                    }
                    return runePages;
                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return runePages;
            }
        }
        
        public static List<Match> GetMatchHistory(Summoner summoner, List<Match> matches)
        {
            if (matches.Count == 0)
            {
                try
                {
                    JObject matchData = RiotApiHandler.RequestMatchHistory(summoner);
                    DataTable table;
                    int temp;
                    bool parsed;

                    foreach (var game in matchData["games"])
                    {
                        Match match = new Match();
                        match.ID = game["gameId"].ToString();
                        match.ChampionID = game["championId"].ToString();

                        if (game["subType"].ToString() == "ARAM_UNRANKED_5x5")
                        {
                            match.GameMode = "ARAM";
                        }
                        else if (game["subType"].ToString() == "RANKED_SOLO_5x5")
                        {
                            match.GameMode = "Ranked";
                        }
                        else if (game["subType"].ToString() == "NORMAL")
                        {
                            match.GameMode = "Normal";
                        }
                        else {
                            match.GameMode = game["subType"].ToString();
                        }

                        match.Spell1ID = game["spell1"].ToString();
                        match.Spell2ID = game["spell2"].ToString();
                        match.GoldEarned = game["stats"]["goldEarned"].ToString();

                        if (!String.IsNullOrEmpty((string)game["stats"]["championsKilled"]))
                        {
                            parsed = int.TryParse(game["stats"]["championsKilled"].ToString(), out temp);
                            if (parsed)
                            {
                                match.Kills = temp;
                            }
                        }
                        if (!String.IsNullOrEmpty((string)game["stats"]["numDeaths"]))
                        {
                            parsed = int.TryParse(game["stats"]["numDeaths"].ToString(), out temp);
                            if (parsed)
                            {
                                match.Deaths = temp;
                            }
                        }
                        if (!String.IsNullOrEmpty((string)game["stats"]["assists"]))
                        {
                            parsed = int.TryParse(game["stats"]["assists"].ToString(), out temp);
                            if (parsed)
                            {
                                match.Assists = temp;
                            }
                        }

                        if (!String.IsNullOrEmpty((string)game["stats"]["minionsKilled"]) && !String.IsNullOrEmpty((string)game["stats"]["neutralMinionsKilled"]))
                        {
				            match.CreepScore = (int)game["stats"]["minionsKilled"] + (int)game["stats"]["neutralMinionsKilled"];
                        }
			            else if (!String.IsNullOrEmpty((string)game["stats"]["minionsKilled"]))
                        {
                            match.CreepScore = (int)game["stats"]["minionsKilled"];
                        }
                        else if (!String.IsNullOrEmpty((string)game["stats"]["neutralMinionsKilled"]))
                        {
                            match.CreepScore = (int)game["stats"]["neutralMinionsKilled"];
                        }
                        else {
                            match.CreepScore = 0;
                        }

                        TimeSpan ts = TimeSpan.FromSeconds((int)game["stats"]["timePlayed"]);
                        match.TimePlayed = ts.ToString(@"mm\:ss");

                        ts = TimeSpan.FromMilliseconds((double)game["createDate"]);
                        DateTime date = (new DateTime(1970, 1, 1) + ts).ToLocalTime();
                        match.CreationDate = date.ToString();
                        match.CreationDateSQL = date.ToString("yyyy-MM-dd HH:mm:ss");

                        if (game["stats"]["win"].ToString() == "True")
                        {
                            match.Result = "Win";
                        }
                        else
                        {
                            match.Result = "Loss";
                        }

                        table = DBHandler.GetChampionData(match.ChampionID);
                        foreach (DataRow row in table.Rows)
                        {
                            match.ChampionName = row["name"].ToString();
                            match.ChampionIcon = row["image"].ToString();
                        }

                        table = DBHandler.GetSummonerSpells();
                        foreach (DataRow row in table.Rows)
                        {
                            if (row["id"].ToString() == match.Spell1ID) match.Spell1Icon = row["image"].ToString();
                            if (row["id"].ToString() == match.Spell2ID) match.Spell2Icon = row["image"].ToString();
                        }

                        JToken value;
                        JObject jObj = (JObject)game["stats"];
                        List<Item> items = new List<Item>();
                        for (int itemId = 0; itemId < 7; itemId++)
                        {
                            if (jObj.TryGetValue(String.Format("item{0}", itemId), out value))
                            {
                                Item item = new Item();
                                item.ID = value.ToString();
                                item.Icon = DBHandler.GetItemIcon(item.ID);
                                items.Add(item);
                            }
                        }
                        match.Items = items;
                        matches.Add(match);
                        DBHandler.InsertMatchToDatabase(match, summoner);
                    }

                    return matches;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return matches;
            }
        }

        public static Dictionary<string, string> GetRegionsPlatforms()
        {
            Dictionary<string, string> regions = new Dictionary<string, string>();
            regions.Add("BR", "BR1");
            regions.Add("EUNE", "EUN1");
            regions.Add("EUW", "EUW1");
            regions.Add("KR", "KR");
            regions.Add("LAN", "LA1");
            regions.Add("LAS", "LA2");
            regions.Add("NA", "NA1");
            regions.Add("OCE", "OC1");
            regions.Add("RU", "RU");
            regions.Add("TR", "TR1");
            return regions;
        }

        public static string GetPlatformByRegion(string region)
        {
            Dictionary<string, string> regions = GetRegionsPlatforms();
            string platformID = regions[region.ToUpper()];
            return platformID;
        }

        public static List<Mastery> GetAllMasteryData()
        {
            DataTable table = DBHandler.GetAllMasteries();
            List<Mastery> masteries = new List<Mastery>();
            foreach (DataRow row in table.Rows)
            {
                Mastery mastery = new Mastery();
                mastery.Descriptions = new Dictionary<int, string>();
                mastery.Descriptions.Add(1, row["descr1"].ToString());
                mastery.Descriptions.Add(2, row["descr2"].ToString());
                mastery.Descriptions.Add(3, row["descr3"].ToString());
                mastery.Descriptions.Add(4, row["descr4"].ToString());
                mastery.Descriptions.Add(5, row["descr5"].ToString());
                mastery.ID = row["id"].ToString();
                mastery.Icon = row["image"].ToString();
                mastery.Opacity = "0.4";
                masteries.Add(mastery);
            }
            return masteries;
        }

        public static Dictionary<string, string> GetAllRunePositions()
        {
            Dictionary<string, string> positions = new Dictionary<string, string>();
            positions.Add("1", "42,339,694,9");
            positions.Add("2", "98,340,638,8");
            positions.Add("3", "164,342,572,6");
            positions.Add("4", "27,290,709,58");
            positions.Add("5", "84,284,652,64");
            positions.Add("6", "136,299,600,49");
            positions.Add("7", "48,242,688,106");
            positions.Add("8", "122,245,614,103");
            positions.Add("9", "86,208,650,140");
            positions.Add("10", "54,166,682,182");
            positions.Add("11", "118,164,618,184");
            positions.Add("12", "81,124,655,224");
            positions.Add("13", "131,102,605,246");
            positions.Add("14", "159,61,577,287");
            positions.Add("15", "210,36,526,312");
            positions.Add("16", "262,20,474,328");
            positions.Add("17", "319,3,417,345");
            positions.Add("18", "345,48,391,300");
            positions.Add("19", "381,5,355,343");
            positions.Add("20", "412,49,324,299");
            positions.Add("21", "440,6,296,342");
            positions.Add("22", "453,84,283,264");
            positions.Add("23", "480,42,256,306");
            positions.Add("24", "516,5,220,343");
            positions.Add("25", "560,36,176,312");
            positions.Add("26", "524,73,212,275");
            positions.Add("27", "542,120,194,228");
            positions.Add("28", "44,28,652,281");
            positions.Add("29", "175,200,521,111");
            positions.Add("30", "387,155,309,156");
            return positions;
        }

        public static string GetRunePositionBySlotId(string id)
        {
            Dictionary<string, string> positions = GetAllRunePositions();
            string position = positions[id];
            return position;
        }
    }
}
