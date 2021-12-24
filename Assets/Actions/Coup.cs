using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq; 

public class Coup : GameAction
{
    public override Command GetCommand(Card card, GameAction action) => new CoupCommand(card, action);

    public override void ExecuteCommandAction(Command command)
    {
        CoupCommand coup = command as CoupCommand;
        List<Country> eligibleCountries = FindObjectsOfType<Country>().ToList();

        foreach (Country country in eligibleCountries)
        {
            // Filter out any countries that are prohibited due to DEFCON or due to lack of opponent influence or cannot be couped for other reasons
            if (DEFCON.Status <= DEFCON.defconRestrictions[country.continent] || country.influence[command.enemyPlayer] == 0 || country.GetComponent<MayNotCoup>())
                eligibleCountries.Remove(country);
        }

        CountryClickHandler.Setup(eligibleCountries, SetCoupTarget);

        void SetCoupTarget(Country country)
        {
            coup.SetTargetCountry(country);

            Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, coup.influenceAdjusted[Game.Faction.USSR]);
            Game.AdjustInfluence.Invoke(country, Game.Faction.USA, coup.influenceAdjusted[Game.Faction.USA]);

            if(coup.targetCountry.isBattleground)
                Game.AdjustDEFCON.Invoke(Game.phasingPlayer, -1); 

            CountryClickHandler.Close();
            command.callback.Invoke();
        }
    }

    public class CoupCommand : Command
    {
        public Country targetCountry;
        public int roll;
        public int modifiedRoll;
        public Dictionary<Game.Faction, int> influenceAdjusted = new Dictionary<Game.Faction, int>();
        public GameEvent<Command> setTargetEvent = new GameEvent<Command>();

        public CoupCommand(Card c, GameAction a) : base(c, a) =>
            roll = Random.Range(0, 6) + 1;

        public void SetTargetCountry(Country country)
        {
            targetCountry = country;
            roll = Random.Range(0, 6) + 1;
            modifiedRoll = roll + cardOpsValue;

            influenceAdjusted.Add(phasingPlayer, Mathf.Max(0, modifiedRoll - country.stability * 2 - country.influence[enemyPlayer]));
            influenceAdjusted.Add(enemyPlayer, -Mathf.Min(Mathf.Max(modifiedRoll - country.stability * 2, 0), country.influence[enemyPlayer]));

            Debug.Log($"{phasingPlayer} coups {targetCountry.countryName} with {cardOpsValue} Ops!");
            Debug.Log($"Coup Roll: {roll}. Removing {influenceAdjusted[enemyPlayer]} {enemyPlayer} influence. Adding {influenceAdjusted[phasingPlayer]} {phasingPlayer} influence in {targetCountry.countryName}");
        }
    }
}
