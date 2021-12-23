using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMapDrag : MonoBehaviour, IDragHandler
{
    [SerializeField] GameObject _mapDisplayArea; 
    [SerializeField] float 
        _dragSpeed = 1.25f, 
        _zoomSpeed = 1f;

    float _ratio; 

    public void OnDrag(PointerEventData eventData)
    {
        // TODO: Make sure we can't drag past our boundaries.

        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f) * _dragSpeed;
    }

    public void Awake()
    {
        _ratio = (float)GetComponent<RectTransform>().sizeDelta.x / (float)GetComponent<RectTransform>().sizeDelta.y;
    }

    public void Update()
    {
        float delta = Input.mouseScrollDelta.y * _zoomSpeed;
        Vector2 size = GetComponent<RectTransform>().sizeDelta; 
        Vector2 displaySize = _mapDisplayArea.GetComponent<RectTransform>().sizeDelta;

        float scaleModifier = (size.x + delta) / size.x;

        if(Input.mouseScrollDelta.y != 0)
        {
            if (size.x + delta > displaySize.x && (size.x + delta) * _ratio > displaySize.y && (size.x + delta) / 2.5f < displaySize.x)
                GetComponent<RectTransform>().localScale += new Vector3(scaleModifier, scaleModifier, scaleModifier);

        }
    }
}
