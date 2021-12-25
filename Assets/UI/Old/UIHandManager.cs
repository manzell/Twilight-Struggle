using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.Linq; 

public class UIHandManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    Dictionary<Card, GameObject> cardsDisplayed = new Dictionary<Card, GameObject>();
    Game.Faction faction; 

    private void Awake()
    {
        Game.phaseStartEvent.AddListener(OnActionRoundStart);
        //Game.setActiveFactionEvent.AddListener(SetFaction); 
    }

    void OnActionRoundStart(Phase phase)
    {
        if (phase is ActionRound)
        {
            ActionRound actionRound = (ActionRound)phase;
            SetFaction(actionRound.phasingPlayer);
        }
    }

    public void SetFaction(Game.Faction faction)
    {
        Player player = FindObjectOfType<Game>().playerMap[faction];
        GetComponent<Image>().color = player.faction == Game.Faction.USA ? new Color(.2f, .2f, .5f) : new Color(.5f, .2f, .2f);
        this.faction = faction;

        RefreshHandDisplay(); 
    }

    public void RefreshHandDisplay()
    {
        List<Card> cards = new List<Card>();
        Player player = FindObjectOfType<Game>().playerMap[faction]; 

        // Firstly, we look through our list of instantiated prefabs and remove any that aren't still in our hand. 
        foreach (Card card in cardsDisplayed.Keys)
            cards.Add(card);

        foreach (Card card in cards)
        {
            if (!player.hand.Contains(card))
            {
                Destroy(cardsDisplayed[card].gameObject);
                cardsDisplayed.Remove(card);
            }
        }

        // Then we go through our list and instantiate any of the cards that are not present
        foreach (Card card in player.hand)
        {
            if (!cardsDisplayed.ContainsKey(card))
            {
                GameObject cardObject = Instantiate(cardPrefab, transform);
                cardObject.GetComponent<UICardDisplay>().SetCard(card);
                cardsDisplayed.Add(card, cardObject);
            }
        }
    }
}