using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fidel : Card
{
    [SerializeField] Country cuba;

    public override void CardEvent(GameAction.Command command)
    {
        Game.SetInfluence.Invoke(cuba, Game.Faction.USA, 0);
        Game.SetInfluence.Invoke(cuba, Game.Faction.USSR, Mathf.Max(cuba.influence[Game.Faction.USSR], cuba.stability));

        Message("Fidel overthrows Batista!"); 
        command.callback.Invoke(); 
    }
}
