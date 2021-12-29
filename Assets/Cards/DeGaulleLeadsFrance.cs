using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class DeGaulleLeadsFrance : Card
    {
        [SerializeField] Country france;
        public int USSRinfluence = 1;
        public int USAinfluence = -2;

        public override void CardEvent(GameCommand command)
        {
            FindObjectOfType<UI.UIMessage>().Message($"De Gaulle removes {Mathf.Min(Mathf.Abs(USAinfluence), france.influence[Game.Faction.USA])} US influence from France and adds {USSRinfluence} USSR Influence");

            Game.AdjustInfluence(france, Game.Faction.USA, USAinfluence);
            Game.AdjustInfluence(france, Game.Faction.USSR, USSRinfluence);

            france.gameObject.AddComponent<DeGaulleLeadsFrance>();

            Destroy(france.gameObject.GetComponent<NATO>());
            Destroy(france.gameObject.GetComponent<MayNotCoup>());
            Destroy(france.gameObject.GetComponent<MayNotRealign>());

            command.FinishCommand();
        }
    }
}