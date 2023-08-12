using System;
using System.Diagnostics;
using Game.Utils;

namespace Game
{
    // In real-world game need to read from external source
    // TODO For an interview-game using string constants, separate from logic, is enough
    public class Localization
    {
        public static string Localize(HandDecision handDecision)
        {
            switch (handDecision)
            {
                case HandDecision.Paper:
                    return "Opponent selects paper";
                case HandDecision.Scissors:
                    return "Opponent selects scissors";
                case HandDecision.Stone:
                    return "Opponent selects stone";
                default:
                    Debug.Assert(false, "value not localized: " + handDecision);
                    return handDecision.ToString();
            }
        }
    }
}