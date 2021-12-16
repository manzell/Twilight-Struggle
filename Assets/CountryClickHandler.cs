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
    UnityAction<Country> callback;
    Color color = Color.yellow; 

    // This is specific for coming from a Card Place Influence Action
    public CountryClickHandler(List<Country> countries, UnityAction<Country> callback, Color color)
    {
        CountryMarker.Click.AddListener(this.callback = callback);

        this.color = color;

        foreach (Country country in countries)
            AddHighlight(country);
    }

    public CountryClickHandler(List<Country> countries, UnityAction<Country> callback)
    {
        this.callback = callback;
        CountryMarker.Click.AddListener(onClick);

        foreach (Country country in countries)
            AddHighlight(country);
    }

    void onClick(Country country)
    {
        if(outlines.ContainsKey(country))
            callback.Invoke(country);
    }

    public void Refresh(List<Country> countries)
    {
        foreach (Country country in countries)
            if (!outlines.ContainsKey(country)) 
                AddHighlight(country);

        foreach (Country country in outlines.Keys)
            if (!countries.Contains(country)) 
                Remove(country);
    }

    public void Close()
    {
        CountryMarker.Click.RemoveListener(onClick);

        foreach (Country country in outlines.Keys)
            Remove(country);
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

    public void Remove(Country country) { 
        if (outlines.ContainsKey(country))
        {
            GameObject.Destroy(outlines[country]);
            outlines.Remove(country);
        }
    }
}
