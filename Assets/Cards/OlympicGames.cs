using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlympicGames : Card
{
    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        // Prompt Participate or Boycott
        Participate(); 
        callback.Invoke(); 
    }

    public void Participate()
    {
        Dictionary<Game.Faction, int> rolls = new Dictionary<Game.Faction, int> { { Game.Faction.USA, 0 }, { Game.Faction.USSR, 0 } };

        while(rolls[Game.Faction.USA] == rolls[Game.Faction.USSR])
        {
            rolls[Game.Faction.USA] = Random.Range(0, 6) + 1;
            rolls[Game.Faction.USSR] = Random.Range(0, 6) + 1;
            rolls[Game.phasingPlayer] += 2;
        }

        Game.AwardVictoryPoints.Invoke(rolls[Game.Faction.USA] > rolls[Game.Faction.USSR] ? 2 : -2);
    }

    public void Boycott()
    {
        Game.AdjustDEFCON.Invoke(Game.phasingPlayer, -1);
        //FindObjectOfType<PlaceInfluence>()?.Place(Game.phasingPlayer == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA, 4);
    }
}