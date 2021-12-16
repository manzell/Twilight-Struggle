using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class NATO : Card
{
    public bool isPlayable = false;

    public override void CardEvent(GameAction.Command command)
    {
        if(isPlayable)
            foreach (Country country in FindObjectsOfType<Country>())
                if (country.continent == Country.Continent.Europe && !country.GetComponent<DeGaulleLeadsFrance>())
                {
                    country.gameObject.AddComponent<NATO>();
                    country.gameObject.AddComponent<MayNotCoup>().faction = Game.Faction.USSR;
                    country.gameObject.AddComponent<MayNotRealign>().faction = Game.Faction.USSR;
                }


        command.callback.Invoke(); 
    }
}
