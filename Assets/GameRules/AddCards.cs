using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class AddCards : MonoBehaviour, TurnSystem.IPhaseAction
    {
        public Game.GamePhase gamePhase;
        public List<Card> cards;

        public void Action(TurnSystem.Phase phase, UnityAction callback)
        {
            Game.gamePhase = gamePhase;

            Game.deck.AddRange(cards);
            Game.deck.Shuffle();

            callback.Invoke();
        }
    }
}