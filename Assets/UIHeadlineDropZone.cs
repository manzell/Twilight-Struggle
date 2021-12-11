using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHeadlineDropZone : MonoBehaviour, IDropHandler
{    
    [SerializeField] Game.Faction faction;
    [SerializeField] GameObject cardSlot; 

    public void OnDrop(PointerEventData eventData)
    {
        eventData.selectedObject.transform.SetParent(cardSlot.transform);

        eventData.selectedObject.transform.localPosition = Vector3.zero;
        eventData.selectedObject.transform.localScale = Vector3.one;
        eventData.selectedObject.transform.localRotation = Quaternion.identity;

        Debug.Log($"Headline Dropped {eventData.selectedObject.GetComponent<UICardDisplay>().card}");

        Game.currentTurn.GetComponentInChildren<Headline>().SubmitHeadline(faction, eventData.selectedObject.GetComponent<UICardDisplay>().card);

        Destroy(eventData.selectedObject.GetComponent<CanvasGroup>());
        Destroy(eventData.selectedObject.GetComponent<UICardDisplay>());
    }
}
