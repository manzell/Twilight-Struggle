using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHeadlineDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] HeadlineAction headlineAction;
    [SerializeField] GameObject cardSlot;
    [SerializeField] ICardStyler cardStyler;

    public Game.Faction faction;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject cardObject = Instantiate(eventData.selectedObject, cardSlot.transform);
        headlineAction.SetHeadline(faction, cardObject.GetComponent<UICardDisplay>().card);

        cardStyler.Style(cardObject);
    }


}
