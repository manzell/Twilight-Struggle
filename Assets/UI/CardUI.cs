using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;

namespace TwilightStruggle.UI
{
    public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] TextMeshProUGUI _cardTitle, _cardOps, _cardText;
        [SerializeField] IDragBehavior _dragBehavior;
        public Image highlight;
        public Card card;

        public void SetDragBehavior(IDragBehavior drag) => _dragBehavior = drag;

        public void SetCard(Card card)
        {
            this.card = card;
            _cardTitle.text = card.cardName;
            _cardText.text = card.cardText;

            if (card is not ScoringCard)
                _cardOps.text = card.opsValue.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_dragBehavior != null)
                _dragBehavior.OnDragStart(transform, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragBehavior != null)
                _dragBehavior.OnDrag(transform, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragBehavior != null)
                _dragBehavior.OnDragEnd(transform, eventData);
        }
    }
}