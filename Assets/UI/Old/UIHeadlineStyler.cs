using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeadlineStyler : MonoBehaviour, ICardStyler
{
    public void Style(GameObject cardObject)
    {
        cardObject.transform.localPosition = Vector3.zero;
        cardObject.transform.localScale = Vector3.one;
        cardObject.transform.localRotation = Quaternion.identity;

        Destroy(cardObject.GetComponent<CanvasGroup>());
        Destroy(cardObject.GetComponent<UICardDisplay>());
    }
}
