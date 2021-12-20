using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deal : MonoBehaviour, IPhaseAction
{
    public int dealUpTo = 8; 

    public void OnPhase(Phase phase, UnityAction callback)
    {
        foreach (Player player in FindObjectsOfType<Player>())
        {
            //player.hand.AddRange(Game.deck.Draw(dealUpTo - player.hand.Count));

            while (player.hand.Count < dealUpTo)
            {

                Card card = Game.deck.Draw();

                if(Game.deck.Contains(card))
                {
                    Debug.Log($"Card {card.cardName} drawn by {player.faction} but still in our Deck. Why?");
                    Game.deck.Remove(card);
                }

                if(player.hand.Contains(card))
                {
                    Debug.Log($"Warning, {card.cardName} drawn by {player.faction} is a duplicate. -- Continuing --");
                    continue; 
                }

                if (card)
                    player.hand.Add(card);
                else
                    break; // should only occur if our deck is entirely empty. 
            }
        }

        callback.Invoke();
    }
}
