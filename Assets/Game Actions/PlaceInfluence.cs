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
            if (Game.currentPhase is TurnSystem.ActionRound)
                Game.currentActionRound.command = command;

            command.parameters = new InfluencePlacementVars();
            command.callback = Complete;

            // Check our current Turn & Action Round for a modifier to ops value or coupStrength 
            foreach (OpsBonus opsBonus in Game.currentTurn.GetComponents<OpsBonus>().Concat(Game.currentActionRound.GetComponents<OpsBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    ((InfluencePlacementVars)command.parameters).totalOps += opsBonus.amount;

            ((InfluencePlacementVars)command.parameters).countries = new List<Country>();
            ((InfluencePlacementVars)command.parameters).ops = Mathf.Max(((InfluencePlacementVars)command.parameters).totalOps + command.card.opsValue, 1);
            ((InfluencePlacementVars)command.parameters).eligibleCountries = EligibleCountries(command);

            prepareEvent.Invoke(command);
        }

        public void Target(GameCommand command)
        {
            // Called each time the user clicks to place 1 influence 
            Country targetCountry = ((InfluencePlacementVars)command.parameters).countries.Last();

            int placementCost = targetCountry.control == command.opponent ? 2 : 1; 

            if(((InfluencePlacementVars)command.parameters).ops >= placementCost)
            {
                ((InfluencePlacementVars)command.parameters).ops -= placementCost;
                Game.AdjustInfluence(targetCountry, command.faction, 1); 

                if(((InfluencePlacementVars)command.parameters).ops == 1)
                {
                    foreach(Country country in ((InfluencePlacementVars)command.parameters).eligibleCountries.ToArray())
                        if (country.control == command.opponent)
                            ((InfluencePlacementVars)command.parameters).eligibleCountries.Remove(country);
                }

                if (((InfluencePlacementVars)command.parameters).ops == 0)
                    targetEvent.Invoke(command);
            }
        }

        public void Complete(GameCommand command)
        {
            completeEvent.Invoke(command);
            command.callback = null;
            command.FinishCommand();
        }

        public class InfluencePlacementVars : ICommandParameters
        {
            public int totalOps, ops;
            public Dictionary<Country, int> influencePlacement = new Dictionary<Country, int>();
            public List<Country> countries = new List<Country>();
            public List<Country> eligibleCountries = new List<Country>(); 
        }

        public List<Country> EligibleCountries(GameCommand command)
        {
            InfluencePlacementVars placementVars = (InfluencePlacementVars)command.parameters;
            List<Country> countries = FindObjectsOfType<Country>().ToList();
            List<Country> eligibleCountries = new List<Country>();

            foreach (Country country in countries)
            {
                if (eligibleCountries.Contains(country)) continue;
                if (placementVars.ops < 1) continue;
                if (country.GetComponent<MayNotPlaceInfluence>()?.faction == command.faction) continue;
                if (country.control == command.opponent && placementVars.ops< 2) continue; 

                if (country.adjacentSuperpower == command.faction || country.influence[command.faction] > 0)
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