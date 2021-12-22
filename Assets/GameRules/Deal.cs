using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deal : MonoBehaviour, IPhaseAction
{
    public int dealUpTo = 8; 

    public void OnPhase(Phase phase, UnityAction callback)
    {
        foreach (Player _player in FindObjectsOfType<Player>())
        {
            // TODO: Double Check for the China Card and Make sure to not count it in our draw-up
            List<Card> _cards = Game.deck.Draw(dealUpTo - _player.hand.Count);

            _player.hand.AddRange(_cards);
            Game.dealCardsEvent.Invoke(_player.faction, _cards);
        }

        callback.Invoke();
    }
}
