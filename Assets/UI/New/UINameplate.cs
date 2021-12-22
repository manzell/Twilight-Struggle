using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class UINameplate : MonoBehaviour
{
    private void Start()
    {
        transform.DOLocalMoveY(0, 1f).SetEase(Ease.Linear);
    }

    private void Awake()
    {
        Game.phaseStartEvent.AddListener(onPhaseStart); 
    }

    void onPhaseStart(Phase phase)
    {
        Game.phaseStartEvent.RemoveListener(onPhaseStart);
        transform.DOLocalMoveY(800, .5f).SetEase(Ease.InBack).SetDelay(0.2f); 
    }
}
