using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening; 

namespace TwilightStruggle
{
    public class HeadlineAnimations : SerializedMonoBehaviour
    {
        [SerializeField] HeadlineAction headlineAction;
        [SerializeField] Dictionary<Game.Faction, TextMeshProUGUI> headlineText = new Dictionary<Game.Faction, TextMeshProUGUI>();
        [SerializeField] Dictionary<Game.Faction, Transform> dropAreas = new Dictionary<Game.Faction, Transform>();

        private void Awake()
        {
            headlineAction.prepareEvent.AddListener(AfterPrepare);
            //headlineAction.targetEvent.AddListener(AfterTarget);
            headlineAction.completeEvent.AddListener(AfterComplete);

            Game.phaseStartEvent.AddListener(phase => { if (phase is TurnSystem.HeadlinePhase) DisplayHeadlinePanel(phase); });
            Game.phaseStartEvent.AddListener(phase => { if(phase is TurnSystem.HeadlinePhase) FindObjectOfType<UI.UIMessage>().Message("Headline Phase"); }); 
        }

        void DisplayHeadlinePanel(TurnSystem.Phase phase) => transform.DOLocalMoveY(500, .5f);
        void HideHeadlinePanel() => transform.DOLocalMoveY(800, .5f).SetDelay(1f).SetEase(Ease.InBounce);

        void AfterPrepare(GameCommand command)
        {
            HeadlineAction.HeadlineVars headlineVars = (HeadlineAction.HeadlineVars)command.parameters;

            foreach(Game.Faction faction in headlineVars.headlines.Keys)
                headlineText[faction].DOFade(1f, 1f); 
        }

        void AfterComplete(GameCommand command) => HideHeadlinePanel();
    }
}
