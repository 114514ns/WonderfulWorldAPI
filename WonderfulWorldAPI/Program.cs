// See https://aka.ms/new-console-template for more information


using System.Text.Json.Nodes;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WonderfulWorldAPI;

class MainClasss
{
    private static Dictionary<string, string> TeamMap = new Dictionary<string, string>();
    static HttpClient httpClient = new HttpClient();
    static DateTime ConvertStringToDateTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    static List<Match> getMatches(string eventId)
    {
        string url = "https://gwapi.pwesports.cn/eventcenter/app/csgo/event/getMatchList?eventId=" + eventId;
        string res = httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        var obj = JsonConvert.DeserializeObject<dynamic>(res);
        foreach (var var1 in obj["result"]["matchResponse"]["dtoList"])
        {
            var team1Name = var1.team1DTO.name;
            var team1Id = var1.team1DTO.id;
            var team2Name = var1.team2DTO.name;
            var team2Id = var1.team2DTO.id;
            TeamMap.Add(team1Id,team1Name);
            TeamMap.Add(team2Id,team2Name);
            Match match = new Match();
            match.Winner = TeamMap[var1.winnerTeamId];
            var teams = new string[2];
            teams[0] = TeamMap[team1Id];
            teams[0] = TeamMap[team1Id];
        }
    }
    static void Main()
    {
        
        var s = httpClient
            .GetAsync("https://appactivity.wmpvp.com/steamcn/app/csgo/event/getEventList?pageNum=1&pageSize=10&type=1").Result
            .Content.ReadAsStringAsync().Result;
        var deserializeObject = JsonConvert.DeserializeObject<dynamic>(s);
        int i = 0;
        var events = new List<Event>();
        foreach (var data in deserializeObject["result"]["eventResponse"]["dtoList"])
        {
            var eventName = (string) data.nameZh;
    
            if (eventName.Contains("BLAST") || eventName.Contains("IEM") || eventName.Contains("ESL"))
            {
                Console.WriteLine(data);
                var Event = new Event();
                Event.EventName = eventName;
                Event.EventId = data.eventId;
                Event.StartTime = ConvertStringToDateTime(data.startTime).ToString();
                Event.EndTime = ConvertStringToDateTime(data.endTime).ToString();
                var Teams = new List<Team>();
                foreach (var var1 in data["teamDTOList"])
                {
                    Team team = new Team();

                    team.name = var1.name;
                    team.rank = var1.rank;
                    Teams.Add(team);
                }

                Event.Teams = Teams;
                events.Add(Event);
                i++;
            }
        }
        Console.WriteLine(i);
    }
}
