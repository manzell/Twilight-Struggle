using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICardDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField] GameAction gameAction; 
    [SerializeField] GameObject cardSlot;
    [SerializeField] ICardStyler cardStyler; 

    public void OnDrop(PointerEventData eventData)
    {
        GameObject cardObject = Instantiate(eventData.selectedObject, cardSlot.transform);
        gameAction.SetCard(cardObject.GetComponent<UICardDisplay>().card);
        
        cardStyler.Style(cardObject);
    }
}
