using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq; 

// All this does is Manage Highlights and Turn a Listener on or off. Should it exist? 
public class CountryClickHandler
{
    Dictionary<Country, Outline> outlines = new Dictionary<Country, Outline>();
    UnityAction<Country> callback;
    Color color = Color.yellow;

    public CountryClickHandler(List<Country> countries, UnityAction<Country> callback)
    {
        this.callback = callback;
        CountryMarker.Click.AddListener(onClick);

        foreach (Country country in countries)
            Add(country);
    }

    public CountryClickHandler(List<Country> countries, UnityAction<Country> callback, Color color) : this(countries, callback)
    {
        this.color = color;
    }

    void onClick(Country country)
    {
        if(outlines.ContainsKey(country))
            callback.Invoke(country);
    }

    public void Add(Country country)
    {
        if (!outlines.ContainsKey(country))
        {
            Outline outline = country.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(5f, 5f);

            outlines.Add(country, outline);
        }
    }

    public void Remove(Country country)
    {
        if (outlines.ContainsKey(country))
        {
            GameObject.Destroy(outlines[country]);
            outlines.Remove(country);
        }
    }

    public void Refresh(List<Country> countries)
    {
        foreach (Country country in countries)
            if (!outlines.ContainsKey(country)) 
                Add(country);

        foreach (Country country in outlines.Keys)
            if (!countries.Contains(country)) 
                Remove(country);
    }

    public void Close()
    {
        CountryMarker.Click.RemoveListener(onClick);

        foreach (Country country in outlines.Keys.ToList())
            Remove(country);
    }


}
