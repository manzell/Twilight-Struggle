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

        public void OnDrag(PointerEventData eventData)
        {
            if (Game.currentPhase is TurnSystem.ActionRound)
            {
                if (transform.localPosition.y - _initY > 35f && _arOpen == false)
                {
                    FindObjectOfType<UIManager>().ShowARPanel(_card);
                    _arOpen = true;
                }
                else if (transform.localPosition.y - _initY < 35f && _arOpen == true)
                {
                    FindObjectOfType<UIManager>().HideARPanel();
                    _arOpen = false;
                }
            }

            float f = Mathf.Clamp((transform.localPosition.y - 35f - _initY) / 80f + 1f, 1f, 1.75f);

            transform.localScale = new Vector3(f, f, f);
            transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f); // TODO - At a certain Y coordinate, remove us from the hand or mark it to exclude from the Hand calculation?
        }

        public void OnDragEnd(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnDragStart(PointerEventData eventData)
        {
            eventData.selectedObject = gameObject; // using this. explicitly
            if(TryGetComponent(out CanvasGroup _cg)) 
                _cg.blocksRaycasts = false;
            _initY = transform.localPosition.y;
        }
    }
}
