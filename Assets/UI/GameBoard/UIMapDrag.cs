using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening; 

public class UIMapDrag : MonoBehaviour, IDragHandler
{
    [SerializeField] RectTransform _viewport, _map; 
    [SerializeField] float 
        _dragSpeed = 1.25f, 
        _zoomSpeed = 1f;

    RectTransform _thisRect; 

    void Awake()
    {
        _thisRect = GetComponent<RectTransform>(); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3[] _mapCorners = new Vector3[4];
        Vector3[] _viewportCorners = new Vector3[4];

        _viewport.GetWorldCorners(_viewportCorners);
        _map.GetWorldCorners(_mapCorners);

        // Look ahead and see if we'll be out of the viewport and break out. 
        if (_viewportCorners[0].x < _mapCorners[0].x + eventData.delta.x) return;
        if (_viewportCorners[0].y < _mapCorners[0].y + eventData.delta.y) return;
        if (_viewportCorners[2].x > _mapCorners[2].x + eventData.delta.x) return;
        if (_viewportCorners[2].y > _mapCorners[2].y + eventData.delta.y) return;

        _thisRect.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f) * _dragSpeed;
    }

    public void OnMouseOver()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float newScale = _thisRect.localScale.x + _zoomSpeed * Input.mouseScrollDelta.y;

            if (newScale > 0.5f && Input.mouseScrollDelta.y < 0 || newScale < 2f && Input.mouseScrollDelta.y > 0)
                _thisRect.DOScale(newScale, 0.1f);
        }
    }
}
