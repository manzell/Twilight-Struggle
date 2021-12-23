using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; 
using System.Linq;
using TMPro; 

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject arDropPanel;
    [SerializeField] public Button primaryButton, confirmButton, cancelButton; 
    public Game.Faction currentFaction;

    private void Awake()
    {
       // Game.setActiveFactionEvent.AddListener(faction => currentFaction = faction);

        Game.phaseStartEvent.AddListener(phase => {
            if(phase is ActionRound)
                arDropPanel.SetActive(true);
        });

        Game.phaseEndEvent.AddListener(phase => {
            if (phase is ActionRound)
                arDropPanel.SetActive(false);
        });
    }

    public void SetButton(Button button, string text, UnityAction callback)
    {
        Debug.Log(button);
        Debug.Log(button.onClick); 

        button.onClick.RemoveAllListeners();
        button.GetComponentInChildren<TextMeshProUGUI>().text = text; 
        button.onClick.AddListener(callback);
        button.interactable = true;
    }

    public void UnsetButton(Button button)
    {

        button.GetComponentInChildren<TextMeshProUGUI>().text = "-";
        button.onClick.RemoveAllListeners();
        button.interactable = false;
    }
}
