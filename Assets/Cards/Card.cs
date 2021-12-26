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
        public int OpsValue => ops + bonusOps;
        public string cardText;
        public bool removeOnEvent = false;

        [HideInInspector] public GameEvent<HeadlinePhase> headlineEvent = new GameEvent<HeadlinePhase>();
        [HideInInspector] public UnityEvent<Card> clickEvent = new UnityEvent<Card>();
        [HideInInspector] public GameEvent<GameAction.Command> cardCommandEvent = new GameEvent<GameAction.Command>();
        protected static UIManager uiManager => FindObjectOfType<UIManager>();

        protected static CountryClickHandler countryClickHandler;
        protected static CardClickHandler cardClickHandler;
        protected static UnityEvent<Country> adjustInfluenceEvent = new UnityEvent<Country>();
        protected static UnityEvent<Card> cardClickEvent = new UnityEvent<Card>();

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

            CountryClickHandler.Setup(countries, onCountryClick);

            void onCountryClick(Country country)
            {
                Debug.Log($"AdjustInfluence: {totalInfluence}");
                if (maxPerCountry == 0 || removedCountries.CountOf(country) < maxPerCountry)
                {
                    removedCountries.Add(country);
                    Game.AdjustInfluence.Invoke(country, faction, totalInfluence > 0 ? 1 : -1);
                    totalInfluence += totalInfluence < 0 ? 1 : -1;
                }

                adjustInfluenceEvent.Invoke(country);

                if (removedCountries.CountOf(country) == maxPerCountry ||
                    totalInfluence < 0 && country.influence[faction] == 0) // cannot remove from countries w/ Zero influence :)
                {
                    CountryClickHandler.Remove(country);
                    countries.Remove(country);
                }

                if (totalInfluence == 0)
                {
                    CountryClickHandler.Close();
                    callback.Invoke();
                }
            }
        }

        protected static void SetInfluence(Game.Faction faction, List<Country> countries, int influence)
        {
            foreach (Country country in countries)
                Game.SetInfluence.Invoke(country, faction, influence);
        }

        public void PlaceInfluence(Game.Faction faction, List<Country> eligibleCountries, int influenceAmt, UnityAction callback)
        {
            Message($"{(influenceAmt > 0 ? "Place" : "Remove")} {Mathf.Abs(influenceAmt)} {faction} Influence");
            int sign = influenceAmt / Mathf.Abs(influenceAmt); // If we submit a negative influence, we want to remove influence rather than add it. 

            CountryClickHandler.Setup(eligibleCountries, onCountryClick, Color.yellow);

            void onCountryClick(Country country)
            {
                if (eligibleCountries.Contains(country))
                {
                    Game.AdjustInfluence.Invoke(country, faction, 1 * sign);
                    influenceAmt -= 1 * sign;

                    Message($"{(influenceAmt > 0 ? "Place" : "Remove")} {Mathf.Abs(influenceAmt)} {faction} Influence");
                }

                adjustInfluenceEvent.Invoke(country);

                if (influenceAmt == 0)
                {
                    CountryClickHandler.Close();
                    callback.Invoke();
                }
            }
        }

        protected static void Message(string message) => FindObjectOfType<UIMessage>().Message(message);
    }
}