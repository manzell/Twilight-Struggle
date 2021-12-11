using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring
{
    public enum ScoreState { Absence, Presence, Domination, Control }
    Dictionary<ScoreState, int> scoreKey; 

    public Dictionary<Game.Faction, ScoreState> scoreState;

    public int vp => scoreKey[scoreState[Game.Faction.USA]] + USbattlegrounds - scoreKey[scoreState[Game.Faction.USSR]] - USSRbattlegrounds;

    int USbattlegrounds = 0, USCountries = 0,
        USSRbattlegrounds = 0, USSRCountries = 0;

    public Scoring(Dictionary<ScoreState, int> scoreKey, List<Country> battlegrounds, List<Country> nonBattlegrounds)
    {
        this.scoreKey = scoreKey;

        if (!scoreKey.ContainsKey(ScoreState.Absence)) 
            scoreKey.Add(ScoreState.Absence, 0);

        scoreState = new Dictionary<Game.Faction, ScoreState> { { Game.Faction.USA, ScoreState.Absence }, { Game.Faction.USSR, ScoreState.Absence } };

        List<Country> countries = new List<Country>(battlegrounds);
        countries.AddRange(nonBattlegrounds);

        foreach (Country country in countries)
        {
            if (country.control == Game.Faction.USA)
            {
                USCountries++;

                if (country.isBattleground)
                    USbattlegrounds++;
            }
            else if (country.control == Game.Faction.USSR)
            {
                USSRCountries++;

                if (country.isBattleground)
                    USSRbattlegrounds++;
            }
        }

        if (USCountries > 0)
            scoreState[Game.Faction.USA] = ScoreState.Presence;
        if (USbattlegrounds > USSRbattlegrounds && USCountries > USSRCountries && USCountries > USbattlegrounds)
            scoreState[Game.Faction.USA] = ScoreState.Domination;
        if (USbattlegrounds == battlegrounds.Count)
            scoreState[Game.Faction.USA] = ScoreState.Control;
        if (USSRCountries > 0)
            scoreState[Game.Faction.USSR] = ScoreState.Presence;
        if (USSRbattlegrounds > USbattlegrounds && USSRCountries > USCountries && USSRCountries > USSRbattlegrounds)
            scoreState[Game.Faction.USSR] = ScoreState.Domination;
        if (USSRbattlegrounds == battlegrounds.Count)
            scoreState[Game.Faction.USSR] = ScoreState.Control;
    }

}
