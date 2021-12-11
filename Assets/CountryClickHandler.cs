using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// All this does is Manage Highlights and Turn a Listener on or off. Should it exist? 
public class CountryClickHandler
{
    Dictionary<Country, Outline> outlines = new Dictionary<Country, Outline>();
    UnityAction<Country, PointerEventData> action;
    Color color = Color.yellow; 

    // This is specific for coming from a Card Place Influence Action
    public CountryClickHandler(List<Country> countries, UnityAction<Country, PointerEventData> action, Color color)
    {
        // Add our Callback Listener 
        CountryMarker.Click.AddListener(this.action = action);

        // Set Color
        this.color = color;

        // Highlight Our Countries
        foreach (Country country in countries)
            AddHighlight(country);
    }

    public CountryClickHandler(List<Country> countries, UnityAction<Country, PointerEventData> action)
    {
        // Add our Callback Listener 
        CountryMarker.Click.AddListener(this.action = action);

        // Highlight Our Countries
        foreach (Country country in countries)
            AddHighlight(country);
    }

    public void Update(List<Country> countries)
    {
        foreach (Country country in countries)
            if (!outlines.ContainsKey(country)) AddHighlight(country);

        foreach (Country country in outlines.Keys)
            if (!countries.Contains(country)) RemoveHighlight(country);
    }

    public void Close()
    {
        // Turn off our Callback Listener 
        CountryMarker.Click.RemoveListener(action);

        foreach (Country country in outlines.Keys)
            RemoveHighlight(country);
    }

    public void AddHighlight(Country country)
    {
        if (!outlines.ContainsKey(country))
        {
            Outline outline = country.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(5f, 5f);

            outlines.Add(country, outline);
        }
    }

    public void RemoveHighlight(Country country) { 
        if (outlines.ContainsKey(country))
            GameObject.Destroy(outlines[country]);
    }
}
