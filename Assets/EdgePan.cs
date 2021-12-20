using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector; 

public class EdgePan : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public (int top, int bottom, int left, int right) buffers; 

    public float speed = 1f; 

    Vector2 defaultScreen = new Vector2(1920, 1080);
    bool hover = false;

    public void OnPointerEnter(PointerEventData eventData) => hover = true;

    public void OnPointerExit(PointerEventData eventData) => hover = false; 

    void Update()
    {
        if(hover)
        {
            // Pan Left and Right
            if (Input.mousePosition.x > defaultScreen.x - buffers.right || Input.mousePosition.x < buffers.left)
            {
                float xPanSpeed = (Input.mousePosition.x > defaultScreen.x / 2 ? -1 : 1) * speed;
                float width = GetComponent<RectTransform>().rect.width / 2;

                if ((xPanSpeed < 0 && transform.position.x - 1 >= defaultScreen.x - width) || (xPanSpeed > 0 && transform.position.x + 1 <= width))
                    transform.position = new Vector2(transform.position.x + xPanSpeed, transform.position.y);                
            }

            // Pan Up and Down
            if (Input.mousePosition.y > defaultScreen.y - buffers.top || Input.mousePosition.y < buffers.bottom)
            {
                float yPanSpeed = (Input.mousePosition.y > defaultScreen.y / 2 ? -1 : 1) * speed;
                float height = GetComponent<RectTransform>().rect.height / 2;

                if ((yPanSpeed < 0 && transform.position.y - 1 >= defaultScreen.y - height) || (yPanSpeed > 0 && transform.position.y + 1 <= height))
                    transform.position = new Vector2(transform.position.x, transform.position.y + yPanSpeed);
            }
        }
    }
}