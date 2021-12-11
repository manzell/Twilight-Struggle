using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceInfluence : Action
{
    InfluencePlacement placer;

    public override void Play(UnityEngine.Events.UnityAction callback)
    {
        this.callback = callback;
        Debug.Log($"{Game.phasingPlayer} playing {card.cardName} for {card.opsValue} Ops");

        currentAction = this;
        Game.currentActionRound.action = this;
        Game.currentActionRound.card = card;

        InfluencePlacement placer = new InfluencePlacement(Game.phasingPlayer, card.opsValue);

        placer.eligibleCountries = placer.AdjacentCountries;
        countryClickHandler = new CountryClickHandler(placer.eligibleCountries, onCountryClick, Color.yellow);

        Game.currentActionRound.gameAction = placer;
    }

    // We pass this is so that we can pass in other implementations 
    public void onCountryClick(Country country, PointerEventData ped)
    {
        if (!placer.eligibleCountries.Contains(country)) return;

        if (placer.influencePlacement.ContainsKey(country))
            placer.influencePlacement[country]++;
        else
            placer.influencePlacement.Add(country, 1);

        placer.opsRemaining--;
        if (country.control == placer.opponent) placer.opsRemaining--;

        Debug.Log($"Place 1 Influence in {country.countryName}; {placer.opsRemaining} remaining");
        Game.AdjustInfluence.Invoke(country, placer.actingPlayer, 1);

        if (placer.opsRemaining == 1)
        {
            for (int i = placer.eligibleCountries.Count - 1; i >= 0; i--)
            {
                if (placer.eligibleCountries[i].control == placer.opponent)
                {
                    countryClickHandler.RemoveHighlight(placer.eligibleCountries[i]);
                    placer.eligibleCountries.Remove(placer.eligibleCountries[i]);
                }
            }
        }

        if (placer.opsRemaining == 0)
            FinishAction();
    }

    public class InfluencePlacement: IGameAction
    {
        public List<Country> eligibleCountries;

        public int ops, opsRemaining;
        public Game.Faction actingPlayer;
        public Game.Faction opponent { get { return actingPlayer == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA; } }
        public Dictionary<Country, int> influencePlacement = new Dictionary<Country, int>();

        public InfluencePlacement(Game.Faction faction, int ops)
        {
            actingPlayer = faction;
            this.ops = ops;
            this.opsRemaining = ops;
        }

        public List<Country> AdjacentCountries // This maybe should be on the Game Object? 
        {
            get
            {
                List<Country> tmp = new List<Country>();
                Country[] countries = FindObjectsOfType<Country>();

                foreach (Country country in countries)
                {
                    if (country.influence[actingPlayer] > 0)
                    {
                        if (!tmp.Contains(country))
                            tmp.Add(country);

                        foreach (Country neighbor in country.adjacentCountries)
                            if (!tmp.Contains(neighbor))
                                tmp.Add(neighbor);
                    }
                }

                return tmp;
            }
        }
    }
}
