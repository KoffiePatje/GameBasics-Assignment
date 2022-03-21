using System;

namespace PouleSimulator
{
    /// <summary>
    /// This enum provides all available positions in a formation, the application assumes these are ordered 
    /// starting from the most deffensive position up to the most offensive position.
    /// </summary>
    public enum ESoccerPlayerPosition
    {
        Goalkeeper = 0,
        Defender,
        Midfielder,
        Attacker
    }
}
