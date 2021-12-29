using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class RedScarePurge : Card
    {
        // TODO: Fix the underlying system whereby we add a Modifier to the Ops Value of the card. 
        public override void CardEvent(GameCommand command)
        {
            command.FinishCommand();
        }
    }
}