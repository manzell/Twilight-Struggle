using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

namespace TwilightStruggle
{
    public class GameAction : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<GameCommand>
            prepareEvent = new UnityEvent<GameCommand>(),
            targetEvent = new UnityEvent<GameCommand>(),
            completeEvent = new UnityEvent<GameCommand>(); 

        void Awake()
        {
            GetComponent<UI.UIDropHandler>()?.cardDropEvent.AddListener(card => GameCommand.Create(Game.actingPlayer, card, this).Prepare());
        }

        public virtual bool CanUseAction(Game.Faction actingPlayer, Card card) => true;
    }
}