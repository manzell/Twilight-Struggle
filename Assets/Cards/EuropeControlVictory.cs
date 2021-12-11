using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EuropeControlVictory : MonoBehaviour
{
    public void EuropeControlWin(Scoring scoring)
    { 
        if (scoring.scoreState[Game.Faction.USA] == Scoring.ScoreState.Control)
            Game.GameOver.Invoke(Game.Faction.USA, "Europe Control");
        if (scoring.scoreState[Game.Faction.USSR] == Scoring.ScoreState.Control)
            Game.GameOver.Invoke(Game.Faction.USSR, "Europe Control");
    }
}
