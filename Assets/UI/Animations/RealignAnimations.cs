using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening; 

namespace TwilightStruggle.UI
{
    public class RealignAnimations : Animations
    {
        [SerializeField] Realign realignAction; 
        [SerializeField] TextMeshProUGUI attemptsRemaining;
        [SerializeField] TextMeshProUGUI rolls, influenceChange;

        private void Awake()
        {
            realignAction.prepareEvent.AddListener(AfterPrepare);
            realignAction.targetEvent.AddListener(AfterTarget);
            realignAction.completeEvent.AddListener(AfterComplete);
            GetComponent<CardDropHandler>().showEvent.AddListener(Unset);
        }

        public void Unset(Card card)
        {
            attemptsRemaining.text = card.opsValue.ToString();
            rolls.alpha = 0; 
            influenceChange.alpha = 0;
        }

        public void AfterPrepare(GameCommand command) // After we drop our card
        {
            FindObjectOfType<UIMessage>().Message($"Select {command.faction} Realign Target");

            CountryClickHandler.Setup(Realign.GetEligibleCountries(command), OnClick);

            void OnClick(Country country)
            {
                Realign.RealignAttempt attempt = new Realign.RealignAttempt();
                attempt.country = country; 

                ((Realign.RealignVars)command.parameters).realignAttempts.Add(attempt);
                realignAction.Target(command);
            }
        }

        public void AfterTarget(GameCommand command) // After we click on a country
        {
            Realign.RealignAttempt realignAttempt = ((Realign.RealignVars)command.parameters).realignAttempts[((Realign.RealignVars)command.parameters).realignAttempts.Count - 1];

            int usRoll = realignAttempt.rolls[Game.Faction.USA] + realignAttempt.modifiers[Game.Faction.USA];
            int ussrRoll = realignAttempt.rolls[Game.Faction.USSR] + realignAttempt.modifiers[Game.Faction.USSR];

            FadeSwapText(attemptsRemaining, $"Realign in {realignAttempt.country.countryName}", .4f);
            FadeSwapText(rolls, $"US Rolls {usRoll}\nUSSR Rolls {ussrRoll}", .4f);

            FadeSwapText(influenceChange, $"", .4f);

            CountryClickHandler.Refresh(Realign.GetEligibleCountries(command));

            if (((Realign.RealignVars)command.parameters).ops == 0)
                realignAction.Complete(command); 
        }

        public void AfterComplete(GameCommand command) // Once our ops is zero
        {
            CountryClickHandler.Close(); 
        }
    }
}
