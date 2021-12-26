using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using UnityEngine.EventSystems;
using DG.Tweening;

namespace TwilightStruggle.UI
{

    public class UICardDropHandler : MonoBehaviour, IDropHandler
    {
        public UnityEvent<Card> dropEvent = new UnityEvent<Card>();
        public bool hidden = true;

        public void OnDrop(PointerEventData eventData)
        {
            Card card = eventData.selectedObject.GetComponent<UICard>().card;

            FindObjectOfType<UIHand>().RemoveCard(card); // Note: This triggers BEFORE UICard.OnDragEnd()
            eventData.selectedObject.transform.parent = transform;
            eventData.selectedObject.transform.DOKill();
            eventData.selectedObject.transform.DOScale(0f, .35f).OnComplete(() => Destroy(eventData.selectedObject));

            float f = 0f;
            foreach (Transform t in transform.parent)
            {
                if (t.GetComponent<UICardDropHandler>() != this)
                {
                    t.GetComponent<UICardDropHandler>().Hide(f += 0.035f);
                }
            }

            dropEvent.Invoke(card);
            GetComponent<GameAction>().SetCard(card); // Set Card should take care of pulling the card out of the player's hand. 
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