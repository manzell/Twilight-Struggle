using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 
using DG.Tweening;
using Sirenix.OdinInspector; 

public class UIHand : SerializedMonoBehaviour
{
    [SerializeField] GameObject usPrefab, ussrPrefab, neutralPrefab, chinaPrefab, scoringPrefab;
    [SerializeField] Transform cardOrigin; 
    public int cardOverlap; 

    Game _game;
    Game.Faction _currentFaction = Game.Faction.USSR; 
    [SerializeField] Dictionary<Card, GameObject> _cards = new Dictionary<Card, GameObject>();

    private void Awake()
    {
        _game = FindObjectOfType<Game>();
        Game.phaseStartEvent.AddListener(OnActionRoundStart);
        Game.dealCardsEvent.AddListener(OnDealCards); 
    }

    public void SetFaction(Game.Faction faction)
    {
        // If we're changing factions, remove all cards. 
        if(_currentFaction != faction)
        {
            _currentFaction = faction;

            foreach (Card card in _cards.Keys)
                RemoveCard(card);
        }

        RefreshHand(); 
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            RemoveCards(_cards.Keys.ToList()); 
        }
    }

    public void RefreshHand()
    {
        List<Card> _hand = _game.playerMap[_currentFaction].hand;
        List<Card> _toAdd = new List<Card>();
        List<Card> _toRemove = new List<Card>(); 

        // Now we add any new cards that we have
        foreach (Card card in _hand)
            if (!_cards.ContainsKey(card))
                _toAdd.Add(card);

        AddCards(_toAdd); 

        // Get rid of any loose cards and reorganize what we have
        foreach (Card card in _cards.Keys)
        {
            if (!_hand.Contains(card))
                _toRemove.Remove(card);
            else
                _cards[card].transform.DOLocalMove(new Vector3(-GetXaxisRight(_cards[card]), 0f, 0f), 0.5f).SetEase(Ease.OutBack);
        }

        RemoveCards(_toRemove); 
    }

    public void AddCards(List<Card> cards)
    {
        StartCoroutine(AddCardEveryTenth()); 

        IEnumerator AddCardEveryTenth()
        {
            while(cards.Count > 0)
            {
                Card card = cards[0]; 
                AddCard(card);
                cards.Remove(card); 
                yield return new WaitForSeconds(0.1f); 
            }
        }
    }

    // TODO: What happens when we try to AddCard/s from the wrong faction?
    // Answer - nothing, that's an error in the code elsewhere. We should only be adding cards when we know who the right faction is. 

    public void AddCard(Card card)
    {
        GameObject _prefab = neutralPrefab;

        if (card is ScoringCard) // Because ScoringCard is not a faction we use branching-if instead of Switch. 
            _prefab = scoringPrefab;
        else if (card.faction == Game.Faction.China)
            _prefab = chinaPrefab;
        else if (card.faction == Game.Faction.USA)
            _prefab = usPrefab;
        else if (card.faction == Game.Faction.USSR)
            _prefab = ussrPrefab;

        GameObject _card = Instantiate(_prefab, cardOrigin.transform.position, Quaternion.identity, transform);
        _card.GetComponent<UICard>().SetCard(card);
        _cards.Add(card, _card);

        _card.transform.DOLocalMove(new Vector3(-GetXaxisLeft(_card), 0f, 0f), 0.7f);  
    }

    public void RemoveCards(List<Card> cards)
    {
        StartCoroutine(RemoveCardEveryTenth()); 

        IEnumerator RemoveCardEveryTenth()
        {
            while(cards.Count > 0)
            {
                RemoveCard(cards[0]);
                cards.RemoveAt(0);
                yield return new WaitForSeconds(0.1f); 
            }
        }
    }

    public void RemoveCard(Card card)
    {
        if(_cards.ContainsKey(card))
        {
            _cards[card].transform.
                DOLocalMove(cardOrigin.transform.position, 0.5f).
                SetEase(Ease.Linear).
                OnComplete(() => { Destroy(_cards[card]); });

            _cards.Remove(card);
        }
    }

    void OnActionRoundStart(Phase phase)
    {
        if (phase is ActionRound)
        {
            ActionRound actionRound = (ActionRound)phase;
            SetFaction(actionRound.phasingPlayer);
        }
    }

    void OnDealCards(Game.Faction faction, List<Card> cards)
    {
        if (faction == _currentFaction)
            AddCards(cards);
    }

    float CalcYAxis(Card card) // Given card X, we determine what percentage of the way along with X axis we are, then apply a Y axis adjustment. 
    {
        return 0f; 
    }

    float GetXaxisRight(GameObject card)
    {
        int index = -1; 
        float cardWidth = card.GetComponent<RectTransform>().rect.width;

        // Trying out this cool Query-Body Expression Thingy!
        IEnumerable<GameObject> orderedCards = 
            from _card in _cards.Values.ToList()
            orderby _card.transform.localPosition.x descending
            select _card;

        for(int i = 0; i < orderedCards.Count(); i++)
        {
            if(card == orderedCards.ElementAt(i))
            {
                index = i;
                orderedCards.ElementAt(i).transform.SetSiblingIndex(i + 1);
                break;
            }
        }

        // our first card should always be X = 0. For each card after that, it's x + cardWidth - overlap
        return 0f + index * (cardWidth - cardOverlap);
    }

    float GetXaxisLeft(GameObject card)
    {
        int index = -1;
        float cardWidth = card.GetComponent<RectTransform>().rect.width;

        // Trying out this cool Query-Body Expression Thingy!
        IEnumerable<GameObject> orderedCards =
            from _card in _cards.Values.ToList()
            orderby _card.transform.localPosition.x 
            select _card;

        for (int i = 0; i < orderedCards.Count(); i++)
        {
            if (card == orderedCards.ElementAt(i))
            {
                index = i;
                orderedCards.ElementAt(i).transform.SetSiblingIndex(i + 1); 
                break;
            }
        }

        // our first card should always be X = 0. For each card after that, it's x + cardWidth - overlap
        return 0f + index * (cardWidth - cardOverlap);
    }
}