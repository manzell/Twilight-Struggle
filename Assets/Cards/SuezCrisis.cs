using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuezCrisis : Card
{
    [SerializeField] List<Country> countries;

    public override void CardEvent(GameAction.Command command)
    {
        RemoveInfluence(countries, Game.Faction.USA, 4, 2); 
    }
}
