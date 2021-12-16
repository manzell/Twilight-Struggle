using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UNIntervention : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        command.callback.Invoke(); 
    }
}
