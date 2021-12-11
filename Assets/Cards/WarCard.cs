using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCard : Card
{
    [SerializeField] Country targetCountry;
    [SerializeField] int victoryPoints = 2, rollRequired = 4; 

    public override void Event(UnityEngine.Events.UnityAction? callback)
    {
        Game.Faction enemyFaction = Game.Faction.Neutral;
        Game.Faction actingFaction = Game.Faction.Neutral;

        if (base.faction == Game.Faction.Neutral)
        {
            // faction = actingPlayer
        }

        switch (base.faction)
        {
            case Game.Faction.Neutral:
                //enemyFaction = actingPlayer == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA
                break;
            case Game.Faction.USA:
                enemyFaction = Game.Faction.USSR;
                break;
            case Game.Faction.USSR:
                enemyFaction = Game.Faction.USA;
                break;
        }

        int adjustment = 0;
        int roll = Random.Range(0, 6) + 1;

        foreach (Country neighbor in targetCountry.adjacentCountries)
        {
            if (neighbor.control == enemyFaction)
                adjustment--;
        }

        if (roll + adjustment >= rollRequired)
        {
            Game.AwardVictoryPoints.Invoke(actingFaction == Game.Faction.USA ? victoryPoints : -victoryPoints);
            targetCountry.influence[actingFaction] += targetCountry.influence[enemyFaction];
            targetCountry.influence[enemyFaction] = 0;
        }

        Game.AdjustMilOps.Invoke(Game.Faction.USSR, 2);
    }
}
