using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class Turn : Phase
{
    Dictionary<Game.Faction, Card> headlines = new Dictionary<Game.Faction, Card>();

    public Headline headline; 
    public List<ActionRound> actionRounds = new List<ActionRound>();

    private void Awake()
    {
        headline = GetComponentInChildren<Headline>();
        actionRounds.AddRange(GetComponentsInChildren<ActionRound>()); 
    }

    public override void StartPhase(UnityAction callback)
    {
        Game.currentTurn = this;
        base.StartPhase(callback);
    }

    public override void EndPhase(UnityAction callback)
    {
        Game.currentTurn = null; 
        base.EndPhase(callback);
    }
}
