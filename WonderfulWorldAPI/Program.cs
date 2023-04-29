// See https://aka.ms/new-console-template for more information


using Newtonsoft.Json;
using WonderfulWorldAPI;

class MainClasss
{
    private static Dictionary<string, string> TeamMap = new Dictionary<string, string>();
    static HttpClient httpClient = new HttpClient();
    static DateTime ConvertStringToDateTime(string timeStamp)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));//当地时区  
        var time = startTime.AddMilliseconds(double.Parse(timeStamp));
        return time;
    }

    static List<Match> getMatches(string eventId)
    {
        string url = "https://gwapi.pwesports.cn/eventcenter/app/csgo/event/getMatchList?eventId=" + eventId;
        string res = httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        var obj = JsonConvert.DeserializeObject<dynamic>(res);
        List<Match> matches = new List<Match>();
        foreach (var var1 in obj["result"]["matchResponse"]["dtoList"])
        {
            var team1Name = var1.team1DTO.name.ToString();
            var team1Id = var1.team1DTO.id.ToString();
            var team2Name = var1.team2DTO.name.ToString();
            var team2Id = var1.team2DTO.id.ToString();
            TeamMap.Add(team1Id,team1Name);
            TeamMap.Add(team2Id,team2Name);
            Match match = new Match();
            match.Winner = TeamMap[var1.winnerTeamId.ToString()];
            var teams = new string[2];
            teams[0] = TeamMap[team1Id];
            teams[1] = TeamMap[team2Id];
            match.Teams = teams;
            matches.Add(match);
        }

        return matches;
    }
    static void Main()
    {
        var smsclient = new SMSClient();
        smsclient.Session = "78186985baba4c6586bf48ac9aa54acf";

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
                Event.StartTime = ConvertStringToDateTime(data.startTime.ToString()).ToString();
                Event.EndTime = ConvertStringToDateTime(data.endTime.ToString()).ToString();
                var Teams = new List<Team>();
                foreach (var var1 in data["teamDTOList"])
                {
                    Team team = new Team();

                    team.name = var1.name;
                    team.rank = var1.rank;
                    Teams.Add(team);
                }

                Event.Matches = getMatches(data.eventId.ToString());
                Event.Teams = Teams;
                events.Add(Event);
                i++;
            }
        }
        Console.WriteLine(i);

    }
}
