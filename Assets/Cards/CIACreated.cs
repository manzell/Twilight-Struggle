using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIACreated : Card
{
    public override void Event(UnityEngine.Events.UnityAction callback)
    {

        callback.Invoke(); 
    }

}
