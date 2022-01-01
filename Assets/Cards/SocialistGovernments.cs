using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class SocialistGovernments : Card
    {
        [SerializeField] List<Country> westernEurope = new List<Country>();
        int influenceToRemove = 3;
        int limit = 2;

        public override void CardEvent(GameCommand command)
        {
            List<Country> eligibleCountries = new List<Country>();
            List<Country> removedFrom = new List<Country>();

            foreach (Country country in westernEurope)
                if (country.influence[Game.Faction.USA] > 0)
                    eligibleCountries.Add(country);

            RemoveInfluence(eligibleCountries, Game.Faction.USA, influenceToRemove, limit, command.FinishCommand);
        }
    }
}