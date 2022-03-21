namespace PouleSimulator
{
    public interface IPlayerAction
    {
        double ActionDurationInSeconds { get; }

        bool CanPerformAction(SoccerPlayer player);
        double GetActionScore(ref MatchState matchState);

        PlayerActionResult Execute(ref MatchState matchState);
    }
}
