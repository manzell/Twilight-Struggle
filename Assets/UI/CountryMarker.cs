using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events; 
using UnityEngine.EventSystems;

public class CountryMarker : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Country country;

    [SerializeField] Image flag, countryNameBG, stabilityBG, USinfluenceBG, USSRinfluenceBG;
    [SerializeField] TextMeshProUGUI countryName, stability, USinfluence, USSRinfluence;

    static Dictionary<Country, CountryMarker> markerLookup = new Dictionary<Country, CountryMarker>();
    static GameObject Line;

    public static UnityEvent<Country> Click = new UnityEvent<Country>();

    private void Awake()
    {
        Game.AdjustInfluence.after.AddListener(onAddInfluence);
    }

    [Button]
    public void Setup()
    {
        if (!country) country = GetComponent<Country>();

        countryName.text = country.countryName;
        stability.text = country.stability.ToString();
        name = $"{country.countryName} Marker";

        if (markerLookup.ContainsKey(country))
            markerLookup[country] = this;
        else markerLookup.Add(country, this);

        if (country.isBattleground)
        {
            countryNameBG.color = Color.red;
            countryName.color = Color.white;
        }
        else
        {
            countryNameBG.color = Color.gray;
            countryName.color = Color.black;
        }

        onAddInfluence(country, Game.Faction.Neutral, 0);
    }

    public void onAddInfluence(Country c, Game.Faction faction, int amount)
    {
        if (c != country) return;

        USinfluence.text = country.influence[Game.Faction.USA].ToString();
        USSRinfluence.text = country.influence[Game.Faction.USSR].ToString();

        if (country.control == Game.Faction.USA)
        {
            USinfluenceBG.color = Color.blue;
            USinfluence.color = Color.white;
            USSRinfluenceBG.color = Color.white; 
            USSRinfluence.color = Color.red;
        }
        else if (country.control == Game.Faction.USSR)
        {
            USinfluenceBG.color = Color.white;
            USinfluence.color = Color.blue;
            USSRinfluenceBG.color = Color.red;
            USSRinfluence.color = Color.white;
        }
        else
        {
            USinfluenceBG.color = Color.white;
            USinfluence.color = Color.blue;
            USSRinfluenceBG.color = Color.white;
            USSRinfluence.color = Color.red;
        }
    }

    public void OnPointerClick(PointerEventData data) {
        Click.Invoke(country); 
    }

}