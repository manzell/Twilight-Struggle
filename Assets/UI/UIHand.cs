using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 
using DG.Tweening;
using Sirenix.OdinInspector;

namespace TwilightStruggle.UI
{
    public class UIHand : SerializedMonoBehaviour
    {
        [SerializeField] GameObject usPrefab, ussrPrefab, neutralPrefab, chinaPrefab, scoringPrefab;
        [SerializeField] Transform cardOrigin;
        public float cardOverlap;
        bool _canRefresh = true; // This locks out the Refresh Function to functionally once every few seconds or whatever to prevent overlap. 

        Game _game;
        Game.Faction _currentFaction = Game.Faction.USSR;
        [SerializeField] Dictionary<Transform, Card> _cards = new Dictionary<Transform, Card>();

        private void Awake()
        {
            _game = FindObjectOfType<Game>();
            Game.dealCardsEvent.AddListener(OnDealCards);
            Game.setActiveFactionEvent.AddListener(SetFaction);
        }

        private void Update()
        {
            // Toggle the Active Faction on Tab. 
            if (Input.GetKeyDown(KeyCode.Tab))
                Game.setActiveFactionEvent.Invoke(_currentFaction == Game.Faction.USSR ? Game.Faction.USA : Game.Faction.USSR);
        }

        public void SetFaction(Game.Faction faction)
        {
            if (_currentFaction != faction)
            {
                _currentFaction = faction;
                RefreshHand();
            }
        }

        [Button]
        public void RefreshHand()
        {
            if (!_canRefresh) return; //  yeah it's a shitty hack so what

            List<Card> _hand = _game.playerMap[_currentFaction].hand;
            List<Card> _toAdd = new List<Card>();
            List<Card> _toRemove = new List<Card>();

            _canRefresh = false;

            // Now we add any new cards that we have
            foreach (Card card in _hand)
                if (!_cards.ContainsValue(card))
                    _toAdd.Add(card);

            // Get rid of any loose cards and reorganize what we have
            foreach (Transform card in _cards.Keys)
            {
                if (_cards[card] == null || !_hand.Contains(_cards[card])) // avoid a potential issue where player contains a null card?
                    _toRemove.Add(_cards[card]);
                else
                {
                    card.DOLocalMove(new Vector3(-GetXaxisRight(card), 0f, 0f), 0.5f).SetEase(Ease.OutBack);
                    card.DOScale(1, .35f);
                }
            }

            if (_toRemove.Count > 0)
                RemoveCards(_toRemove);

            AddCards(_toAdd);
        }

        public void AddCards(List<Card> cards)
        {
            StartCoroutine(AddCardEveryTenth());

            IEnumerator AddCardEveryTenth()
            {
                foreach (Card card in cards)
                {
                    AddCard(card);
                    yield return new WaitForSeconds(0.1f);
                }

                yield return null; // try waiting 1 frame so that we can ensure that our _cards table is properly set. Otherwise wait .1 per card + .1

                // This saves our ordered list of cards back to the player's Hand. 
                IEnumerable<Card> orderedCards =
                    from _card in _cards.Keys
                    where _cards[_card] != null
                    orderby _card.localPosition.x descending
                    select _cards[_card];

                // OK this is causing weirdness because we're interrupting a different process and writing directly to the hand. 
                // So instead we'll only write to hand a filtered list so it doesn't necessarily matter what's happening with add/removes. 

                List<Card> newHand = new List<Card>();

                for (int i = 0; i < orderedCards.Count(); i++)
                    if (!newHand.Contains(orderedCards.ElementAt(i)))
                        newHand.Add(orderedCards.ElementAt(i));

                _game.playerMap[_currentFaction].hand = newHand; // actually unclear if this works/will work. 
                _canRefresh = true;
            }
        }

