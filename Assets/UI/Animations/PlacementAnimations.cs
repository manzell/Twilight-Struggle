using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; 

namespace TwilightStruggle.UI
{
    public class PlacementAnimations : Animations
    {
        [SerializeField] AudioClip placementSound;
        [SerializeField] PlaceInfluence influencePlacement;
        [SerializeField] Image dieGraphic;
        [SerializeField] TextMeshProUGUI opsRemaining;
        [SerializeField] TextMeshProUGUI opsRemainingText; 

        private void Awake()
        {
            influencePlacement.prepareEvent.AddListener(AfterPrepare);
            influencePlacement.targetEvent.AddListener(AfterTarget);
            influencePlacement.completeEvent.AddListener(AfterComplete);
        }

        private void Start()
        {
            Unset(); 
        }

        private void Unset()
        {
            dieGraphic.ChangeAlpha(0);
            opsRemaining.text = string.Empty;
            opsRemaining.DOFade(0, 0f); 
            opsRemainingText.DOFade(0, 0f); 
        }

        void AfterPrepare(GameCommand command)
        {
            dieGraphic.DOFade(1, .8f);
            opsRemaining.text = ((PlaceInfluence.InfluencePlacementVars)command.parameters).ops.ToString();
            opsRemainingText.DOFade(1, .8f); 

            CountryClickHandler.Setup(((PlaceInfluence.InfluencePlacementVars)command.parameters).eligibleCountries, OnClick); 

            void OnClick(Country country)
            {
                ((PlaceInfluence.InfluencePlacementVars)command.parameters).countries.Add(country);
                influencePlacement.Target(command);
                FadeSwapText(opsRemaining, ((PlaceInfluence.InfluencePlacementVars)command.parameters).ops.ToString(), .8f);
            }
        }

        void AfterTarget(GameCommand command)
        {
            Unset(); 
            CountryClickHandler.Close();
            influencePlacement.Complete(command);
        }

        void AfterComplete(GameCommand command)
        {
        }
    }
}
