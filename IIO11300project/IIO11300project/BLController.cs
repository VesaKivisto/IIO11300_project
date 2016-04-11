using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace IIO11300project
{
    // None of these functions use JSON serialization, which could have been one way to get all the needed data.
    // This, however, would have meant to use classes that had variables for every key in the JSON data.
    // Almost all the requests return data which I don't need so I didn't want to use serialization.
    public class BLController
    {
        // A function used to get summoner data. The if statement isn't really needed, but it's there just in case.
        public static Summoner GetSummonerData(Summoner summoner)
        {
            if (String.IsNullOrEmpty(summoner.ID))
            {
                try
                {
                    // API request
                    JObject summonerData = RiotApiHandler.RequestSummonerData(summoner);
                    summoner.ID = summonerData[summoner.Name]["id"].ToString();
                    summoner.ProfileIcon = DBHandler.GetProfileIcon(summonerData[summoner.Name]["profileIconId"].ToString());
                    summoner.Level = summonerData[summoner.Name]["summonerLevel"].ToString();
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
        // A function used to get summoner ranked data. The if statement isn't really needed, but it's there just in case.
        // Lots of int.TryParsing here, so this section may look a bit confusing.
        public static Summoner GetRankedData(Summoner summoner)
        {
            if (String.IsNullOrEmpty(summoner.Division))
            {
                try
                {
                    // API request
                    JObject rankedData = RiotApiHandler.RequestRankedData(summoner);
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
                        // TextInfo is used to convert summoner tier data from all caps to only first letter uppercase.
                        TextInfo ti = new CultureInfo("en-US", false).TextInfo;
                        summoner.Tier = ti.ToTitleCase(summoner.Tier.ToLower());
                        // Checks the tier to see which tier image to use.
                        if (summoner.Tier != "Challenger" && summoner.Tier != "Master")
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
                    // If ranked data is empty, use these values to showe that the summoner is currently unranked.
                    else
                    {
                        summoner.RankIcon = "/images/base_icons/provisional.png";
                        summoner.Tier = "Unranked";
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
        // A function used to get summoner champion mastery data. The if statement is there to determine if this function has been executed => less API requests.
        public static List<Champion> GetChampionMastery(Summoner summoner, List<Champion> champions)
        {
            if (champions.Count == 0)
            {
                try
                {
                    // API request
                    JArray championMasteryData = RiotApiHandler.RequestChampionMastery(summoner);
                    foreach (var value in championMasteryData)
                    {
                        Champion champion = new Champion();
                        champion.ID = value["championId"].ToString();
                        champion.MasteryLevel = value["championLevel"].ToString();
                        champion.TotalPoints = value["championPoints"].ToString();
                        champion.PointsToNextLevel = value["championPointsUntilNextLevel"].ToString();
                        // Gets all champion data from database. The API doesn't return champion name, title or icon via this request.
                        DataTable table = DBHandler.GetChampionData(champion.ID);
                        foreach (DataRow row in table.Rows)
                        {
                            champion.Name = row["name"].ToString();
                            champion.Title = row["title"].ToString();
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
        // A function used to get summoner mastery pages. The if statement is there to determine if this function has been executed => less API requests.
        public static List<Masterypage> GetMasteryPages(Summoner summoner, List<Masterypage> masteryPages)
        {
            if (masteryPages.Count == 0)
            {
                try
                {
                    // API request
                    JObject masteryData = RiotApiHandler.RequestMasteryPages(summoner);
                    foreach (var pageData in masteryData[summoner.ID]["pages"])
                    {
                        List<Mastery> masteries = new List<Mastery>();
                        // Gets every single possible mastery. Used to determine which masteries the summoner has active.
                        masteries = GetAllMasteryData();
                        // Checks is pageData has masteries array in it. Empty pages do not have any masteries => no masteries array.
                        if (pageData.Count() == 4)
                        {
                            int masteryCount = pageData["masteries"].Count();
                            for (int iter = 0; iter < masteryCount; iter++)
                            {
                                string id = pageData["masteries"][iter]["id"].ToString();
                                // Finds masteryID from masteries list. Sets the opacity to 1.0 aka makes that mastery active.
                                int index = masteries.FindIndex(mastery => mastery.ID.Equals(id, StringComparison.Ordinal));
                                masteries[index].Opacity = "1.0";
                                masteries[index].Rank = (int)pageData["masteries"][iter]["rank"];
                            }
                            Masterypage page = new Masterypage();
                            page.Name = pageData["name"].ToString();
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
        // A function used to get summoner rune pages. The if statement is there to determine if this function has been executed => less API requests.
        public static List<Runepage> GetRunePages(Summoner summoner, List<Runepage> runePages)
        {
            if (runePages.Count == 0)
            {
                try
                {
                    // API request
                    JObject runeData = RiotApiHandler.RequestRunePages(summoner);
                    foreach (var pageData in runeData[summoner.ID]["pages"])
                    {
                        Runepage page = new Runepage();
                        page.DescriptionCount = new Dictionary<string, int>();
                        // Checks is pageData has runes array in it. Empty pages do not have any runes => no runes array.
                        if (pageData.Count() == 4)
                        {
                            List<Rune> runes = new List<Rune>();
                            string descr = "";
                            int count = 0;
                            int runeCount = pageData["slots"].Count();
                            page.Name = pageData["name"].ToString();
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
        // A function used to get summoner match history. The if statement is there to determine if this function has been executed => less API requests.
        public static List<Match> GetMatchHistory(Summoner summoner, List<Match> matches)
        {
            if (matches.Count == 0)
            {
                try
                {
                    // API request
                    JObject matchHistoryData = RiotApiHandler.RequestMatchHistory(summoner);
                    int temp;
                    bool parsed;
                    foreach (var game in matchHistoryData["games"])
                    {
                        Match match = new Match();
                        Champion champion = new Champion();
                        champion.ID = game["championId"].ToString();
                        match.ID = game["gameId"].ToString();
                        // Gamemodes have their own silly codes. Trying to make them more sensible.
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
                        match.GoldEarned = game["stats"]["goldEarned"].ToString();
                        // Some of the data isn't always returned. Lots of checking needed.
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
			            if (!String.IsNullOrEmpty((string)game["stats"]["minionsKilled"]))
                        {
                            match.Minions = (int)game["stats"]["minionsKilled"];
                        }
                        else if (!String.IsNullOrEmpty((string)game["stats"]["neutralMinionsKilled"]))
                        {
                            match.NeutralMinions = (int)game["stats"]["neutralMinionsKilled"];
                        }
                        else
                        {
                            match.Minions = 0;
                            match.NeutralMinions = 0;
                        }
                        // TimeSpan used to convert play time and creation date to more sensible formats. Also
                        TimeSpan ts = TimeSpan.FromSeconds((int)game["stats"]["timePlayed"]);
                        match.TimePlayed = ts.ToString(@"mm\:ss");
                        ts = TimeSpan.FromMilliseconds((double)game["createDate"]);
                        DateTime date = (new DateTime(1970, 1, 1) + ts).ToLocalTime();
                        match.CreationDate = date.ToString("d");
                        match.CreationDateSQL = date.ToString("yyyy-MM-dd HH:mm:ss");

                        if (game["stats"]["win"].ToString() == "True")
                        {
                            match.Result = "Win";
                        }
                        else
                        {
                            match.Result = "Loss";
                        }
                        // Gets champion name etc. based on id.
                        match.Champion = GetChampionInfo(champion.ID);
                        // Some simple stuff to iterate through summoner spells.
                        JToken value;
                        JObject jObj = (JObject)game;
                        List<Spell> spells = new List<Spell>();
                        for (int index = 1; index < 3; index++)
                        {
                            if (jObj.TryGetValue(String.Format("spell{0}", index), out value))
                            {
                                string source = value.ToString();
                                // Get summoner spell data from database.
                                GetSpellInfo(spells, source);
                            }
                        }
                        match.Spells = spells;
                        // Same thing for items.
                        jObj = (JObject)game["stats"];
                        List<Item> items = new List<Item>();
                        for (int index = 0; index < 7; index++)
                        {
                            if (jObj.TryGetValue(String.Format("item{0}", index), out value))
                            {
                                string source = value.ToString();
                                // Gets item data from database.
                                GetItemInfo(items, source);
                            }
                        }
                        match.Items = items;
                        matches.Add(match);
                    }
                    // Inserts every match to database (if the matchID + summonerID unique doesn't exist). More about this in DBHandler.
                    DBHandler.InsertMatchesToDatabase(matches, summoner);
                    // Gets all saved matched from database. Returns a new list with only matches from database.
                    matches = GetMatchesFromDatabase(summoner);
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
        // A function used to get match details. Has some similarities with match history function.
        public static Matchdetails GetMatchDetails(Summoner summoner, Match match)
        {
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;
            Matchdetails details = new Matchdetails();
            // API request.
            JObject matchDetailData = RiotApiHandler.RequestMatchDetails(summoner, match.ID);
            details.ID = matchDetailData["matchId"].ToString();
            details.GameMode = matchDetailData["matchMode"].ToString();
            details.GameMode = ti.ToTitleCase(details.GameMode.ToLower());
            // TimeSpan used again for play time and creation date.
            TimeSpan ts = TimeSpan.FromSeconds((int)matchDetailData["matchDuration"]);
            details.TimePlayed = ts.ToString(@"mm\:ss");
            ts = TimeSpan.FromMilliseconds((double)matchDetailData["matchCreation"]);
            DateTime date = (new DateTime(1970, 1, 1) + ts).ToLocalTime();
            details.CreationDate = date.ToString("d");
            details.CreationDateSQL = date.ToString("yyyy-MM-dd HH:mm:ss");
            details.Participants = new List<Participant>();
            // Iterates through every participant (= player in match).
            foreach (var participantData in matchDetailData["participants"])
            {
                Stats stats = new Stats();
                Champion champion = new Champion();
                champion.ID = participantData["championId"].ToString();
                Participant participant = new Participant();
                // PARTICIPANT DATA STARTS
                // Teams are only given as IDs, this turns them to something sensible.
                if ((int)participantData["teamId"] == 100)
                {
                    participant.Team = "Blue Team";
                }
                else if ((int)participantData["teamId"] == 200)
                {
                    participant.Team = "Red Team";
                }
                // Some simple stuff to iterate through summoner spells. Pretty much the same as with match history.
                JToken value;
                JObject jObj = (JObject)participantData;
                List<Spell> spells = new List<Spell>();
                for (int index = 1; index < 3; index++)
                {
                    if (jObj.TryGetValue(String.Format("spell{0}Id", index), out value))
                    {
                        string source = value.ToString();
                        GetSpellInfo(spells, source);
                    }
                }
                participant.Spells = spells;
                // Get champion data. Same as with match history.
                participant.Champion = GetChampionInfo(champion.ID);

                if (participantData["timeline"]["role"].ToString() == "DUO_CARRY")
                {
                    participant.Role = "AD Carry";
                }
                else if (participantData["timeline"]["role"].ToString() == "DUO_SUPPORT")
                {
                    participant.Role = "Support";
                }
                else
                {
                    participant.Role = ti.ToTitleCase(participantData["timeline"]["role"].ToString().ToLower());
                }
                participant.Lane = ti.ToTitleCase(participantData["timeline"]["lane"].ToString().ToLower());
                // PARTICIPANT DATA ENDS
                // If statements to get the result.
                if (participantData["stats"]["winner"].ToString() == "True" && (int)participantData["teamId"] == 100 && String.IsNullOrEmpty(details.Result))
                {
                    details.Result = "Blue Team";
                }
                else if (participantData["stats"]["winner"].ToString() == "True" && (int)participantData["teamId"] == 200 && String.IsNullOrEmpty(details.Result))
                {
                    details.Result = "Red Team";
                }
                // STATS DATA STARTS
                // Some simple stuff to iterate through items. Pretty much the same as with match history.
                jObj = (JObject)participantData["stats"];
                List<Item> items = new List<Item>();
                for (int index = 0; index < 7; index++)
                {
                    if (jObj.TryGetValue(String.Format("item{0}", index), out value))
                    {
                        string source = value.ToString();
                        GetItemInfo(items, source);
                    }
                }
                stats.Items = items;
                stats.Kills = (int)participantData["stats"]["kills"];
                stats.Deaths = (int)participantData["stats"]["deaths"];
                stats.Assists = (int)participantData["stats"]["assists"];
                stats.Minions = (int)participantData["stats"]["minionsKilled"];
                stats.NeutralMinions = (int)participantData["stats"]["neutralMinionsKilled"];
                stats.LargestMultiKill = participantData["stats"]["largestMultiKill"].ToString();
                stats.LargestKillingSpree = participantData["stats"]["largestKillingSpree"].ToString();
                stats.MagicDamageDealt = participantData["stats"]["magicDamageDealt"].ToString();
                stats.PhysicalDamageDealt = participantData["stats"]["physicalDamageDealt"].ToString();
                stats.TrueDamageDealt = participantData["stats"]["trueDamageDealt"].ToString();
                stats.TotalDamageDealt = participantData["stats"]["totalDamageDealt"].ToString();
                stats.MagicDamageDealtToChampions = participantData["stats"]["magicDamageDealtToChampions"].ToString();
                stats.PhysicalDamageDealtToChampions = participantData["stats"]["physicalDamageDealtToChampions"].ToString();
                stats.TrueDamageDealtToChampions = participantData["stats"]["trueDamageDealtToChampions"].ToString();
                stats.TotalDamageDealtToChampions = participantData["stats"]["totalDamageDealtToChampions"].ToString();
                stats.MagicDamageTaken = participantData["stats"]["magicDamageTaken"].ToString();
                stats.PhysicalDamageTaken = participantData["stats"]["physicalDamageTaken"].ToString();
                stats.TrueDamageTaken = participantData["stats"]["trueDamageTaken"].ToString();
                stats.TotalDamageTaken = participantData["stats"]["totalDamageTaken"].ToString();
                stats.LargestCriticalStrike = participantData["stats"]["largestCriticalStrike"].ToString();
                stats.TotalHeal = participantData["stats"]["totalHeal"].ToString();
                stats.NeutralMinionsTeamJungle = participantData["stats"]["neutralMinionsKilledTeamJungle"].ToString();
                stats.NeutralMinionsEnemyJungle = participantData["stats"]["neutralMinionsKilledEnemyJungle"].ToString();
                stats.GoldEarned = participantData["stats"]["goldEarned"].ToString();
                stats.GoldSpent = participantData["stats"]["goldSpent"].ToString();
                stats.VisionWardsBought = participantData["stats"]["visionWardsBoughtInGame"].ToString();
                stats.TowerKills = participantData["stats"]["towerKills"].ToString();
                stats.InhibitorKills = participantData["stats"]["inhibitorKills"].ToString();
                stats.WardsPlaced = participantData["stats"]["wardsPlaced"].ToString();
                stats.WardsKilled = participantData["stats"]["wardsKilled"].ToString();
                stats.TotalTimeCrowdControlDealt = participantData["stats"]["totalTimeCrowdControlDealt"].ToString();
                // STATS DATA ENDS
                participant.Stats = stats;
                details.Participants.Add(participant);
            }
            return details;
        }
        // A dictionary used to store all servers and their respective platforms. Servers and platforms are needed for the API requests.
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
        // Gets platforms based on a region.
        public static string GetPlatformByRegion(string region)
        {
            Dictionary<string, string> regions = GetRegionsPlatforms();
            string platformID = regions[region.ToUpper()];
            return platformID;
        }
        // Gets all mastery data from database.
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
                // Opacity is set to 0.4 to determine inactive masteries. Active masteries are determined in GetMasteryPages function.
                mastery.Opacity = "0.4";
                masteries.Add(mastery);
            }
            return masteries;
        }
        // A dictionary used to store rune positions for each runes based on their runeSlotID.
        // I had a few problems with getting the runes to display the way I wanted, kind of happy with this solutions.
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
        // Gets rune position based on runeSlotID.
        public static string GetRunePositionBySlotId(string id)
        {
            Dictionary<string, string> positions = GetAllRunePositions();
            string position = positions[id];
            return position;
        }
        // Gets matches from database. Nothing really interesting happening here.
        public static List<Match> GetMatchesFromDatabase(Summoner summoner)
        {
            List<Match> matches = new List<Match>();
            DataTable table = DBHandler.GetMatchesFromDatabase(summoner.ID);
            foreach (DataRow row in table.Rows)
            {
                List<Spell> spells = new List<Spell>();
                List<Item> items = new List<Item>();
                Champion champion = new Champion();
                Match match = new Match();
                match.ID = row["matchID"].ToString();
                champion.ID = row["championID"].ToString();
                match.GameMode = row["gameMode"].ToString();
                match.GoldEarned = row["goldEarned"].ToString();
                match.Kills = (int)row["kills"];
                match.Deaths = (int)row["deaths"];
                match.Assists = (int)row["assists"];
                match.Minions = (int)row["minions"];
                match.NeutralMinions = (int)row["neutralMinions"];
                match.Result = row["result"].ToString();
                match.TimePlayed = row["timePlayed"].ToString();
                match.CreationDateSQL = row["creationDate"].ToString();

                DateTime date = Convert.ToDateTime(match.CreationDateSQL);
                match.CreationDate = date.ToString("d");
                match.Champion = GetChampionInfo(champion.ID);
                // Iterates through summoner spells.
                for (int index = 1; index < 3; index++)
                {
                    string source = row[String.Format("spell{0}ID", index)].ToString();
                    GetSpellInfo(spells, source);
                }
                match.Spells = spells;
                // Iterates through items.
                for (int index = 1; index < 8; index++)
                {
                    string source = row[String.Format("item{0}ID", index)].ToString();
                    GetItemInfo(items, source);
                }
                match.Items = items;
                matches.Add(match);
            }
            return matches;
        }
        // Gets champion data from database. Pretty basic stuff.
        public static Champion GetChampionInfo(string championID)
        {
            DataTable championTable = DBHandler.GetChampionData(championID);
            Champion champion = new Champion();
            champion.ID = championID;
            foreach (DataRow championRow in championTable.Rows)
            {
                champion.Name = championRow["name"].ToString();
                champion.Icon = championRow["image"].ToString();
                champion.Title = championRow["title"].ToString();
                champion.LoadingImage = championRow["loadingImage"].ToString();
            }
            return champion;
        }
        // Gets summoner spell data from database. Basic stuff.
        public static void GetSpellInfo(List<Spell> spells, string source)
        {
            Spell spell = new Spell();
            spell.ID = source;
            DataTable table = DBHandler.GetSummonerSpellData(spell.ID);
            foreach (DataRow row in table.Rows)
            {
                spell.Name = row["name"].ToString();
                spell.Descr = row["descr"].ToString();
                spell.Icon = row["image"].ToString();
            }
            spells.Add(spell);
        }
        // Gets item data from database. Basic.
        public static void GetItemInfo(List<Item> items, string source)
        {
            Item item = new Item();
            item.ID = source;
            DataTable itemTable = DBHandler.GetItemData(item.ID);
            foreach (DataRow itemRow in itemTable.Rows)
            {
                item.Name = itemRow["name"].ToString();
                item.Descr = itemRow["descr"].ToString();
                item.Value = itemRow["value"].ToString();
                item.Icon = itemRow["image"].ToString();
            }
            items.Add(item);
        }
    }
}