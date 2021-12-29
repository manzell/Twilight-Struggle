using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TwilightStruggle.UI
{
    public class UIHeadline : MonoBehaviour
    {
        private void Awake()
        {
            Game.phaseStartEvent.AddListener(phase => { if (phase is TurnSystem.HeadlinePhase) DisplayHeadlinePanel(phase); });
            Game.phaseEndEvent.AddListener(phase => { if (phase is TurnSystem.HeadlinePhase) HideHeadlinePanel(phase); });
        }

        void DisplayHeadlinePanel(TurnSystem.Phase phase) => transform.DOLocalMoveY(500, .5f);
        void HideHeadlinePanel(TurnSystem.Phase phase) => transform.DOLocalMoveY(800, .5f).SetDelay(1f).SetEase(Ease.InBounce);
    }
}