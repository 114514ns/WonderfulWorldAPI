namespace WonderfulWorldAPI;

public struct Match
{
    public string[] Teams { get; set; }
    public STATE State { get; set; }
    private Dictionary<string,PlayerScore> Scores { get; set; }
    public string Winner { get; set; }


}