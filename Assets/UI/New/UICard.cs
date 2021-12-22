using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;
using DG.Tweening; 

public class UICard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] TextMeshProUGUI _cardTitle, _cardOps, _cardText;
    [SerializeField] Image _cardImage;
    public Card card;
    CanvasGroup _canvasGroup;
    UIHand _uiHand;
    float initialY; 

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _uiHand = FindObjectOfType<UIHand>();
    }

    public void SetCard(Card card)
    {
        this.card = card; 
        _cardTitle.text = card.cardName;
        _cardText.text = card.cardText;

        if(card is not ScoringCard)
            _cardOps.text = card.OpsValue.ToString();       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.selectedObject = this.gameObject; // using this. explicitly
        _canvasGroup.blocksRaycasts = false;
        initialY = transform.localPosition.y; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f); // TODO - At a certain Y coordinate, remove us from the hand? 

        float f = Mathf.Clamp((transform.localPosition.y - 35f - initialY) / 80f + 1f, 1f, 1.75f); 
        transform.localScale = new Vector3(f, f, f);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // There may be a race condition here regarding if the DragDropHandler gets called first or not. In any case, if we do not 
        _canvasGroup.blocksRaycasts = true;
        _uiHand.RefreshHand();
        transform.DOScale(1f, 0.25f); 
    }
}