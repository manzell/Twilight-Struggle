using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle.UI
{
    public class PlacementAnimations : MonoBehaviour
    {
        [SerializeField] AudioClip placementSound;
        [SerializeField] PlaceInfluence influencePlacement; 

        private void Awake()
        {
            influencePlacement.prepareEvent.AddListener(AfterPrepare);
            influencePlacement.targetEvent.AddListener(AfterTarget);
            influencePlacement.completeEvent.AddListener(AfterComplete);
        }

        void AfterPrepare(GameCommand command)
        {
            // We've received a card, set our ops value and all that. 
            // Now let's set up a Country Click handler: 
            // We're gonna roll our own function here to calculate ops cost and all that because the one for Card Event's is fundamentally different. 
            // Of note is that OnClick should not be where the logic resides for this, instead it should be 

            CountryClickHandler.Setup(((PlaceInfluence.InfluencePlacementVars)command.parameters).eligibleCountries, OnClick); 

            void OnClick(Country country)
            {
                ((PlaceInfluence.InfluencePlacementVars)command.parameters).countries.Add(country);
                influencePlacement.Target(command);
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
