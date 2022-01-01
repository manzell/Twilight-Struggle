using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class COMECON : Card
    {
        public int influenceAmt = 5; 
        [SerializeField] List<Country> easternEurope = new List<Country>();

        public override void CardEvent(GameCommand command)
        {
            List<Country> eligibleCountries = new List<Country>();

            foreach (Country country in easternEurope)
                if (country.control != Game.Faction.USA)
                    eligibleCountries.Add(country);

            if (eligibleCountries.Count <= influenceAmt)
                AddInfluence(Game.Faction.USSR, eligibleCountries, 1); 
            else
                AddInfluence(eligibleCountries, Game.Faction.USSR, influenceAmt, 1, command.FinishCommand); 
        }
    }
}