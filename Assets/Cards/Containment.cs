using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class Containment : Card
    {
        public override void CardEvent(GameCommand command)
        {
            command.FinishCommand();
        }
    }
}