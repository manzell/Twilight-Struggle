using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class CIACreated : Card
    {
        public override void CardEvent(GameCommand command)
        {
            // The key here is that when we trigger the event, we may or may not invoke the callback. Maybe 
            // we need to update Turn Start so that when it presents the player the option to do a thing
            // it's separated, that way we can call it separately here
            // We do that already in the Receive Thread method, just make sure to move other stuff in there as well. 


            command.FinishCommand();
        }
    }
}