        public void AddCard(Card card)
        {
            GameObject _prefab = neutralPrefab;

            // Check if this card hasn't already been added by some other process!
            if (_cards.ContainsValue(card)) return;

            if (card is ScoringCard) // Because ScoringCard is not a faction we use branching-if instead of Switch :( 
                _prefab = scoringPrefab;
            else if (card.faction == Game.Faction.China)
                _prefab = chinaPrefab;
            else if (card.faction == Game.Faction.USA)
                _prefab = usPrefab;
            else if (card.faction == Game.Faction.USSR)
                _prefab = ussrPrefab;

            GameObject _card = Instantiate(_prefab, cardOrigin.transform.position, Quaternion.identity, transform);

            UICard uiCard = _card.GetComponent<UICard>();
            uiCard.SetCard(card);
            if (_currentFaction == Game.Faction.USSR)
                uiCard.highlight.color = Color.red;
            else if (_currentFaction == Game.Faction.USA)
                uiCard.highlight.color = Color.cyan;
            uiCard.highlight.SetAlpha(0.5f);

            _cards.Add(_card.transform, card);

            _card.transform.DOLocalMove(new Vector3(-GetXaxisLeft(_card.transform), 0f, 0f), 0.7f).SetEase(Ease.OutFlash);
            // TODO: OnComplete, save out the order of our cards back to the player's hand so that we retain our ordering. 
        }

        public void RemoveCards(List<Card> cards)
        {
            List<Transform> _toRemove = new List<Transform>();

            // Unset the Card from our list of cards immediately rather than every Nth-of-seconds; this ensures any new cards we're adding won't place themselves based on card's we're about to remove 
            foreach (Card card in cards)
            {
                foreach (Transform t in _cards.Keys.ToArray())
                    if (_cards[t] == card || _cards[t] == null)
                    {
                        _toRemove.Add(t);
                        _cards.Remove(t);
                    }
            }

            StartCoroutine(RemoveCardEveryTenth());

            IEnumerator RemoveCardEveryTenth()
            {
                while (_toRemove.Count > 0)
                {
                    RemoveCard(_toRemove[0]);
                    _toRemove.RemoveAt(0);
                    yield return new WaitForSeconds(0.08f);
                }
            }
        }

        public void RemoveCard(Card card)
        {
            if (_cards.ContainsValue(card))
                foreach (Transform t in _cards.Keys.ToArray())
                    if (_cards[t] == card)
                        _cards.Remove(t);
        }

        void RemoveCard(Transform card)
        {
            card.DOLocalMove(cardOrigin.transform.position, 0.5f).
                SetEase(Ease.Linear).
                OnComplete(() =>
                {
                    _cards.Remove(card);
                    Destroy(card.gameObject);
                });
        }

        void OnDealCards(Game.Faction faction, List<Card> cards)
        {
            if (faction == _currentFaction)
                AddCards(cards);
        }

        float GetXaxisRight(Transform card)
        {
            int index = -1;
            float cardWidth = card.GetComponent<RectTransform>().rect.width;

            // Trying out this cool Query-Body Expression Thingy!
            IEnumerable<Transform> orderedCards =
                from _card in _cards.Keys
                where _cards[_card] != null
                orderby _card.localPosition.x descending
                select _card;

            for (int i = 0; i < orderedCards.Count(); i++)
            {
                if (card == orderedCards.ElementAt(i))
                {
                    index = i;
                    orderedCards.ElementAt(i).SetSiblingIndex(i + 1);
                    break;
                }
            }

            return 0f + index * (cardWidth * (1 - cardOverlap));
        }

        float GetXaxisLeft(Transform card) // Too lazy to generalize the above function
        {
            int index = -1;
            float cardWidth = card.GetComponent<RectTransform>().rect.width;

            // Trying out this cool Query-Body Expression Thingy!
            IEnumerable<Transform> orderedCards =
                from _card in _cards.Keys
                where _cards[_card] != null
                orderby _card.localPosition.x
                select _card;

            for (int i = 0; i < orderedCards.Count(); i++)
            {
                if (card == orderedCards.ElementAt(i))
                {
                    index = i;
                    orderedCards.ElementAt(i).SetSiblingIndex(i + 1);
                    break;
                }
            }

            return 0f + index * (cardWidth * (1 - cardOverlap));
        }
    }
}