using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public abstract class Card : SerializedMonoBehaviour
    {
        // TODO: send to ScriptableObject
        public string cardName;
        public Game.Faction faction;
        [SerializeField] int ops = 0;
        public int bonusOps = 0;
        public int opsValue => ops + bonusOps;
        public string cardText;
        public bool removeOnEvent = false;
        [HideInInspector] public UnityEvent<Card> clickEvent = new UnityEvent<Card>();
        [HideInInspector] public UnityEvent<Game.Faction, Country, int> adjustInfluenceEvent; 

        protected static UI.CountryClickHandler countryClickHandler;
        protected static UI.CardClickHandler cardClickHandler;
        protected static UI.UIMessage _messenger; 

        private void Awake()
        {
            _messenger = FindObjectOfType<UI.UIMessage>(); 
        }

        protected void RemoveInfluence(Game.Faction faction, Dictionary<Country, int> countries)
        {
            foreach (Country country in countries.Keys)
                Game.AdjustInfluence(country, faction, -countries[country]);
        }
        protected void RemoveInfluence(Game.Faction faction, List<Country> countries, int influence)
        {
            foreach (Country country in countries)
                Game.AdjustInfluence(country, faction, -influence);
        }
        protected void RemoveInfluence(Game.Faction faction, Country country, int influence)
        {
            if (influence != 0)
                Game.AdjustInfluence(country, faction, -influence);
        }
        public static void RemoveInfluence(List<Country> countries, Game.Faction faction, int totalInfluence, int maxPerCountry, UnityAction callback) =>
            AddInfluence(countries, faction, -totalInfluence, maxPerCountry, callback);

        protected void AddInfluence(Game.Faction faction, Dictionary<Country, int> countries)
        {
            foreach (Country country in countries.Keys)
                Game.AdjustInfluence(country, faction, countries[country]);
        }
        protected void AddInfluence(Game.Faction faction, List<Country> countries, int influence)
        {
            foreach (Country country in countries)
                Game.AdjustInfluence(country, faction, influence);
        }
        protected void AddInfluence(Game.Faction faction, Country country, int influence)
        {
            if (influence != 0)
                Game.AdjustInfluence(country, faction, influence);
        }
        
        public static void AddInfluence(List<Country> countries, Game.Faction faction, int totalInfluence, int maxPerCountry, UnityAction callback)
        {
            List<Country> selectedCountries = new List<Country>();

            UI.CountryClickHandler.Setup(countries, onCountryClick);

            void onCountryClick(Country country)
            {
                if (maxPerCountry == 0 || selectedCountries.CountOf(country) < maxPerCountry)
                {
                    selectedCountries.Add(country);
                    Game.AdjustInfluence(country, faction, totalInfluence > 0 ? 1 : -1);
                    totalInfluence -= totalInfluence > 0 ? 1 : -1;
                }

                if (selectedCountries.CountOf(country) == maxPerCountry ||
                    (totalInfluence < 0 && country.influence[faction] == 0)) // cannot remove from countries w/ Zero influence :)
                {
                    UI.CountryClickHandler.Remove(country);
                    countries.Remove(country);
                }

                if (totalInfluence == 0)
                {
                    UI.CountryClickHandler.Close();
                    callback.Invoke();
                }
            }
        }

        public static void SetInfluence(Game.Faction faction, List<Country> countries, int influence)
        {
            foreach (Country country in countries)
                Game.SetInfluence(country, faction, influence);
        }

        public abstract void CardEvent(GameCommand command);
        public void PlaceInfluence(Game.Faction faction, List<Country> eligibleCountries, int influenceAmt, UnityAction callback)
        {
            Message($"{(influenceAmt > 0 ? "Place" : "Remove")} {Mathf.Abs(influenceAmt)} {faction} Influence");
            int sign = influenceAmt / Mathf.Abs(influenceAmt); // If we submit a negative influence, we want to remove influence rather than add it. 

            UI.CountryClickHandler.Setup(eligibleCountries, onCountryClick, Color.yellow);

            void onCountryClick(Country country)
            {
                if (eligibleCountries.Contains(country))
                {
                    Game.AdjustInfluence(country, faction, 1 * sign);
                    influenceAmt -= 1 * sign;

                    Message($"{(influenceAmt > 0 ? "Place" : "Remove")} {Mathf.Abs(influenceAmt)} {faction} Influence");
                }

                if (influenceAmt == 0)
                {
                    UI.CountryClickHandler.Close();
                    callback.Invoke();
                }
            }
        }

        protected static void Message(string message) => _messenger.Message(message);
    }
}