using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormosanRevolution : Card
{
    [SerializeField] Country taiwan;
    [SerializeField] Card chinaCard; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        taiwan.gameObject.AddComponent<FormosanRevolution>(); 
    }
}
