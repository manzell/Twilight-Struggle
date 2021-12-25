using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening; 
using TMPro; 

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _arDropPanel, _buttonPanel; 
    [SerializeField] public Button primaryButton, confirmButton, cancelButton; 
    public Game.Faction currentFaction;

    bool _buttonShow;

    private void Awake()
    {
        Game.phaseEndEvent.AddListener(phase => {
            if (phase is ActionRound)
                HideARPanel();
        });
    }

    public void SetButton(Button button, string text, UnityAction callback)
    {
        button.onClick.RemoveAllListeners();
        button.GetComponentInChildren<TextMeshProUGUI>().text = text; 
        button.onClick.AddListener(callback);
        button.interactable = true;

        ShowButtonPanel(); 
    }

    public void UnsetButtons()
    {
        UnsetButton(primaryButton); 
        UnsetButton(cancelButton);
        UnsetButton(confirmButton); 
    }

    public void UnsetButton(Button button)
    {

        button.GetComponentInChildren<TextMeshProUGUI>().text = "-";
        button.onClick.RemoveAllListeners();
        button.interactable = false;

        HideButtonPanel(); 
    }

    public void ShowButtonPanel()
    {
        if (_buttonShow == false)
        {
            _buttonShow = true;
            _buttonPanel.transform.DOKill(); 
            _buttonPanel.transform.DOLocalMoveY(460, .4f); 
        }
    }

    public void HideButtonPanel()
    {
        if(_buttonShow == true)
        {
            _buttonShow = false;
            _buttonPanel.transform.DOKill();
            _buttonPanel.transform.DOLocalMoveY(600, .4f); 
        }
    }

    public void ShowARPanel(Card card)
    {
        float f = 0f; 
        foreach (Transform t in _arDropPanel.transform)
        {
            if (t.TryGetComponent<UICardDropHandler>(out UICardDropHandler handler))
            {
                // TODO: Add a "CanUseGameAction(Faction/Card)" method to determine which actions to show

                handler.GetComponent<GameAction>().CanUseAction(Game.actingPlayer, card);

                if (handler.hidden)
                    handler.Show(f += .05f);
            }
        }
    }

    public void HideARPanel()
    {
        float f = 0f; 
        foreach (Transform t in _arDropPanel.transform)
        {
            if (t.TryGetComponent<UICardDropHandler>(out UICardDropHandler handler))
            {
                // TODO: Add a "CanUseGameAction(Faction/Card)" method to determine which actions to show
                if(handler.hidden == false)
                    handler.Hide(f += .1f);
            }
        }
    }
}
