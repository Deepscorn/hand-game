using System;
using Game.Utils;

namespace Game
{
    public class Localization
    {
        public static string Localize(HandDecision handDecision)
        {
            switch (handDecision)
            {
                case HandDecision.Paper:
                    return "paper";
                case HandDecision.Scissors:
                    return "scissors";
                case HandDecision.Stone:
                    return "stone";
                default:
                    AssertExt.Fail("value not localized: " + handDecision);
                    return handDecision.ToString();
            }
        }
    }
}