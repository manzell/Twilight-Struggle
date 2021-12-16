using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class UIHandManager : MonoBehaviour
{
    public Player currentPlayer;
    List<Player> players; 

    private void Awake()
    {
        players = FindObjectOfType<Game>().playerMap.Values.ToList(); 
        Game.actionRoundStartEvent.AddListener(OnActionRoundStart);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != currentPlayer)
                {
                    currentPlayer = players[i];

                    break;
                }
            }
        }
    }

    public void Setup(Player player) => GetComponent<CardGenerator>().Setup(player.hand);

    void OnActionRoundStart(ActionRound actionRound)
    {
        foreach (Player player in players)
            if (player.faction == actionRound.phasingPlayer)
                GetComponent<CardGenerator>().Setup(currentPlayer.hand);
    }
}