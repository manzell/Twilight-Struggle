using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class CheckHeldCards : MonoBehaviour, TurnSystem.IPhaseAction
    {
        public void Action(TurnSystem.Phase phase, UnityAction callback)
        {
            List<Game.Faction> factions = new List<Game.Faction>();

            foreach (Player player in FindObjectsOfType<Player>())
                foreach (Card card in player.hand)
                    if (card is ScoringCard && !factions.Contains(player.faction))
                        factions.Add(player.faction);

            if (factions.Count == 2) // Both players held a card. 
                Game.GameOver.Invoke(Game.Faction.Neutral, "Mutual Held Scoring Cards");
            else if (factions.Count == 1)
                Game.GameOver.Invoke(factions[0] == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA, "Held Scoring Card");
            else
                callback.Invoke();
        }
    }
}