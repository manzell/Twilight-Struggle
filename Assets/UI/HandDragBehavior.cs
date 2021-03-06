using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TwilightStruggle.UI
{
    public class HandDragBehavior : MonoBehaviour, IDragBehavior
    {
        float _initY;
        Card _card; 
        bool _arOpen = false;
        CanvasGroup _canvasGroup;
        HandUI _handUI;
        UIActionRound _uiActionRound; 

        public void OnDragStart(Transform t, PointerEventData eventData)
        {
            if (Game.currentPhase is TurnSystem.ActionRound && Game.actingPlayer != Game.phasingPlayer)
            {
                eventData.pointerDrag = null;
                return;
            }

            eventData.selectedObject = t.gameObject;
            _canvasGroup = t.GetComponent<CanvasGroup>();
            _canvasGroup.blocksRaycasts = false;
            _initY = t.localPosition.y;
        }
        
        public void OnDrag(Transform t, PointerEventData eventData)
        {
            float f = Mathf.Clamp((t.localPosition.y - 35f - _initY) / 80f + 1f, 1f, 1.75f);
            _uiActionRound = FindObjectOfType<UIActionRound>();
            _card = t.GetComponent<CardUI>().card;
            _handUI = FindObjectOfType<HandUI>();

            t.localScale = new Vector3(f, f, f);
            t.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f);

            if (Game.currentPhase is TurnSystem.ActionRound)
            {
                if (t.localPosition.y - _initY > 35f && _arOpen == false)
                {
                    _arOpen = true;
                    _uiActionRound.ShowARPanel(_card);
                }
                else if (t.localPosition.y - _initY < 35f && _arOpen == true)
                {
                    _arOpen = false;
                    _uiActionRound.HideARPanel();
                }
            }
        }

        public void OnDragEnd(Transform t, PointerEventData eventData)
        {
            if (_handUI.HasCard(_card))
                _uiActionRound.HideARPanel();

            _canvasGroup.blocksRaycasts = true;
            _handUI.RefreshHand();
        }
    }
}
