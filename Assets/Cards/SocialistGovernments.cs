using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialistGovernments : Card
{
    [SerializeField] List<Country> westernEurope = new List<Country>();
    int influenceToRemove = 3;
    int limit = 2; 

    public override void CardEvent(GameAction.Command command)
    {
        List<Country> eligibleCountries = new List<Country>();
        List<Country> removedFrom = new List<Country>();

        foreach(Country country in westernEurope)
            if(country.influence[Game.Faction.USA] > 0) 
                eligibleCountries.Add(country);

        uiManager.SetButton(uiManager.primaryButton, "Finish SocGov", Finish);

        RemoveInfluence(westernEurope, Game.Faction.USA, influenceToRemove, limit, Finish);

        void Finish()
        {
            uiManager.UnsetButton(uiManager.primaryButton); 
            command.callback.Invoke(); 
        }
    }
}
