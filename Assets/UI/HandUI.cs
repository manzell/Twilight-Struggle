using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 
using DG.Tweening;
using Sirenix.OdinInspector;

namespace TwilightStruggle.UI
{
    public class HandUI : SerializedMonoBehaviour
    {
        [SerializeField] GameObject usPrefab, ussrPrefab, neutralPrefab, chinaPrefab, scoringPrefab;
        [SerializeField] IDragBehavior _dragBehavior;
        [SerializeField] Transform cardOrigin;
        public float cardOverlap;
        bool _canRefresh = true; // This locks out the Refresh Function to functionally once every few seconds or whatever to prevent overlap. 

        Game _game;
        [SerializeField] Dictionary<Transform, Card> _displayedCards = new Dictionary<Transform, Card>();

        private void Awake()
        {
            _game = FindObjectOfType<Game>();
            Game.dealCardsEvent.AddListener(OnDealCards);
            Game.setActingFactionEvent.AddListener(OnSetFaction);
        }

        private void Update()
        {
            // Toggle the Active Faction on Tab. 
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Game.SetActingFaction(Game.actingPlayer == Game.Faction.USSR ? Game.Faction.USA : Game.Faction.USSR);

            }
        }

        public void OnSetFaction(Game.Faction faction) => RefreshHand();

        public bool HasCard(Card card) => _displayedCards.ContainsValue(card); 

        [Button]
        public void RefreshHand()
        {
            if (!_canRefresh) return; //  yeah it's a shitty hack so what

            List<Card> _hand = _game.playerMap[Game.actingPlayer].hand;
            List<Card> _toAdd = new List<Card>();
            List<Card> _toRemove = new List<Card>();

            _canRefresh = false;

            // Get rid of any loose cards and reorganize what we have
            foreach (Transform card in _displayedCards.Keys)
            {
                if (_displayedCards[card] == null || !_hand.Contains(_displayedCards[card])) // avoid a potential issue where player contains a null card?
                    _toRemove.Add(_displayedCards[card]);
                else
                {
                    card.DOLocalMove(new Vector3(-GetXaxisRight(card), 0f, 0f), 0.5f).SetEase(Ease.OutBack);
                    card.DOScale(1, .35f);
                }
            }

            if (_toRemove.Count > 0)
                RemoveCards(_toRemove);

            AddCards(_hand.Where(card => !_displayedCards.ContainsValue(card)).ToList());
        }

        void SetHandOrder()
        {
            // This saves our ordered list of cards back to the player's Hand. 
            IEnumerable<Card> orderedCards =
                from _card in _displayedCards.Keys
                where _displayedCards[_card] != null
                orderby _card.localPosition.x descending
                select _displayedCards[_card];

            List<Card> newHand = new List<Card>();

            for (int i = 0; i < orderedCards.Count(); i++)
                if (!newHand.Contains(orderedCards.ElementAt(i)))
                    newHand.Add(orderedCards.ElementAt(i));

            _game.playerMap[Game.actingPlayer].hand = newHand; // TODO: we're saving out the present order. If the player tabs to quickly, it's based on their animated X-axis order
                                                               // and then gets shuffled oddly. See about setting our hand value only before tabbing out. Presently this is resolved
                                                               // by delaying .1 seconds per card before executing this code but that feels mid. 
        }

        public void AddCards(List<Card> cards)
        {
            StartCoroutine(AddCardOnInterval(.1f));

            IEnumerator AddCardOnInterval(float f)
            {
                foreach (Card card in cards)
                {
                    AddCard(card);
                    yield return new WaitForSeconds(f);
                }                

                yield return new WaitForSeconds(cards.Count * 0.1f); // try waiting 1 frame so that we can ensure that our _cards table is properly set. Otherwise wait .1 per card + .1
                _canRefresh = true;
                SetHandOrder();
            }
        }

        public void AddCard(Card card)
        {
            GameObject _prefab = neutralPrefab;

            // Check if this card hasn't already been added by some other process!
            if (_displayedCards.ContainsValue(card)) return;

            if (card is ScoringCard) // Because ScoringCard is not a faction we use branching-if instead of a Dictionary
                _prefab = scoringPrefab;
            else if (card.faction == Game.Faction.China)
                _prefab = chinaPrefab;
            else if (card.faction == Game.Faction.USA)
                _prefab = usPrefab;
            else if (card.faction == Game.Faction.USSR)
                _prefab = ussrPrefab;

            GameObject _card = Instantiate(_prefab, cardOrigin.transform.position, Quaternion.identity, transform);

            CardUI uiCard = _card.GetComponent<CardUI>();
            uiCard.SetCard(card);
            uiCard.SetDragBehavior(_dragBehavior);

            if (Game.actingPlayer == Game.Faction.USSR)
                uiCard.highlight.color = Color.red;
            else if (Game.actingPlayer == Game.Faction.USA)
                uiCard.highlight.color = Color.cyan;
            uiCard.highlight.SetAlpha(0.5f);

            _displayedCards.Add(_card.transform, card);

            _card.transform.DOLocalMove(new Vector3(-GetXaxisLeft(_card.transform), 0f, 0f), 0.7f).SetEase(Ease.OutFlash);
            // TODO: OnComplete, save out the order of our cards back to the player's hand so that we retain our ordering. 
        }

        public void RemoveCards(List<Card> cards)
        {
            List<Transform> _toRemove = new List<Transform>();

            // Unset the Card from our list of cards immediately rather than every Nth-of-seconds; this ensures any new cards we're adding won't place themselves based on card's we're about to remove 
            foreach (Card card in cards)
            {
                foreach (Transform t in _displayedCards.Keys.Where(u => _displayedCards[u] == card || _displayedCards[u] == null).ToArray())
                {
                    _toRemove.Add(t);
                    _displayedCards.Remove(t);
                }
            }

            StartCoroutine(RemoveOnInterval(.08f));
            IEnumerator RemoveOnInterval(float f)
            {
                while (_toRemove.Count > 0)
                {
                    RemoveCard(_toRemove[0]);
                    _toRemove.RemoveAt(0);
                    yield return new WaitForSeconds(f);
                }

                SetHandOrder();
            }
        }

        public void RemoveCard(Card card)
        {
                foreach (Transform t in _displayedCards.Keys.Where(u => _displayedCards[u] == card).ToArray())
                {
                    _displayedCards.Remove(t);
                    //Destroy(t);
                }
        }

        void RemoveCard(Transform card)
        {
            card.DOLocalMove(cardOrigin.transform.position, 0.5f).
                SetEase(Ease.Linear).
                OnComplete(() =>
                {
                    _displayedCards.Remove(card);
                    Destroy(card.gameObject);
                });
        }

        void OnDealCards(Game.Faction faction, List<Card> cards)
        {
            if (faction == Game.actingPlayer)
                AddCards(cards);
        }

        float GetXaxisRight(Transform card)
        {
            int index = -1;
            float cardWidth = card.GetComponent<RectTransform>().rect.width;

            // Trying out this cool Query-Body Expression Thingy!
            IEnumerable<Transform> orderedCards =
                from _card in _displayedCards.Keys
                where _displayedCards[_card] != null
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
                from _card in _displayedCards.Keys
                where _displayedCards[_card] != null
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