using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject arDropPanel;
    [SerializeField] GameObject headlineDropPanel;
    [SerializeField] public Button button;
    public Game.Faction currentFaction;

    private void Awake()
    {
        Game.setActiveFactionEvent.AddListener(faction => currentFaction = faction);

        Game.phaseStartEvent.AddListener(phase => {
            if(phase is ActionRound)
                arDropPanel.SetActive(true);
            else if(phase is HeadlinePhase)
                headlineDropPanel.SetActive(true);
        });

        Game.phaseEndEvent.AddListener(phase => {
            arDropPanel.SetActive(false);
            headlineDropPanel.SetActive(false);
        });
    }

    private void Update()
    {
        List<Player> players = FindObjectsOfType<Player>().ToList();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].faction != currentFaction)
                {
                    Game.setActiveFactionEvent.Invoke(players[i].faction); 
                    break;
                }
            }
        }
    }
}
