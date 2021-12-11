using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class UICardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] TextMeshProUGUI cardTitle, cardInfluence;
    public Card card;

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.selectedObject = this.gameObject;
        gameObject.AddComponent<CanvasGroup>().alpha = 0.5f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    public void OnDrag(PointerEventData eventData) => transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f);

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(GetComponent<CanvasGroup>());
        transform.localScale = Vector3.one;

        LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
    }

    [Button] public void Setup(Card c)
    {
        card = c;

        cardTitle.text = card.cardName;
        cardInfluence.text = card.opsValue.ToString(); 

        switch(card.faction)
        {
            case Game.Faction.Neutral:
                GetComponent<Outline>().effectColor = new Color(.8f, .8f, .8f); 
                cardInfluence.transform.parent.GetComponent<Image>().color = new Color(.8f, .8f, .8f);
                transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().color = Color.black; 
                break;
            case Game.Faction.USA:
                GetComponent<Outline>().effectColor = Color.blue;
                cardInfluence.transform.parent.GetComponent<Image>().color = Color.blue;
                transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                break;
            case Game.Faction.USSR:
                GetComponent<Outline>().effectColor = Color.red;
                cardInfluence.transform.parent.GetComponent<Image>().color = Color.red;
                transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                break;
            case Game.Faction.China:
                GetComponent<Outline>().effectColor = new Color(1f, .7f, .0f);
                cardInfluence.transform.parent.GetComponent<Image>().color = new Color(1f, .7f, .0f);
                transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                break;
        }
    }
}
