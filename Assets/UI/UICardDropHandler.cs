using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICardDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField] GameAction gameAction; 
    [SerializeField] GameObject cardSlot;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject cardObject = Instantiate(eventData.selectedObject, cardSlot.transform);
        gameAction.SetCard(cardObject.GetComponent<UICardDisplay>().card);
        
        GetComponent<UICardStyler>()?.Style(cardObject);
    }
}
