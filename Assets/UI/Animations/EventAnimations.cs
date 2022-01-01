using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle.UI
{
    public class EventAnimations : Animations
    {
        [SerializeField] CardEvent eventAction;

        private void Awake()
        {
            eventAction.targetEvent.AddListener(OnTarget); 
        }

        void OnTarget(GameCommand command)
        {
            FindObjectOfType<UIMessage>().Message($"{command.faction} plays {command.card.cardName}");
        }
    }
}
