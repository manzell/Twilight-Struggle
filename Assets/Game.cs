using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector; 

public class Game : SerializedMonoBehaviour
{
    public enum Faction { Neutral, USSR, USA, China }
    public enum GamePhase { Setup, EarlyWar, Midwar, LateWar, FinalScoring }

    // These are here for Game-Rule reasons. They are not involved in managing the Sequence of Phases
    public static UnityEvent<Phase> 
        phaseStartEvent = new UnityEvent<Phase>(),
        phaseEndEvent = new UnityEvent<Phase>();     
    
    public static GameEvent<HeadlinePhase>
        headlineEvent = new GameEvent<HeadlinePhase>();

    public static UnityEvent<IPhaseAction> phaseActionEvent = new UnityEvent<IPhaseAction>();
    public static UnityEvent<Faction> setActiveFactionEvent = new UnityEvent<Faction>();

    public static GameEvent<Faction, int> AdjustDEFCON = new GameEvent<Faction, int>(); 
    public static GameEvent<int> AdjustVPs = new GameEvent<int>(); 
    public static GameEvent<Faction, int> AdjustMilOps = new GameEvent<Faction, int>();
    public static UnityEvent<Faction> AdvanceSpaceRate = new UnityEvent<Faction>();

    public static GameEvent<Country, Faction, int> AdjustInfluence = new GameEvent<Country, Faction, int>();
    public static GameEvent<Country, Faction, int> SetInfluence = new GameEvent<Country, Faction, int>();

    public static GameEvent<GameAction.Command> cardCommandEvent = new GameEvent<GameAction.Command>();

    public static UnityEvent GameStart = new UnityEvent();
    public static UnityEvent<Faction, string> GameOver = new UnityEvent<Faction, string>();

    public static UnityEvent<Country> CountryClick = new UnityEvent<Country>();
    public static UnityEvent<Card> CardClick = new UnityEvent<Card>();

    public static Faction phasingPlayer, actingPlayer;
    public static GamePhase gamePhase;
    public static Phase currentPhase; 
    public static Turn currentTurn; 
    public static ActionRound currentActionRound; 

    public static Deck deck;    

    public Dictionary<Faction, Player> playerMap = new Dictionary<Faction,Player>();    
    public List<Card> earlyWarCards, midwarCards, lateWarCards;

    private void Awake()
    {
        if (deck is null) deck = new Deck();

        deck.Shuffle();

        AdjustInfluence.AddListener(onAdjustInfluence);
        SetInfluence.AddListener(onSetInfluence); 
    }

    [Button] void AdvancePhase() => currentPhase.NextPhase(currentPhase.callback);

    // These are the only functions that should be allowed to touch the Game State
    public static void onAdjustInfluence(Country country, Faction faction, int amount) => country.influence[faction] = Mathf.Max(0, country.influence[faction] + amount);
    public static void onSetInfluence(Country country, Faction faction, int amount) => country.influence[faction] = amount;
}
