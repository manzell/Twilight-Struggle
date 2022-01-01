using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace TwilightStruggle
{
    public class Game : SerializedMonoBehaviour
    {
        public enum Faction { Neutral, USSR, USA, China }
        public enum GamePhase { Setup, EarlyWar, Midwar, LateWar, FinalScoring }

        // These are here for Game-Rule reasons. They are not involved in managing the Sequence of Phases
        public static UnityEvent<TurnSystem.Phase>
            phaseStartEvent = new UnityEvent<TurnSystem.Phase>(),
            phaseEndEvent = new UnityEvent<TurnSystem.Phase>();

        public static GameEvent<TurnSystem.HeadlinePhase> headlineEvent = new GameEvent<TurnSystem.HeadlinePhase>();

        public static UnityEvent<TurnSystem.IPhaseAction> phaseActionEvent = new UnityEvent<TurnSystem.IPhaseAction>();
        public static GameEvent<Faction> setActingFactionEvent = new GameEvent<Faction>();

        public static GameEvent<Country, Faction, int> adjustInfluenceEvent = new GameEvent<Country, Faction, int>();
        public static GameEvent<Country, Faction, int> setInfluenceEvent = new GameEvent<Country, Faction, int>();

        public static UnityEvent<Faction, List<Card>> dealCardsEvent = new UnityEvent<Faction, List<Card>>();

        public static UnityEvent gameStartEvent = new UnityEvent();
        public static UnityEvent<Faction, string> GameOver = new UnityEvent<Faction, string>();

        public static UnityEvent<Country> CountryClick = new UnityEvent<Country>();
        public static UnityEvent<Card> CardClick = new UnityEvent<Card>();

        public static Faction phasingPlayer, actingPlayer = Faction.USSR;
        public static GamePhase gamePhase;

        public static TurnSystem.Phase currentPhase;
        public static TurnSystem.Turn currentTurn;
        public static TurnSystem.ActionRound currentActionRound;

        public static Deck deck;

        public Dictionary<Faction, Player> playerMap = new Dictionary<Faction, Player>();
        public List<Card> earlyWarCards, midwarCards, lateWarCards;

        private void Awake()
        {
            deck = new Deck(); 
            gameStartEvent.AddListener(() => deck.Shuffle()); // TODO: Move this. 
        }

        [Button] public void AdvancePhase() => currentPhase.NextPhase(currentPhase.callback);

        public static void SetInfluence(Country country, Faction faction, int amount) =>
            AdjustInfluence(country, faction, amount - country.influence[faction]);

        public static void AdjustInfluence(Country country, Faction faction, int amount)
        {
            country.influence[faction] = Mathf.Max(0, country.influence[faction] + amount);
            adjustInfluenceEvent.Invoke(country, faction, amount); 
        }

        public static void SetActingFaction(Faction faction)
        {
            if(actingPlayer != faction)
            {
                actingPlayer = faction;
                setActingFactionEvent.Invoke(faction);
            }
        }

        public static void SetPhasingFaction(Faction faction)
        {
            if (phasingPlayer != faction)
            {
                phasingPlayer = faction;
                actingPlayer = faction;
                setActingFactionEvent.Invoke(faction);
            }
        }
    }
}