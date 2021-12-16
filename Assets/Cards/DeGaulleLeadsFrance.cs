using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class DeGaulleLeadsFrance : Card
{
    [SerializeField] Country france;

    public override void CardEvent(GameAction.Command command)
    {
        Game.AdjustInfluence.Invoke(france, Game.Faction.USA, -2);
        Game.AdjustInfluence.Invoke(france, Game.Faction.USSR, 1);
        
        france.gameObject.AddComponent<DeGaulleLeadsFrance>();

        Destroy(france.gameObject.GetComponent<NATO>());
        Destroy(france.gameObject.GetComponent<MayNotCoup>());
        Destroy(france.gameObject.GetComponent<MayNotRealign>());

        command.callback.Invoke();
    }
}
