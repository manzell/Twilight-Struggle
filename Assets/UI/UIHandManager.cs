using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandManager : MonoBehaviour
{
    public Player currentPlayer;
    Player[] players;
    [SerializeField] GameObject cardPrefab; 
    Dictionary<Card, GameObject> cards = new Dictionary<Card, GameObject>();

    private void Awake()
    {
        players = FindObjectsOfType<Player>();
        Game.ActionRoundStart.AddListener(OnActionRoundStart); 
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            for(int i = 0; i < players.Length; i++)
            {
                if(players[i] != currentPlayer)
                {
                    currentPlayer = players[i];
                    Setup(currentPlayer); 
                    break; 
                }
            }
        }
    }

    void OnActionRoundStart(ActionRound actionRound)
    {
        foreach(Player player in players)
            if(player.faction == actionRound.phasingPlayer)
                Setup(currentPlayer);
    }

    public void Setup(Player player) 
    {
        List<Card> presentCards = new List<Card>();
        // Firstly, we look through our list of instantiated prefabs and remove any that aren't still in our hand. 
        foreach(Transform t in transform)
        {
            Card card = t.GetComponent<UICardDisplay>().card; 

            if (!player.hand.Contains(card))
            {
                cards.Remove(card); 
                Destroy(t.gameObject);
            }
            else
            {
                presentCards.Add(card); 
            }
        }

        // Then we go through our hand and instantiate any of the cards that are not present
        foreach(Card card in player.hand)
        {
            if(!presentCards.Contains(card))
            {
                GameObject cardObject = Instantiate(cardPrefab, transform);
                cardObject.GetComponent<UICardDisplay>().Setup(card); 
            }
        }

        if(player.faction == Game.Faction.USSR)
            GetComponent<UnityEngine.UI.Image>().color = new Color(.25f, .05f, 0f); 
        else if (player.faction == Game.Faction.USA)
            GetComponent<UnityEngine.UI.Image>().color = new Color(0f, .05f, .25f);
    }

}
