using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class ChinaCard : Card
{
    public bool faceUp;

    public override void Event(UnityEngine.Events.UnityAction? callback)
    {
        FindObjectOfType<Game>().playerMap[Game.phasingPlayer].hand.Remove(this);

        FindObjectOfType<Game>().playerMap[Game.phasingPlayer == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA].hand.Add(this);
        faceUp = false; 

        // The event on this call mov
        callback.Invoke();
    }
}
