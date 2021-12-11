using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector; 

public class Game : SerializedMonoBehaviour
{
    public enum Faction { Neutral, USSR, USA, China }
    public enum GamePhase { Setup, EarlyWar, Midwar, LateWar, FinalScoring }

    public static GamePhase gamePhase = GamePhase.Setup;

    public static UnityEvent<Phase> // Reminder, these don't manage the turn/phase, **JUST** gamestate effects. 
        TurnStart = new UnityEvent<Phase>(),
        TurnEnd = new UnityEvent<Phase>(); 
    
    public static UnityEvent<ActionRound> // Reminder, these don't manage the turn/phase, **JUST** gamestate effects. 
        ActionRoundStart = new UnityEvent<ActionRound>(),
        ActionRoundEnd = new UnityEvent<ActionRound>();

    public static UnityEvent<Headline>
        HeadlineStart = new UnityEvent<Headline>(),
        HeadlineEnd = new UnityEvent<Headline>();
    
    //public static UnityEvent<Headline> // Reminder, these don't manage the turn/phase, **JUST** gamestate effects. 
    //    HeadlinePhase = new UnityEvent<Headline>(),
    //    HeadlineEvent = new UnityEvent<Headline>();Fac

    //public static UnityEvent<Coup> Coup = new UnityEvent<Coup>();
    //public static UnityEvent<Coup> CoupSelectTarget = new UnityEvent<Coup>();
    //public static UnityEvent<Coup> CoupConfirm = new UnityEvent<Coup>();

    //public static UnityEvent<Realign> Realign = new UnityEvent<Realign>();
    //public static UnityEvent<Realign.RealignAttempt> StartRealign = new UnityEvent<Realign.RealignAttempt>();

    //public static UnityEvent<SpaceRace>
    //    StartSpaceShot = new UnityEvent<SpaceRace>(),
    //    SpaceShot = new UnityEvent<SpaceRace>(); 

    public static UnityEvent<Scoring> Score = new UnityEvent<Scoring>();

    public static UnityEvent<CardEvent> CardEvent = new UnityEvent<CardEvent>();

    public static UnityEvent<PlaceInfluence> InfluenceSelectTarget = new UnityEvent<PlaceInfluence>();
    public static UnityEvent<PlaceInfluence> InfluencePlacement = new UnityEvent<PlaceInfluence>();

    public static UnityEvent<Faction, int> AdjustDEFCON = new UnityEvent<Faction, int>(); 
    public static GameEvent<int> AwardVictoryPoints = new GameEvent<int>(); 
    public static UnityEvent<Faction, int> AdjustMilOps = new UnityEvent<Faction, int>();
    public static UnityEvent<Faction> AdvanceSpaceRate = new UnityEvent<Faction>();

    public static GameEvent<Country, Faction, int> AdjustInfluence = new GameEvent<Country, Faction, int>();
    public static GameEvent<Country, Faction, int> SetInfluence = new GameEvent<Country, Faction, int>();

    public static UnityEvent GameStart = new UnityEvent();
    public static UnityEvent<Faction, string> GameOver = new UnityEvent<Faction, string>();

    public static UnityEvent<Country> CountryClick = new UnityEvent<Country>();

    public static Faction phasingPlayer;
    public static GamePhase coldwarPhase;

    public static Phase currentPhase; 
    public static Turn currentTurn; 
    public static ActionRound currentActionRound; 

    public static Deck deck;

    public static void onAdjustInfluence(Country country, Faction faction, int amount) => country.influence[faction] = Mathf.Max(0, country.influence[faction] + amount);
    public static void onSetInfluence(Country country, Faction faction, int amount) => country.influence[faction] = Mathf.Max(0, country.influence[faction] + amount);
    

    public Dictionary<Faction, Player> playerMap = new Dictionary<Faction,Player>();
    
    public List<Card> earlyWarCards, midwarCards, lateWarCards;

    private void Awake()
    {
        if (deck is null) deck = new Deck();

        deck.AddRange(earlyWarCards);
        deck.Shuffle();

        AdjustInfluence.AddListener(onAdjustInfluence);
        SetInfluence.AddListener(onSetInfluence); 
    }
}
