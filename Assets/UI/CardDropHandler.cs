using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI; 
namespace TwilightStruggle.UI
{
    public class CardDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public bool hidden = true;
        public Game.Faction faction;
        [SerializeField] Color baseColor, highlightColor; 
        [SerializeField] Image panelImage;  
        [HideInInspector] public UnityEvent<Card> showEvent = new UnityEvent<Card>();
        [HideInInspector] public UnityEvent<Card> cardDropEvent = new UnityEvent<Card>();

        public void OnDrop(PointerEventData eventData)
        {
            Card card = eventData.selectedObject.GetComponent<CardUI>().card;

            FindObjectOfType<Game>().playerMap[Game.actingPlayer].hand.Remove(card); // Always remove the card on drop; return it back if we need to

            if(Game.currentPhase is TurnSystem.HeadlinePhase && Game.actingPlayer != faction) return; 
            if(Game.currentPhase is TurnSystem.ActionRound && Game.phasingPlayer != Game.actingPlayer) return;

            FindObjectOfType<HandUI>()?.RemoveCard(card); // Note: This triggers BEFORE UICard.OnDragEnd() 
            eventData.selectedObject.transform.parent = transform;
            eventData.selectedObject.transform.DOKill();
            eventData.selectedObject.transform.DOScale(0f, .35f).OnComplete(() => Destroy(eventData.selectedObject));

            float f = 0f;
            foreach (Transform t in transform.parent)
            {
                if (t != this.transform)
                    t.GetComponent<CardDropHandler>()?.Hide(f += 0.035f);
            }

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
        public void Show(Card card, float f = 0f)
        {
            if (hidden == true)
            {
                showEvent.Invoke(card); 
                hidden = false;
                transform.DOKill();
                transform.DOLocalMoveY(-350f, .1f).SetEase(Ease.InOutSine).SetDelay(f);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            panelImage.color = highlightColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            panelImage.color = baseColor;
        }
    }
}