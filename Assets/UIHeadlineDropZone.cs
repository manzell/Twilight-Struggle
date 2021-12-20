using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHeadlineDropZone : MonoBehaviour, IDropHandler
{
    public HeadlineAction headlineAction;
    [SerializeField] GameObject cardSlot;

    public Game.Faction faction;

    public void OnDrop(PointerEventData eventData)
    {
        if(Game.currentTurn.headlinePhase.headlines[faction] == null &&
            FindObjectOfType<UIManager>().currentFaction == faction)
        {
            GameObject cardObject = Instantiate(eventData.selectedObject, cardSlot.transform);
            Card card = eventData.selectedObject.GetComponent<UICardDisplay>().card;

            GetComponent<ICardStyler>()?.Style(cardObject);
            headlineAction.SetHeadline(faction, card);
            FindObjectOfType<UIHandManager>().RefreshHandDisplay(); 
        }
    }
}
