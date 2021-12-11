using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NATO : Card
{
    [SerializeField] Card warsawPact, marshallPlan;

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        if(Game.deck.removed.Contains(warsawPact) || Game.deck.removed.Contains(marshallPlan))
        {
            foreach (Country country in FindObjectsOfType<Country>())
                if (country.continent == Country.Continent.Europe)
                    country.gameObject.AddComponent<NATO>(); 
        }

        callback.Invoke(); 
    }
}
