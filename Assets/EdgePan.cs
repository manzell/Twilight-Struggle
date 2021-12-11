using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EdgePan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int edgeBuffer = 100;
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
            if (Input.mousePosition.x > defaultScreen.x - edgeBuffer || Input.mousePosition.x < edgeBuffer)
            {
                float xmove = (Input.mousePosition.x > defaultScreen.x / 2 ? -1 : 1) * speed;

                if ((xmove < 0 && transform.position.x - 1 >= defaultScreen.x - (GetComponent<RectTransform>().rect.width / 2)) || (xmove > 0 && transform.position.x + 1 <= (GetComponent<RectTransform>().rect.width / 2)))
                    transform.position = new Vector2(transform.position.x + xmove, transform.position.y);                
            }

            // Pan Up and Down
            if (Input.mousePosition.y > defaultScreen.y - edgeBuffer || Input.mousePosition.y < edgeBuffer)
            {
                float ymove = (Input.mousePosition.y > defaultScreen.y / 2 ? -1 : 1) * speed;

                if ((ymove < 0 && transform.position.y - 1 >= defaultScreen.y - (GetComponent<RectTransform>().rect.height / 2)) || (ymove > 0 && transform.position.y + 1 <= (GetComponent<RectTransform>().rect.height / 2)))
                    transform.position = new Vector2(transform.position.x, transform.position.y + ymove);
            }
        }
    }
}