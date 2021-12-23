using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class UIHeadline : MonoBehaviour
{
    private void Awake()
    {
        Game.phaseStartEvent.AddListener(phase => { if (phase is HeadlinePhase) DisplayHeadlinePanel(phase); });
        Game.phaseEndEvent.AddListener(phase => { if (phase is HeadlinePhase) HideHeadlinePanel(phase); });
    }

    void DisplayHeadlinePanel(Phase phase) => transform.DOLocalMoveY(500, .5f);
    void HideHeadlinePanel(Phase phase) => transform.DOLocalMoveY(800, .5f).
        SetDelay(1f).
        SetEase(Ease.InBounce);
}
