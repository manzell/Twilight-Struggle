using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class UICardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] TextMeshProUGUI cardTitleText, influenceText;
    [SerializeField] Image cardBackground, influenceBackground;
    [SerializeField] Outline cardOutline; 
    public Card card;

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.selectedObject = this.gameObject; // using this explicitly
        CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    public void OnDrag(PointerEventData eventData) => transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0f);

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(GetComponent<CanvasGroup>());
        transform.localScale = Vector3.one;

        LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>()); // Shouldn't need this but Horizontal Layout Group doesn't seem to trigger after adjusting local position. 
    }

    [Button] public void SetCard(Card c)
    {
        card = c;

        cardTitleText.text = card.cardName;
        influenceText.text = card.OpsValue.ToString();

        if (card is not ScoringCard)
        {
            influenceBackground.gameObject.SetActive(true);
        }

        if (card is ScoringCard)
        {
            influenceBackground.gameObject.SetActive(false);
            cardBackground.color = Color.yellow;
        }
        else switch (card.faction)
        {
            case Game.Faction.Neutral:
                cardTitleText.color = new Color(.1f, .1f, .1f);
                cardBackground.color = new Color(.5f, .5f, .5f);
                influenceText.color = new Color(.1f, .1f, .1f);
                influenceBackground.color = new Color(.9f, .9f, .9f);
                //cardOutline.effectColor = new Color(.25f, .25f, .25f);
                break;
            case Game.Faction.USA:
                cardTitleText.color = Color.white;
                cardBackground.color = Color.blue;
                influenceText.color = Color.blue;
                influenceBackground.color = Color.white;
                //cardOutline.effectColor = new Color(.25f, .25f, .25f);
                break;
            case Game.Faction.USSR:
                cardTitleText.color = Color.white;
                cardBackground.color = Color.red;
                influenceText.color = Color.black;
                influenceBackground.color = Color.yellow;
                //cardOutline.effectColor = Color.red;
                break;
            case Game.Faction.China:
                cardTitleText.color = Color.yellow;
                cardBackground.color = new Color(1f, .9f, .1f);
                influenceText.color = Color.white;
                influenceBackground.color = Color.yellow;
                //cardOutline.effectColor = new Color(1f, .75f, 0f);
                break;
        }
    }
}
