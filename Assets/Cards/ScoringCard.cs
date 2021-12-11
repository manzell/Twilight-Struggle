using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class ScoringCard : Card
{
    public Dictionary<Scoring.ScoreState, int> scoreKey; 
    public List<Country> battlegrounds, nonBattlegrounds;
    public UnityEvent<Scoring> Score = new UnityEvent<Scoring>();

    public override void Event(UnityEngine.Events.UnityAction? callback)
    {
        Scoring scoring = new Scoring(scoreKey, battlegrounds, nonBattlegrounds);

        Score.Invoke(scoring); 
        Game.Score.Invoke(scoring);
        Game.AwardVictoryPoints.Invoke(scoring.vp);
    }
}
