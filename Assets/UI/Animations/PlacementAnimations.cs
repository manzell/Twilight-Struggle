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
            GetComponent<CardDropHandler>().showEvent.AddListener(Unset);
        }

        private void Unset(Card card)
        {
            opsRemaining.text = card.opsValue.ToString();
        }

        void AfterPrepare(GameCommand command)
        {
            opsRemaining.text = ((PlaceInfluence.InfluencePlacementVars)command.parameters).ops.ToString();

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
            CountryClickHandler.Close();
            influencePlacement.Complete(command);
        }

        void AfterComplete(GameCommand command)
        {
        }
    }
}
