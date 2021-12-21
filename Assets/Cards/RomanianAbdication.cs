using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomanianAbdication : Card
{
    [SerializeField] Country romania;

    public override void CardEvent(GameAction.Command command)
    {
        Message("King Michael I of Romania abdicates!"); 

        Game.SetInfluence.Invoke(romania, Game.Faction.USA, 0);
        Game.SetInfluence.Invoke(romania, Game.Faction.USSR, Mathf.Max(romania.stability, romania.influence[Game.Faction.USSR]));

        command.callback.Invoke(); 
    }
}
