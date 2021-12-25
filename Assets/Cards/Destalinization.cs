using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destalinization : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        int count = 0;
        List<Country> eligibleCountries = new List<Country>();
        List<Country> removedFrom = new List<Country>();
        List<Country> addedTo = new List<Country>();

        int eligibleInfluence = 0; 

        foreach (Country country in FindObjectsOfType<Country>())
        {
            if (country.influence[Game.Faction.USSR] > 0)
            {
                eligibleCountries.Add(country);
                eligibleInfluence += country.influence[Game.Faction.USSR];
            }
        }

        uiManager.SetButton(uiManager.cancelButton, "Finish Destal", Finish);
        uiManager.SetButton(uiManager.confirmButton, "Finish Removing Influence", RedeployInfluence);

        RemoveInfluence(eligibleCountries, Game.Faction.USSR, 4, 0, RedeployInfluence);
        adjustInfluenceEvent.AddListener(c => count++); 

        void RedeployInfluence()
        {
            Message($"Place {count} USSR influence");
            CountryClickHandler.Close();

            foreach (Country c in FindObjectsOfType<Country>())
                if (c.control != Game.Faction.USA)
                    eligibleCountries.Add(c);

            AddInfluence(eligibleCountries, Game.Faction.USSR, count, 0, Finish);
        }

        void Finish()
        {
            CountryClickHandler.Close(); 
            
            uiManager.UnsetButton(uiManager.cancelButton);
            uiManager.UnsetButton(uiManager.confirmButton);
            
            command.callback.Invoke();
        }
    }
}
