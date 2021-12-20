using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class AddCards : MonoBehaviour, IPhaseAction
{
    public Game.GamePhase gamePhase; 
    public List<Card> cards;

    public void OnPhase(Phase phase, UnityAction callback)
    {
        Debug.Log($"*** {gamePhase} cards added ***"); 
        Game.gamePhase = gamePhase;

        Game.deck.AddRange(cards);
        Game.deck.Shuffle(); 

        callback.Invoke();
    }
}
