using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

namespace TwilightStruggle.UI
{
    public class UIHeadlineDropZone : MonoBehaviour, IDropHandler
    {
        public Game.Faction faction;
        [SerializeField] TextMeshProUGUI _revealedHeadline;

        //
        // Todo and just replace with a UIDropHandler using a separate Headline Game Action. 
        //  move the animations to animator like the other actions. 
        //

        public void OnDrop(PointerEventData eventData)
        {
            if (Game.currentTurn.headlinePhase.headlines[faction] == null &&
                Game.actingPlayer == faction)
            {
                Card _card = eventData.selectedObject.GetComponent<CardUI>().card;
                _revealedHeadline.text = _card.cardName;
                _revealedHeadline.DOFade(1, 0.35f);

                FindObjectOfType<HandUI>().RemoveCard(_card); // Note: This triggers BEFORE UICard.OnDragEnd()

                eventData.selectedObject.transform.parent = transform;
                eventData.selectedObject.transform.DOKill();
                eventData.selectedObject.transform.DOScale(0f, .35f).OnComplete(() => Destroy(eventData.selectedObject));

                //headlineAction.SetHeadline(faction, _card);
            }
        }
    }
}