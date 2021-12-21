using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public abstract class Card : SerializedMonoBehaviour
{

    // TODO: send to ScriptableObject
    public string cardName;
    public Game.Faction faction;
    public int ops = 0;
    public int bonusOps = 0;
    public int OpsValue => ops + bonusOps;
    public string cardText;
    public bool removeOnEvent = false;

    [HideInInspector] public GameEvent<HeadlinePhase> headlineEvent = new GameEvent<HeadlinePhase>();
    [HideInInspector] public UnityEvent<Card> clickEvent = new UnityEvent<Card>();    
    [HideInInspector] public GameEvent<GameAction.Command> cardCommandEvent = new GameEvent<GameAction.Command>();
    protected static UIManager uiManager => FindObjectOfType<UIManager>();

    protected static CountryClickHandler countryClickHandler;
    protected static CardClickHandler cardClickHandler;

    public abstract void CardEvent(GameAction.Command command);

    protected static void RemoveInfluence(Game.Faction faction, Dictionary<Country, int> countries)
    {
        foreach (Country country in countries.Keys)
            Game.AdjustInfluence.Invoke(country, faction, -countries[country]);
    }
    protected static void RemoveInfluence(Game.Faction faction, List<Country> countries, int influence)
    {
        foreach (Country country in countries)
            Game.AdjustInfluence.Invoke(country, faction, -influence);
    }
    protected static void RemoveInfluence(Game.Faction faction, Country country, int influence)
    {
        if (influence != 0)
            Game.AdjustInfluence.Invoke(country, faction, -influence); 
    }
    protected static void RemoveInfluence(List<Country> countries, Game.Faction faction, int totalInfluence, int maxPerCountry, UnityAction callback) =>
        AddInfluence(countries, faction, -totalInfluence, maxPerCountry, callback);

    protected static void AddInfluence(Game.Faction faction, Dictionary<Country, int> countries)
    {
        foreach (Country country in countries.Keys)
            Game.AdjustInfluence.Invoke(country, faction, countries[country]);
    }
    protected static void AddInfluence(Game.Faction faction, List<Country> countries, int influence)
    {
        foreach (Country country in countries)
            Game.AdjustInfluence.Invoke(country, faction, influence);
    }
    protected static void AddInfluence(Game.Faction faction, Country country, int influence)
    {
        if (influence != 0)
            Game.AdjustInfluence.Invoke(country, faction, influence);
    }
    protected static void AddInfluence(List<Country> countries, Game.Faction faction, int totalInfluence, int maxPerCountry, UnityAction callback)
    {
        List<Country> removedCountries = new List<Country>();

        countryClickHandler = new CountryClickHandler(countries, onCountryClick);

        void onCountryClick(Country country)
        {
            if (maxPerCountry == 0 && removedCountries.CountOf(country) < maxPerCountry)
            {
                removedCountries.Add(country);
                Game.AdjustInfluence.Invoke(country, faction, totalInfluence > 0 ? 1 : -1);
                totalInfluence--;
            }

            if (removedCountries.CountOf(country) == maxPerCountry)
            {
                countryClickHandler.Remove(country);
                countries.Remove(country);
            }

            if (totalInfluence == 0)
            {
                countryClickHandler.Close();
                callback.Invoke(); 
            }
        }
    }

    protected static void SetInfluence(Game.Faction faction, List<Country> countries, int influence)
    {
        foreach (Country country in countries)
            Game.SetInfluence.Invoke(country, faction, influence);
    }

    public void PlaceInfluence(Game.Faction faction, List<Country> eligibleCountries, int amount, UnityAction callback)
    {
        FindObjectOfType<UIMessage>().Message($"{(amount > 0 ? "Place" : "Remove")} {Mathf.Abs(amount)} {faction} Influence");
        countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick, Color.yellow);

        void onCountryClick(Country country)
        {
            if (eligibleCountries.Contains(country))
            {
                int sign = amount / Mathf.Abs(amount); // If we submit a negative influence, we want to remove influence rather than add it. 
                Game.AdjustInfluence.Invoke(country, faction, 1 * sign);
                amount--;

                FindObjectOfType<UIMessage>().Message($"{(amount > 0 ? "Place" : "Remove")} {Mathf.Abs(amount)} {faction} Influence");
            }

            if (amount == 0)
            {
                countryClickHandler.Close();
                callback.Invoke(); 
            }
        }
    }

    protected static void Message(string message) =>FindObjectOfType<UIMessage>().Message(message); 
}