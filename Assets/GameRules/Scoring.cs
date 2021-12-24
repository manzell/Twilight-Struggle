using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring
{
    public enum ScoreState { Absence, Presence, Domination, Control }

    Dictionary<ScoreState, int> scoreKey; 
    public Dictionary<Game.Faction, ScoreState> scoreState;

    public int vp => (scoreKey[scoreState[Game.Faction.USA]] + USbattlegrounds + USAadjacents) - 
        (scoreKey[scoreState[Game.Faction.USSR]] + USSRbattlegrounds + USSRadjacents);

    public int totalBattlegrounds = 0,
        USbattlegrounds = 0, USCountries = 0, USAadjacents = 0,
        USSRbattlegrounds = 0, USSRCountries = 0, USSRadjacents = 0;

    public Game.Faction scoringFaction
    {
        get
        {
            if (vp > 0) return Game.Faction.USA;
            else if (vp < 0) return Game.Faction.USSR;
            else return Game.Faction.Neutral; 
        }
    }

    public Scoring(Dictionary<ScoreState, int> scoreKey, Country.Continent continent)
    {
        this.scoreKey = scoreKey;

        // This just ensures when we set up the score key for the Continent, we have a zero state. 
        if (!scoreKey.ContainsKey(ScoreState.Absence)) 
            scoreKey.Add(ScoreState.Absence, 0);

        scoreState = new Dictionary<Game.Faction, ScoreState> { { Game.Faction.USA, ScoreState.Absence }, { Game.Faction.USSR, ScoreState.Absence } };

        foreach (Country country in GameObject.FindObjectsOfType<Country>())
        {
            if (country.continent == continent)
            {
                if (country.isBattleground) 
                    totalBattlegrounds++; 

                if (country.control == Game.Faction.USA)
                {
                    USCountries++;

                    if (country.isBattleground) { }
                        USbattlegrounds++;
                    if (country.adjacentSuperpower == Game.Faction.USSR)
                        USAadjacents++;
                }
                else if (country.control == Game.Faction.USSR)
                {
                    USSRCountries++;

                    if (country.isBattleground)
                        USSRbattlegrounds++;
                    if (country.adjacentSuperpower == Game.Faction.USA)
                        USSRadjacents++;
                }
            }
        }

        if (USCountries > 0)
            scoreState[Game.Faction.USA] = ScoreState.Presence;
        if (USbattlegrounds > USSRbattlegrounds && USCountries > USSRCountries && USCountries > USbattlegrounds)
            scoreState[Game.Faction.USA] = ScoreState.Domination;
        if (USbattlegrounds == totalBattlegrounds)
            scoreState[Game.Faction.USA] = ScoreState.Control;

        if (USSRCountries > 0)
            scoreState[Game.Faction.USSR] = ScoreState.Presence;
        if (USSRbattlegrounds > USbattlegrounds && USSRCountries > USCountries && USSRCountries > USSRbattlegrounds)
            scoreState[Game.Faction.USSR] = ScoreState.Domination;
        if (USSRbattlegrounds == totalBattlegrounds)
            scoreState[Game.Faction.USSR] = ScoreState.Control;
    }

}
