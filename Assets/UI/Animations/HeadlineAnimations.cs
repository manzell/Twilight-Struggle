using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening; 

namespace TwilightStruggle.UI
{
    public class HeadlineAnimations : Animations
    {
        TextMeshProUGUI test; 
        [SerializeField] HeadlineAction headlineAction;
        [SerializeField] Dictionary<Game.Faction, TextMeshProUGUI> headlineText = new Dictionary<Game.Faction, TextMeshProUGUI>();
        [SerializeField] Dictionary<Game.Faction, Transform> dropAreas = new Dictionary<Game.Faction, Transform>();
        [SerializeField] Color highlightHeadlineColor;

        [SerializeField] float animationDuration = .8f; 

        private void Awake()
        {
            headlineAction.prepareEvent.AddListener(AfterPrepare);
            headlineAction.completeEvent.AddListener(AfterComplete);

            headlineAction.headlineEvent.AddListener(OnHeadline);

            Game.phaseStartEvent.AddListener(phase => { if(phase is TurnSystem.HeadlinePhase) DisplayHeadlinePanel(); });
            Game.phaseStartEvent.AddListener(phase => { if(phase is TurnSystem.HeadlinePhase) FindObjectOfType<UIMessage>().Message("Headline Phase"); }); 
        }

        void AfterPrepare(GameCommand command)
        {
            HeadlineAction.HeadlineVars headlineVars = (HeadlineAction.HeadlineVars)command.parameters;

            foreach(Game.Faction faction in headlineVars.headlines.Keys)
                headlineText[faction].DOFade(1f, animationDuration);
        }

        void OnHeadline(GameCommand command)
        {
            FindObjectOfType<UIMessage>().Message($"{command.faction} Headlining {command.card.cardName}");

            FadeSwapText(headlineText[command.faction], command.card.cardName, animationDuration);
            headlineText[command.faction].DOColor(highlightHeadlineColor, 0f).SetDelay(animationDuration / 2);  
        }

        void DisplayHeadlinePanel() => transform.DOLocalMoveY(500, animationDuration / 2).SetEase(Ease.InSine);
        void HideHeadlinePanel() => transform.DOLocalMoveY(800, animationDuration / 2).SetEase(Ease.InBounce);
        void AfterComplete(GameCommand command) => HideHeadlinePanel();
    }
}
