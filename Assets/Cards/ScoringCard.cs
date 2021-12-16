using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class ScoringCard : Card
{
    public Country.Continent continent;
    public Dictionary<Scoring.ScoreState, int> scoreKey; 
    public UnityEvent<Scoring> scoreEvent = new UnityEvent<Scoring>();

    public override void CardEvent(GameAction.Command command)
    {
        Scoring scoring = new Scoring(scoreKey, continent);
        scoreEvent.Invoke(scoring); 
        Game.AdjustVPs.Invoke(scoring.vp);
    }
}
