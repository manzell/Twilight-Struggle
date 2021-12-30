using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    public class PlaceInfluence : GameAction, IActionPrepare, IActionTarget, IActionComplete
    {
        public void Prepare(GameCommand command) 
        {
            PlacementVars placementVars = new PlacementVars();
            command.callback = Target; 
            command.parameters = placementVars;

            // Check our current Turn & Action Round for a modifier to ops value or coupStrength 
            foreach (OpsBonus opsBonus in Game.currentTurn.GetComponents<OpsBonus>().Concat(Game.currentActionRound.GetComponents<OpsBonus>()))
                if (opsBonus.faction == Game.actingPlayer || opsBonus.faction == Game.Faction.Neutral)
                    placementVars.totalOps += opsBonus.amount;

            placementVars.countries = new List<Country>(); 
            placementVars.ops = placementVars.totalOps;
            placementVars.eligibleCountries = EligibleCountries(command); 

            prepareEvent.Invoke(command);
        }

        public void Target(GameCommand command) 
        {
            PlacementVars placementVars = (PlacementVars)command.parameters;

            Country targetCountry = placementVars.countries.Last();
            int opsCost = targetCountry.control == command.opponent ? 2 : 1;

            // Check our current Country for a bonus-ops modifier
            foreach(OpsBonus opsBonus in targetCountry.GetComponents<OpsBonus>())
            {
                // TODO: Do this. 
            }

            if (placementVars.ops >= opsCost)
            {
                placementVars.ops -= opsCost;

                if (placementVars.influencePlacement.ContainsKey(targetCountry))
                    placementVars.influencePlacement[targetCountry] += 1;
                else
                    placementVars.influencePlacement.Add(targetCountry, 1);

                Game.AdjustInfluence(targetCountry, command.faction, 1); 
            }

            command.parameters = placementVars;
            command.callback = Complete;
            targetEvent.Invoke(command);
        }

        public void Complete(GameCommand command) 
        {
            completeEvent.Invoke(command);
            command.callback = null;
            command.FinishCommand(); 
        }

        public struct PlacementVars : ICommandVariables
        {
            public int totalOps, ops;
            public Dictionary<Country, int> influencePlacement;
            public List<Country> countries;
            public List<Country> eligibleCountries; 
        }

        public List<Country> EligibleCountries(GameCommand command)
        {
            PlacementVars placementVars = (PlacementVars)command.parameters;
            List<Country> countries = FindObjectsOfType<Country>().ToList();
            List<Country> eligibleCountries = new List<Country>();

            foreach (Country country in countries)
            {
                if (eligibleCountries.Contains(country) || placementVars.ops < 1 ||
                (country.GetComponent<MayNotPlaceInfluence>()?.faction == command.faction) || 
                (country.control == command.opponent && placementVars.ops < 2))
                    continue;

                if (country.adjacentSuperpower == command.faction)
                    eligibleCountries.Add(country);
                else
                    foreach (Country c in country.adjacentCountries)
                        if (c.influence[command.faction] > 0)
                        {
                            eligibleCountries.Add(country);
                            break;
                        }
            }
            return eligibleCountries; 
        }
    }
}