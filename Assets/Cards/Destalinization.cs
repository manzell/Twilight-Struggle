using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class Destalinization : Card
    {
        public override void CardEvent(GameCommand command)
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

            RemoveInfluence(eligibleCountries, Game.Faction.USSR, 4, 0, RedeployInfluence);
            Game.adjustInfluenceEvent.AddListener(TickCount);

            void TickCount(Country c, Game.Faction f, int n)
            {
                if (eligibleCountries.Contains(c) && f == Game.Faction.USSR && n < 0) 
                    count++;
            }

            void RedeployInfluence()
            {
                Message($"Place {count} USSR influence");
                UI.CountryClickHandler.Close();
                Game.adjustInfluenceEvent.RemoveListener(TickCount);

                foreach (Country c in FindObjectsOfType<Country>())
                    if (c.control != Game.Faction.USA)
                        eligibleCountries.Add(c);

                AddInfluence(eligibleCountries, Game.Faction.USSR, count, 0, command.FinishCommand);
            }
        }
    }
}