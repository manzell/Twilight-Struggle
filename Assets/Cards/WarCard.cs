using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCard : Card
{
    [SerializeField] List<Country> targetCountries;
    [SerializeField] int victoryPoints = 2, rollRequired = 4, milOps = 2;

    public override void CardEvent(GameAction.Command command)
    {
        if (targetCountries.Count > 1)
            CountryClickHandler.Setup(targetCountries, War, Color.red);
        else if (targetCountries.Count == 1)
            War(targetCountries[0]);
        else
            command.callback.Invoke();

        void War(Country targetCountry)
        {
            CountryClickHandler.Close(); // Hack! Maybe set CCH to a close-after-n-clicks mode? 

            int adjustment = 0;
            int roll = Random.Range(0, 6) + 1;

            foreach (Country neighbor in targetCountries[0].adjacentCountries)
                if (neighbor.control == command.enemyPlayer)
                    adjustment--;

            Game.Faction warFaction = command.card.faction == Game.Faction.Neutral ? command.phasingPlayer : command.card.faction; 
            Game.Faction warEnemyFaction = warFaction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;

            FindObjectOfType<UIMessage>().Message($"{warFaction} launched a war in {targetCountry.countryName}! They rolled a {roll}+{adjustment} (needed: {rollRequired}). {(roll + adjustment >= rollRequired ? "Success!" : "Failure.")}");

            if (roll + adjustment >= rollRequired)
            {
                Game.AdjustVPs.Invoke(warFaction == Game.Faction.USA ? victoryPoints : -victoryPoints);
                Game.AdjustInfluence.Invoke(targetCountry, warFaction, targetCountry.influence[warEnemyFaction]);
                Game.SetInfluence.Invoke(targetCountry, warEnemyFaction, 0);
            }

            Game.AdjustMilOps.Invoke(warFaction, milOps);

            command.callback.Invoke();
        }
    }
}
