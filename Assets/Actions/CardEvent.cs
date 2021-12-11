using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEvent : Action
{
    public override void Play(UnityEngine.Events.UnityAction callback)
    {
        card.Event(callback);
    }
}
