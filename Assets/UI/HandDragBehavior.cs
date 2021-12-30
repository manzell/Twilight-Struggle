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
        UIManager _uiManager;

        public void OnDragStart(Transform t, PointerEventData eventData)
        {
            eventData.selectedObject = t.gameObject;
            _canvasGroup = t.GetComponent<CanvasGroup>();
            _canvasGroup.blocksRaycasts = false;
            _initY = t.localPosition.y;
        }
        
        public void OnDrag(Transform t, PointerEventData eventData)
        {
            _uiManager = FindObjectOfType<UIManager>();

            if (Game.currentPhase is TurnSystem.ActionRound)
            {
                if (t.localPosition.y - _initY > 35f && _arOpen == false)
                {
                    _uiManager.ShowARPanel(_card); // _card is never set
                    _arOpen = true;
                }
                else if (t.localPosition.y - _initY < 35f && _arOpen == true)
                {
                    _uiManager.HideARPanel();
                    _arOpen = false;
                }
            }

            float f = Mathf.Clamp((t.localPosition.y - 35f - _initY) / 80f + 1f, 1f, 1.75f);
            _handUI = FindObjectOfType<HandUI>();
            t.localScale = new Vector3(f, f, f);
            t.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f); // TODO - At a certain Y coordinate, remove us from the hand or mark it to exclude from the Hand calculation?
        }

        public void OnDragEnd(Transform t, PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            if (!eventData.selectedObject.transform.GetComponent<UIDropHandler>()) // If we dropped the card on nothing, close the AR panel if it's hanging out. 
                _uiManager.HideARPanel();

            _handUI.RefreshHand();
        }
    }
}
