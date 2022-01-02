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
            command.parameters = new CoupVars(); 
            ((CoupVars)command.parameters).coupOps = command.card.opsValue;
            ((CoupVars)command.parameters).enemyFaction = command.faction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;
            ((CoupVars)command.parameters).eligibleTargets = GetEligibleCountries(command).ToList();
            ((CoupVars)command.parameters).affectsDefcon = true;
            ((CoupVars)command.parameters).grantsMilOps = true; 

            // Check our current Turn & Action Round for a modifier to ops value or coupStrength 
            foreach (OpsBonus opsBonus in Game.currentTurn.GetComponents<OpsBonus>().Concat(Game.currentActionRound.GetComponents<OpsBonus>()))
                if(opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    ((CoupVars)command.parameters).coupOps += opsBonus.amount;
            foreach (CoupBonus opsBonus in Game.currentTurn.GetComponents<CoupBonus>().Concat(Game.currentActionRound.GetComponents<CoupBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    ((CoupVars)command.parameters).coupOps += opsBonus.amount;

            prepareEvent.Invoke(command); 
        }

        public void Target(GameCommand command)
        {
            ((CoupVars)command.parameters).coupDefense = ((CoupVars)command.parameters).targetCountry.stability * 2;
            ((CoupVars)command.parameters).roll = Random.Range(0, 6) + 1;

            // Check our country and card for coup or ops bonuses
            foreach (OpsBonus opsBonus in ((CoupVars)command.parameters).targetCountry.GetComponents<OpsBonus>().Concat(command.card.GetComponents<OpsBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    ((CoupVars)command.parameters).coupOps += opsBonus.amount;
            
            foreach (CoupBonus opsBonus in ((CoupVars)command.parameters).targetCountry.GetComponents<CoupBonus>().Concat(command.card.GetComponents<CoupBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    ((CoupVars)command.parameters).coupOps += opsBonus.amount;
           
            int influenceToRemove = Mathf.Min(((CoupVars)command.parameters).modifiedRoll(), ((CoupVars)command.parameters).targetCountry.influence[command.opponent]);
            int influenceToAdd = Mathf.Max(((CoupVars)command.parameters).modifiedRoll() - ((CoupVars)command.parameters).targetCountry.influence[command.opponent], 0); 

            ((CoupVars)command.parameters).influenceChange = new Dictionary<Game.Faction, int>();
            ((CoupVars)command.parameters).influenceChange.Add(command.faction, influenceToAdd);
            ((CoupVars)command.parameters).influenceChange.Add(command.opponent, -influenceToRemove);

            command.callback = Complete; 
            targetEvent.Invoke(command);  
        }

        public void Complete(GameCommand command)
        {
            foreach(Game.Faction faction in ((CoupVars)command.parameters).influenceChange.Keys)
                Game.AdjustInfluence(((CoupVars)command.parameters).targetCountry, faction, ((CoupVars)command.parameters).influenceChange[faction]);

            if (((CoupVars)command.parameters).grantsMilOps)
                MilOpsTrack.GiveMilOps(command.faction, ((CoupVars)command.parameters).coupOps);

            if (((CoupVars)command.parameters).affectsDefcon && ((CoupVars)command.parameters).targetCountry.isBattleground)
                DEFCONtrack.AdjustDefcon(command.faction, -1);

            command.callback = Finish;
            completeEvent.Invoke(command);
        }

        public class CoupVars : ICommandParameters
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

        public static IEnumerable<Country> GetEligibleCountries(GameCommand command) =>
            FindObjectsOfType<Country>()
                .Where(country => country.influence[command.opponent] > 0)
                .Where(country => !country.GetComponent<MayNotCoup>())
                .Where(country => DEFCONtrack.status > DEFCONtrack.defconRestrictions[country.continent])
                ;
    }
}
