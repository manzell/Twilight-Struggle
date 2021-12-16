using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class USJapanMutualDefensePact : Card
{
    [SerializeField] Country japan;

    public override void CardEvent(GameAction.Command command)
    {
        Game.SetInfluence.Invoke(japan, Game.Faction.USA, Mathf.Max(japan.influence[Game.Faction.USA], japan.stability + japan.influence[Game.Faction.USSR]));
        
        japan.gameObject.AddComponent<USJapanMutualDefensePact>();
        japan.gameObject.AddComponent<MayNotCoup>().faction = Game.Faction.USSR;
        japan.gameObject.AddComponent<MayNotRealign>().faction = Game.Faction.USSR;

        command.callback.Invoke();
    }
}
