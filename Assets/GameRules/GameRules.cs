using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; 
using Sirenix.OdinInspector;

public class GameRules : SerializedMonoBehaviour
{
    public void Deal(UnityAction callback)
    {
        Debug.Log("Dealing Cards to Players");

        foreach (Player player in FindObjectsOfType<Player>())
        {
            while (player.hand.Count < (Game.gamePhase == Game.GamePhase.EarlyWar ? 8 : 9)) 
            {
                Card card = Game.deck.Draw();

                if (card) player.hand.Add(card);
                else break;
            }

            FindObjectOfType<UIHandManager>().Setup(player); 
        }
        callback.Invoke(); 
    }

    public void CheckHeldCards(UnityAction callback)
    {
        Debug.Log("Checking Held Cards"); 

        List<Game.Faction> factions = new List<Game.Faction>();

        foreach(Player player in FindObjectsOfType<Player>())
            foreach(Card card in player.hand)
                if (card is ScoringCard && !factions.Contains(player.faction))
                    factions.Add(player.faction);

        if (factions.Count == 2) // Tie
            Game.GameOver.Invoke(Game.Faction.Neutral, "Mutual Held Scoring Cards");
        else if (factions.Count == 1)
            Game.GameOver.Invoke(factions[0] == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA, "Held Scoring Card"); 
        else
            callback.Invoke();
    }

    public void ImproveDefcon(UnityAction callback)
    {
        DEFCON.AdjustDEFCON.Invoke(1);
        callback.Invoke(); 
    }

    public List<Country> westernEurope, easternEurope, southeastAsia;

    public void PlaceUSSRStartingInfluence(UnityAction callback) =>
        PlaceInfluence(Game.Faction.USSR, easternEurope, 6, callback);

    public void PlaceUSAStartingInfluence(UnityAction callback) =>
        PlaceInfluence(Game.Faction.USA, westernEurope, 7, callback); 

    public void PlaceBonusUSInfluence(UnityAction callback)
    {
        List<Country> eligibleCountries = new List<Country>();

        foreach(Country country in FindObjectsOfType<Country>())
            if(country.influence[Game.Faction.USA] > 0)
                eligibleCountries.Add(country);

        PlaceInfluence(Game.Faction.USA, eligibleCountries, 2, callback); 
    }

    CountryClickHandler countryClickHandler;
    public void PlaceInfluence(Game.Faction faction, List<Country> eligibleCountries, int amount, UnityAction callback)
    {
        FindObjectOfType<UIMessage>().Message($"Place {amount} {faction} Influence");
        countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick, Color.yellow);

        void onCountryClick(Country country, PointerEventData ped)
        {
            if(eligibleCountries.Contains(country))
            {
                Game.AdjustInfluence.Invoke(country, faction, 1);
                amount--;

                FindObjectOfType<UIMessage>().Message($"Place {amount} {faction} Influence");
            }

            if (amount == 0)
            {
                countryClickHandler.Close();
                callback.Invoke();
            }
        }
    }

    public void SetEarlyWar(UnityAction callback)
    {
        Game.coldwarPhase = Game.GamePhase.EarlyWar;
        callback.Invoke();
    }
}
