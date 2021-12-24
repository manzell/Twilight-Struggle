using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Linq; 

// All this does is Manage Highlights and Turn a Listener on or off. Should it exist? 
public class CountryClickHandler
{
    static Dictionary<Country, UICountry> _highlights = new Dictionary<Country, UICountry>();
    static UnityAction<Country> _callback;
    static Color _color = Color.yellow;

    public static void Setup(List<Country> countries, UnityAction<Country> callback)
    {
        _callback = callback;

        foreach (Country country in countries)
        {
            Add(country);
            _highlights[country].clickEvent.AddListener(onClick); 
        }
    }

    public static void Setup(List<Country> countries, UnityAction<Country> callback, Color color)
    {
        _color = color;
        Setup(countries, callback);
    }

    private static void onClick(Country country)
    {
        if(_highlights.ContainsKey(country))
            _callback.Invoke(country);
    }

    public static void Add(Country country)
    {
        if (!_highlights.ContainsKey(country))
        {
            _highlights.Add(country, UICountry.countryMap[country]);

            UICountry.countryMap[country].Shadow.DOColor(_color, .3f);
            UICountry.countryMap[country].Shadow.DOFade(.75f, .3f);
        }
    }

    public static void Remove(Country country)
    {
        if (_highlights.ContainsKey(country))
        {            
            _highlights[country].ResetShadow();
            _highlights[country].clickEvent.RemoveListener(onClick);
            _highlights.Remove(country);
        }
    }

    public static void Refresh(List<Country> countries)
    {
        foreach (Country country in countries)
            if (!_highlights.ContainsKey(country)) 
                Add(country);

        foreach (Country country in _highlights.Keys.ToList())
            if (!countries.Contains(country)) 
                Remove(country);
    }

    public static void Close()
    {
        foreach (Country country in _highlights.Keys.ToList())
            Remove(country);
    }
}
