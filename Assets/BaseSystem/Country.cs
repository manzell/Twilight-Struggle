using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TwilightStruggle
{
    public class Country : SerializedMonoBehaviour
    {
        public enum Continent { Europe, Asia, MiddleEast, Africa, SouthAmerica, CentralAmerica }

        public CountryData countryData;
        public string countryName;
        public Continent continent;
        public int stability;
        public bool isBattleground;

        public Game.Faction adjacentSuperpower;
        public List<Country> adjacentCountries = new List<Country>();
        public Dictionary<Game.Faction, int> influence = new Dictionary<Game.Faction, int> {
        { Game.Faction.USSR, 0 },
        { Game.Faction.USA, 0 }
    };

        public UICountry marker;

        void Awake()
        {
            if (countryData != null)
            {
                countryName = countryData.countryName;
                continent = countryData.continent;
                stability = countryData.stability;
                isBattleground = countryData.isBattleground;
            }
        }

        public Game.Faction control
        {
            get
            {
                int margin = influence[Game.Faction.USA] - influence[Game.Faction.USSR];

                if (margin >= stability) return Game.Faction.USA;
                else if (Mathf.Abs(margin) >= stability) return Game.Faction.USSR;
                else return Game.Faction.Neutral;
            }
        }
    }
}