using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class ChinaCard : Card
{
    public bool faceUp = true; 
    public Game.Faction currentFaction = Game.Faction.USSR;

    public override void CardEvent(GameAction.Command command)
    {
        faceUp = false;
        currentFaction = currentFaction == Game.Faction.USSR ? Game.Faction.USA : Game.Faction.USSR;

        command.callback.Invoke(); 
    }
}