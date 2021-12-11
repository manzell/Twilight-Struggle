using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHeldCards : MonoBehaviour
{
    void CheckCards(Phase turn)
    {
        Debug.Log("Check Cards Step"); 

        foreach(Player player in FindObjectsOfType<Player>())
            foreach(Card card in player.hand)
                if(card is ScoringCard)
                    Game.GameOver.Invoke(player.faction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA, "Held Scoring Card"); 
    }
}
