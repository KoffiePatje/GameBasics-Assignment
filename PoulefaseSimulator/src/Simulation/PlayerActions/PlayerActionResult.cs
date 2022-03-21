namespace PouleSimulator
{
    public struct PlayerActionResult
    {
        public SoccerPlayer NewPlayerWithBallPosession { get; }

        public bool Success { get; }
        public bool HasScored { get; }

        public PlayerActionResult(SoccerPlayer playerWithBallPossesion, bool success, bool didScore = false) {
            this.NewPlayerWithBallPosession = playerWithBallPossesion;

            this.Success = success;
            this.HasScored = didScore;
        }
    }
}
