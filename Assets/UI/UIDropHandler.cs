using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using UnityEngine.EventSystems;
using DG.Tweening;

namespace TwilightStruggle.UI
{
    public class UIDropHandler : MonoBehaviour, IDropHandler
    {
        public bool hidden = true;
        public Game.Faction faction; 
        [HideInInspector] public UnityEvent<Card> cardDropEvent = new UnityEvent<Card>();

        public void OnDrop(PointerEventData eventData)
        {
            if (faction != Game.Faction.Neutral && Game.actingPlayer != faction) return; 

            // TODO Send this out to an Animation Script. 
            Card card = eventData.selectedObject.GetComponent<CardUI>().card;

            FindObjectOfType<HandUI>()?.RemoveCard(card); // Note: This triggers BEFORE UICard.OnDragEnd() // TODO: We may be dropping cards from places other than the hand. Make sure all draggable cards have something
            eventData.selectedObject.transform.parent = transform;
            eventData.selectedObject.transform.DOKill();
            eventData.selectedObject.transform.DOScale(0f, .35f).OnComplete(() => Destroy(eventData.selectedObject));

            float f = 0f;
            foreach (Transform t in transform.parent)
                if (t.GetComponent<UIDropHandler>() != this)
                    t.GetComponent<UIDropHandler>().Hide(f += 0.035f);

            cardDropEvent.Invoke(card);
        }

        public void Hide(float f = 0f)
        {
            if (hidden == false)
            {
                hidden = true;
                transform.DOKill();
                transform.DOLocalMoveY(0f, .25f).SetDelay(f);
            }
        }
        public void Show(float f = 0f)
        {
            if (hidden == true)
            {
                hidden = false;
                transform.DOKill();
                transform.DOLocalMoveY(-300f, .1f).SetEase(Ease.InOutSine).SetDelay(f);
            }
        }
    }
}