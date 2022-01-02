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

        public void Awake()
        {
            GetComponent<UI.CardDropHandler>()?.cardDropEvent.AddListener(OnCardDrop);
        }

        public void OnCardDrop(Card card)
        {
            GameCommand command = GameCommand.Create(Game.actingPlayer, card, this);
            command.Prepare();
        }

        public void Finish(GameCommand command)
        {
            command.callback = null;
            command.FinishCommand();
        }

        public virtual bool CanUseAction(Game.Faction actingPlayer, Card card) => true;
    }
}