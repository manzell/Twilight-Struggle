using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro; 

public class UIHeadlineDropZone : MonoBehaviour, IDropHandler
{
    public Game.Faction faction;
    public HeadlineAction headlineAction;
    [SerializeField] GameObject _cardSlot;
    [SerializeField] TextMeshProUGUI _revealedHeadline; 

    public void OnDrop(PointerEventData eventData)
    {
        if (Game.currentTurn.headlinePhase.headlines[faction] == null &&
            Game.actingPlayer == faction)
        {
            Card _card = eventData.selectedObject.GetComponent<UICard>().card;
            _revealedHeadline.text = _card.cardName; 
            _revealedHeadline.DOFade(1, 0.35f);

            FindObjectOfType<UIHand>().RemoveCard(_card); // Note: This triggers BEFORE UICard.OnDragEnd()

            eventData.selectedObject.transform.parent = transform;
            eventData.selectedObject.transform.DOKill();
            eventData.selectedObject.transform.DOScale(0f, .35f).OnComplete(() => Destroy(eventData.selectedObject)); 

            headlineAction.SetHeadline(faction, _card);
        }
    }
}
