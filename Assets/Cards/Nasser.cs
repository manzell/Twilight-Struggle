using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class Nasser : Card
    {
        [SerializeField] Country egypt;

        public override void CardEvent(GameCommand command)
        {
            Game.adjustInfluenceEvent.Invoke(egypt, Game.Faction.USSR, 2);
            Game.adjustInfluenceEvent.Invoke(egypt, Game.Faction.USA, -(int)Mathf.Ceil(egypt.influence[Game.Faction.USA] / 2));
            Message("July 23rd Revolution in Egypy! Nasser ousts King Farouk!");
            command.FinishCommand();
        }
    }
}