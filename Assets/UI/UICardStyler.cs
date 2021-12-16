using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICardStyler : MonoBehaviour, ICardStyler
{
    public void Style(GameObject cardObject)
    {
        cardObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        cardObject.transform.position = Vector3.zero;
        cardObject.transform.localPosition = Vector3.zero;
        Destroy(cardObject.GetComponent<UICardDisplay>());
    }
}

public interface ICardStyler {
    public void Style(GameObject cardObject); 
}