using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; 
using UnityEngine.UI;

public class StartGameAnimation : MonoBehaviour
{
    [SerializeField] RectTransform buttonArea, messagePanel;
    [SerializeField] TextMeshProUGUI headline;
    [SerializeField] Image namePlate;

    public void Animate()
    {
        // Overall Panel Size Adjustment
        Vector2 messageDelta = messagePanel.sizeDelta;
        messageDelta.y = 75f;
        messagePanel.DOSizeDelta(messageDelta, 1.5f);

        // Remove our NamePlate image
        namePlate.DOFade(0, 1f).OnComplete( () => Destroy(namePlate.gameObject));

        // The Button Panel - Fade it out. It should move as the height of the panel is adjusted
        StartCoroutine(FadeOutCanvas(buttonArea, 1.5f)); 
        IEnumerator FadeOutCanvas(RectTransform canvas, float time)
        {
            float timer = 0f;

            if (canvas.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
            {
                while(timer <= time)
                {
                    canvasGroup.alpha = 1 - Mathf.Clamp(timer / time, 0, 1);
                    timer += Time.deltaTime; 
                    yield return null;
                }

                canvasGroup.alpha = 0f; 
            }
        }

        // Headline - Fade it in
        headline.DOFade(1, .5f).SetDelay(1.5f);        
    }
}
