using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Containment : Card
{
    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        // Create an effect that impacts checking a Card's Ops Value; then Remove it 
        // Player add listener 
        // Game.TurnEnd.AddListener

        callback.Invoke(); 
    }
}
