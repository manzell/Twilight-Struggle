using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class ScoringCard : Card
    {
        public Country.Continent continent;
        public Dictionary<Scoring.ScoreState, int> scoreKey;
        public UnityEvent<Scoring> scoreEvent = new UnityEvent<Scoring>();

        public override void CardEvent(GameCommand command)
        {
            Scoring scoring = new Scoring(scoreKey, continent);

            if (scoring.vp != 0)
                Message($"{cardName} scored for {scoring.scoringFaction} {scoring.scoreState[scoring.scoringFaction]} (+{scoring.vp} VPs)");
            else
                Message($"{cardName} scored for Even");

            scoreEvent.Invoke(scoring);
            VictoryTrack.AdjustVPs(scoring.vp);

            command.FinishCommand();
        }
    }
}