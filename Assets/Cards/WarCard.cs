using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class WarCard : Card
    {
        [SerializeField] List<Country> targetCountries;
        [SerializeField] int victoryPoints = 2, rollRequired = 4, milOps = 2;

        public override void CardEvent(GameCommand command) // TODO: There are way too many UI calls here! 
        {
            if (targetCountries.Count > 1)
                UI.CountryClickHandler.Setup(targetCountries, War, Color.red);
            else if (targetCountries.Count == 1)
                War(targetCountries[0]);
            else
                command.FinishCommand();

            void War(Country targetCountry)
            {
                UI.CountryClickHandler.Close(); // Hack! Maybe set CCH to a close-after-n-clicks mode? 

                int adjustment = 0;
                int roll = Random.Range(0, 6) + 1;
                Game.Faction warFaction = command.card.faction == Game.Faction.Neutral ? command.faction : command.card.faction;
                Game.Faction warEnemyFaction = warFaction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;

                foreach (Country neighbor in targetCountries[0].adjacentCountries)
                    if (neighbor.control == warEnemyFaction)
                        adjustment--;

                FindObjectOfType<UI.UIMessage>().Message($"{warFaction} launched a war in {targetCountry.countryName}! They rolled a {roll}+{adjustment} (needed: {rollRequired}). {(roll + adjustment >= rollRequired ? "Success!" : "Failure.")}");

                if (roll + adjustment >= rollRequired)
                {
                    Game.AdjustVPsEvent.Invoke(warFaction == Game.Faction.USA ? victoryPoints : -victoryPoints);
                    Game.adjustInfluenceEvent.Invoke(targetCountry, warFaction, targetCountry.influence[warEnemyFaction]);
                    Game.setInfluenceEvent.Invoke(targetCountry, warEnemyFaction, 0);
                }

                Game.AdjustMilOps.Invoke(warFaction, milOps);

                command.FinishCommand();
            }
        }
    }
}