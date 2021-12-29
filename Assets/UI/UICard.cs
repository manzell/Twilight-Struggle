using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;

namespace TwilightStruggle.UI
{
    public class UICard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] TextMeshProUGUI _cardTitle, _cardOps, _cardText;
        [SerializeField] IDragBehavior _dragBehavior;
        public Image highlight;
        public Card card;
        CanvasGroup _canvasGroup;
        HandUI _uiHand;
        float initialY;

        static bool _arOpen = false; // keeps track of if the actionRoundPanel is open. TODO Move this to the UImanager

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _uiHand = FindObjectOfType<HandUI>();
        }

        public void SetDragBehavior(IDragBehavior drag) => _dragBehavior = drag;

        public void SetCard(Card card)
        {
            this.card = card;
            _cardTitle.text = card.cardName;
            _cardText.text = card.cardText;

            if (card is not ScoringCard)
                _cardOps.text = card.OpsValue.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_dragBehavior != null)
                _dragBehavior.OnDragStart(eventData);
            else
            {
                eventData.selectedObject = this.gameObject; // using this. explicitly
                _canvasGroup.blocksRaycasts = false;
                initialY = transform.localPosition.y;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragBehavior != null)
                _dragBehavior.OnDrag(eventData);
            else
            {
                if (Game.currentPhase is TurnSystem.ActionRound)
                {
                    if (transform.localPosition.y - initialY > 35f && _arOpen == false)
                    {
                        FindObjectOfType<UIManager>().ShowARPanel(card);
                        _arOpen = true;
                    }
                    else if (transform.localPosition.y - initialY < 35f && _arOpen == true)
                    {
                        FindObjectOfType<UIManager>().HideARPanel();
                        _arOpen = false;
                    }
                }

                float f = Mathf.Clamp((transform.localPosition.y - 35f - initialY) / 80f + 1f, 1f, 1.75f);

                transform.localScale = new Vector3(f, f, f);
                transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f); // TODO - At a certain Y coordinate, remove us from the hand or mark it to exclude from the Hand calculation? 
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragBehavior != null)
                _dragBehavior.OnDragEnd(eventData);
            else
            {
                _canvasGroup.blocksRaycasts = true;

                if (!eventData.selectedObject.transform.TryGetComponent<UIDropHandler>(out var x)) // If we dropped the card on nothing, close the AR panel if it's hanging out. 
                    FindObjectOfType<UIManager>().HideARPanel();

                _uiHand.RefreshHand();
            }
        }
    }
}