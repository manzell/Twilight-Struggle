using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class MarshallPlan : Card
    {
        public int numCountries = 7; 
        [SerializeField] List<Country> westernEurope = new List<Country>();
        [SerializeField] NATO NATO;

        public override void CardEvent(GameCommand command)
        {
            NATO.isPlayable = true;

            foreach (Country country in westernEurope)
                if (country.control == Game.Faction.USSR)
                    westernEurope.Remove(country);

            // TODO: Set the Messenger Buttons

            if (westernEurope.Count <= numCountries)
            {
                AddInfluence(faction, westernEurope, 1);
                command.FinishCommand(); 
            }
            else
                AddInfluence(westernEurope, Game.Faction.USA, numCountries, 1, command.FinishCommand); 
        }
    }
}