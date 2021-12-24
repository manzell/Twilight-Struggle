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

    bool _buttonShow,
        _arShow; 

    private void Awake()
    {
       // Game.setActiveFactionEvent.AddListener(faction => currentFaction = faction);

        Game.phaseStartEvent.AddListener(phase => {
            if (phase is ActionRound)
                ShowARPanel(); 
        });

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

    void ShowButtonPanel()
    {
        if (_buttonShow == false)
        {
            _buttonShow = true;
            _buttonPanel.transform.DOKill(); 
            _buttonPanel.transform.DOLocalMoveY(460, .4f); 
        }
    }

    void HideButtonPanel()
    {
        if(_buttonShow == true)
        {
            _buttonShow = false;
            _buttonPanel.transform.DOKill();
            _buttonPanel.transform.DOLocalMoveY(600, .4f); 
        }
    }

    void ShowARPanel()
    {
        if(_arShow == false)
        {
            _arShow = true;
            _arDropPanel.transform.DOKill();
            _arDropPanel.transform.DOLocalMoveY(475f, .2f); 
        }
    }

    void HideARPanel()
    {
        if (_arShow == true)
        {
            _arShow = false;
            _arDropPanel.transform.DOKill();
            _arDropPanel.transform.DOLocalMoveY(672f, .2f);
        }
    }
}
