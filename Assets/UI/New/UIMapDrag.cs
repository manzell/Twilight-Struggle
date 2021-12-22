using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMapDrag : MonoBehaviour, IDragHandler
{
    [SerializeField] GameObject mapDisplayArea; 
    [SerializeField] float dragSpeed = 1.25f, 
        zoomSpeed = 1f;

    float _ratio; 

    public void OnDrag(PointerEventData eventData)
    {
        // TODO: Make sure we can't drag past our boundaries.

        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f) * dragSpeed;
    }

    public void Awake()
    {
        _ratio = (float)GetComponent<RectTransform>().sizeDelta.x / (float)GetComponent<RectTransform>().sizeDelta.y;
    }

    public void Update()
    {
        Vector2 delta = new Vector2(Input.mouseScrollDelta.y, Input.mouseScrollDelta.y) * zoomSpeed;
        Vector2 size = GetComponent<RectTransform>().sizeDelta; 
        Vector2 displaySize = mapDisplayArea.GetComponent<RectTransform>().sizeDelta;

        if (size.x + delta.x > displaySize.x && (size.x + delta.x) * _ratio > displaySize.y && (size.x + delta.x) / 2.5f < displaySize.x)
            GetComponent<RectTransform>().sizeDelta *= (size.x + delta.x) / size.x; 
    }
}
