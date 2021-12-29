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
            PlaceInfluence.PlacementVars placementVars = (PlaceInfluence.PlacementVars)command.parameters;

            CountryClickHandler.Setup(placementVars.eligibleCountries, Click); 

            void Click(Country country)
            {
                placementVars.countries.Add(country); 

                command.parameters = placementVars;

                influencePlacement.Target(command); 
            }
        }

        void AfterTarget(GameCommand command) 
        { 
            // After each click... the CountryClickHandler covers most UI stuff already - maybe we send an additional animation later? 
        }

        void AfterComplete(GameCommand command)
        {
            CountryClickHandler.Close(); 
        }
    }
}
