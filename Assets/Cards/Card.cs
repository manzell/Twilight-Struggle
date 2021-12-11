using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class Card : SerializedMonoBehaviour
{
    public string cardName;
    public Game.Faction faction;
    public int opsValue = 0; 
    public string cardText;
    public bool removeOnEvent = false; 

    public UnityEvent Play = new UnityEvent();
    public UnityEvent<Dictionary<Game.Faction, Card>> Headline = new UnityEvent<Dictionary<Game.Faction, Card>>();

    [HideInInspector] public abstract void Event(UnityEngine.Events.UnityAction callback);
    [HideInInspector] public virtual void OnHeadline(Dictionary<Game.Faction, Card> headlines) { }

    protected UnityEngine.Events.UnityAction callback;

    private void Awake()
    {
        Headline.AddListener(OnHeadline); 
    }

    protected void RemoveInfluence(Game.Faction faction, Dictionary<Country, int> countries)
    {
        foreach (Country country in countries.Keys)
            Game.AdjustInfluence.Invoke(country, faction, -countries[country]);
    }

    protected void RemoveInfluence(Game.Faction faction, List<Country> countries, int influence)
    {
        foreach (Country country in countries)
            Game.AdjustInfluence.Invoke(country, faction, -influence);
    }

    protected void AddInfluence(Game.Faction faction, Dictionary<Country, int> countries)
    {
        foreach (Country country in countries.Keys)
            Game.AdjustInfluence.Invoke(country, faction, countries[country]);
    }

    protected void AddInfluence(Game.Faction faction, List<Country> countries, int influence)
    {
        foreach (Country country in countries)
            Game.AdjustInfluence.Invoke(country, faction, influence);
    }

    protected void SetInfluence(Game.Faction faction, List<Country> countries, int influence)
    {
        foreach (Country country in countries)
            Game.SetInfluence.Invoke(country, faction, influence);
    }

    protected static CountryClickHandler countryClickHandler;
    public static void PlaceInfluence(Game.Faction faction, List<Country> eligibleCountries, int amount, UnityAction callback)
    {
        FindObjectOfType<UIMessage>().Message($"{(amount > 0 ? "Place" : "Remove")} {Mathf.Abs(amount)} {faction} Influence");
        countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick, Color.yellow);

        void onCountryClick(Country country, PointerEventData ped)
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
}
