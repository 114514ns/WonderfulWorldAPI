namespace WonderfulWorldAPI;

public struct Event
{
    public string EventName { get; set; }
    public int EventId { get; set; }
    public List<Match> Matches { get; set; }
    public List<Team> Teams { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    
}