using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


namespace TwilightStruggle
{
    public class Coup : GameAction, IActionPrepare, IActionTarget, IActionComplete
    {
        public void Prepare(GameCommand command)
        {
            CoupVars coupVars = new CoupVars();
            coupVars.coupOps = command.card.opsValue;
            coupVars.enemyFaction = command.faction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA; 
            coupVars.eligibleTargets = GetEligibleCountries(command);
            coupVars.affectsDefcon = false;
            coupVars.grantsMilOps = true; 
            
            // Check our current Turn & Action Round for a modifier to ops value or coupStrength 
            foreach(OpsBonus opsBonus in Game.currentTurn.transform.GetComponents<OpsBonus>().Concat(Game.currentActionRound.transform.GetComponents<OpsBonus>()))
                if(opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    coupVars.coupOps += opsBonus.amount;
            foreach (CoupBonus opsBonus in Game.currentTurn.transform.GetComponents<CoupBonus>().Concat(Game.currentActionRound.transform.GetComponents<CoupBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    coupVars.coupOps += opsBonus.amount;

            command.parameters = coupVars;
            prepareEvent.Invoke(command); 
        }

        public void Target(GameCommand command)
        {
            CoupVars coupVars = (CoupVars)command.parameters;
            coupVars.coupDefense = coupVars.targetCountry.stability * 2;
            coupVars.roll = Random.Range(0, 6) + 1;

            // Check our country and card for coup or ops bonuses
            foreach (OpsBonus opsBonus in coupVars.targetCountry.transform.GetComponents<OpsBonus>().Concat(command.card.transform.GetComponents<OpsBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    coupVars.coupOps += opsBonus.amount;
            foreach (CoupBonus opsBonus in coupVars.targetCountry.transform.GetComponents<CoupBonus>().Concat(command.card.transform.GetComponents<CoupBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    coupVars.coupOps += opsBonus.amount;

            coupVars.influenceChange = new Dictionary<Game.Faction, int>(); 
            coupVars.influenceChange.Add(command.faction, Mathf.Max(0, coupVars.modifiedRoll() - coupVars.targetCountry.influence[coupVars.enemyFaction]));
            coupVars.influenceChange.Add(coupVars.enemyFaction, -Mathf.Min(Mathf.Max(coupVars.modifiedRoll(), coupVars.targetCountry.influence[coupVars.enemyFaction])));

            command.parameters = coupVars;
            command.callback = Complete; 
            targetEvent.Invoke(command);  
        }

        public void Complete(GameCommand command)
        {
            CoupVars coupVars = (CoupVars)command.parameters;

            foreach(Game.Faction faction in coupVars.influenceChange.Keys)
                Game.AdjustInfluence(coupVars.targetCountry, faction, coupVars.influenceChange[faction]);

            if (coupVars.grantsMilOps)
                MilOpsTrack.GiveMilOps(command.faction, coupVars.coupOps);

            if (coupVars.affectsDefcon && coupVars.targetCountry.isBattleground)
                DEFCONtrack.AdjustDefcon(command.faction, -1);
 
            completeEvent.Invoke(command);
            command.callback = null; 
            command.FinishCommand();
        }

        public class CoupVars : ICommandVariables
        {
            public Country targetCountry;
            public List<Country> eligibleTargets;
            public Game.Faction enemyFaction; 
            public int coupOps, coupDefense;
            public int roll;
            public bool grantsMilOps;
            public bool affectsDefcon;
            public Dictionary<Game.Faction, int> influenceChange;

            public int modifiedRoll() => Mathf.Max(roll + coupOps - coupDefense, 0); 
        }

        public static List<Country> GetEligibleCountries(GameCommand command)
        {
            CoupVars coupVars = (CoupVars)command.parameters;

            List<Country> eligibleCountries = FindObjectsOfType<Country>().ToList();
            foreach (Country country in eligibleCountries.ToArray())
            {
                // Filter out any countries that are prohibited due to DEFCON or due to lack of opponent influence or cannot be couped for other reasons
                if (DEFCONtrack.status <= DEFCONtrack.defconRestrictions[country.continent] || 
                    country.influence[command.opponent] == 0 || 
                    country.GetComponent<MayNotCoup>())

                    eligibleCountries.Remove(country);
            }

            return eligibleCountries;
        }
    }
}
