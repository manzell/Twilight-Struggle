using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coup : Action
{
    CoupAttempt coupAttempt;

    public override void Play(UnityEngine.Events.UnityAction callback)
    {
        this.callback = callback;
        List<Country> eligibleCountries = new List<Country>();

        foreach (Country country in FindObjectsOfType<Country>())
            if (country.influence[opponent] > 0 && DEFCON.status > DEFCON.defconRestrictions[country.continent])
                eligibleCountries.Add(country);

        if (eligibleCountries.Count == 0) return;

        countryClickHandler = new CountryClickHandler(eligibleCountries, SetTarget, Color.red);

        coupAttempt = new CoupAttempt();
        coupAttempt.coupFaction = Game.phasingPlayer;
        coupAttempt.coupOps = card.opsValue;

        Game.currentActionRound.gameAction = coupAttempt;
    }

    void SetTarget(Country country, PointerEventData ped) 
    {
        coupAttempt.targetCountry = country;
        countryClickHandler.Close();

        coupAttempt.influenceRemoved = Mathf.Min(Mathf.Max(coupAttempt.roll + coupAttempt.coupOps + coupAttempt.coupStrength - coupAttempt.targetCountry.stability * 2, 0), coupAttempt.targetCountry.influence[opponent]);
        coupAttempt.influenceAdded = Mathf.Max(coupAttempt.roll + coupAttempt.coupOps + coupAttempt.coupStrength - coupAttempt.targetCountry.stability * 2 - coupAttempt.targetCountry.influence[opponent], 0);

        FindObjectOfType<MilOpsTrack>().GiveMilOps(coupAttempt.coupFaction, coupAttempt.coupOps); 

        Debug.Log($"Roll {coupAttempt.roll} + {coupAttempt.coupOps} Ops - {coupAttempt.targetCountry.stability * 2} Coup Defense = " + 
            $"{coupAttempt.roll + coupAttempt.coupStrength + coupAttempt.coupOps - coupAttempt.targetCountry.stability * 2} [Removing: {coupAttempt.influenceRemoved} Adding: {coupAttempt.influenceAdded}]");

        Game.AdjustInfluence.Invoke(country, coupAttempt.opponent, -coupAttempt.influenceRemoved);
        Game.AdjustInfluence.Invoke(country, coupAttempt.coupFaction, coupAttempt.influenceAdded);

        if (country.isBattleground)
            DEFCON.AdjustDEFCON.Invoke(-1);

        Game.currentActionRound.card = card; 
        Game.currentActionRound.gameAction = coupAttempt;

        FinishAction(); 
    }

    public class CoupAttempt : IGameAction
    {
        public Country targetCountry;
        public Game.Faction coupFaction;
        public Game.Faction opponent { get { return coupFaction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA; } }
        public bool affectDefcon = true;
        public int roll = Random.Range(0, 6) + 1;
        public int
            coupStrength = 0,
            coupOps = 0, 
            influenceRemoved, 
            influenceAdded;
    }

}
