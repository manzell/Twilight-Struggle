using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace TwilightStruggle.UI
{
    public class CoupAnimation : Animations
    {
        [SerializeField] Coup coupAction;
        [SerializeField] TextMeshProUGUI selectTargetText, influenceChange, defconChange, confirmText, dieResult;
        [SerializeField] Image dieGraphic;
        [SerializeField] Button confirmButton;
        [SerializeField] float animationDuration = .4f; 

        private void Awake()
        {
            coupAction.prepareEvent.AddListener(PrepareCoup);
            coupAction.targetEvent.AddListener(SetTarget);
            coupAction.completeEvent.AddListener(ExecuteCoup);
        }

        private void Start()
        {
            Unset(); 
        }

        public void Unset()
        {
            selectTargetText.text = string.Empty; 
            influenceChange.text = string.Empty;
            defconChange.text = string.Empty;
            dieResult.text = string.Empty;
            confirmButton.GetComponent<CanvasGroup>().alpha = 0; 
            dieGraphic.SetAlpha(0);
        }

        public void PrepareCoup(GameCommand coup) // this is called at the END of Coup.Prepare(GameAction)
        {
            FindObjectOfType<UIMessage>().Message($"Select {coup.faction} Coup Target");
            
            dieGraphic.DOFade(1, animationDuration);
            dieResult.text = ((Coup.CoupVars)coup.parameters).coupOps.ToString(); 
            dieResult.DOFade(.25f, animationDuration);

            CountryClickHandler.Setup(((Coup.CoupVars)coup.parameters).eligibleTargets, OnClick);

            void OnClick(Country country)
            {
                ((Coup.CoupVars)coup.parameters).targetCountry = country;
                CountryClickHandler.Close();
                coupAction.Target(coup); 
            }
        }

        public void SetTarget(GameCommand coup)  // this is called at the END of Coup.Target(GameAction)
        {
            FindObjectOfType<UIMessage>().Message($"{coup.faction} Launches Coup in {((Coup.CoupVars)coup.parameters).targetCountry.countryName}!");
            FadeSwapText(selectTargetText, $"{((Coup.CoupVars)coup.parameters).targetCountry.countryName}\n" +
                $"[{((Coup.CoupVars)coup.parameters).coupDefense}]", animationDuration); 
            selectTargetText.DOFade(1, animationDuration);            

            if (((Coup.CoupVars)coup.parameters).targetCountry.isBattleground && ((Coup.CoupVars)coup.parameters).affectsDefcon == true) 
                defconChange.DOFade(1, animationDuration / 2).SetDelay(animationDuration / 2);

            confirmButton.GetComponent<CanvasGroup>().alpha = 1; 
            confirmButton.onClick.AddListener(() => coupAction.Complete(coup)); 
        }

        public void ExecuteCoup(GameCommand coup)  // this is called at the END of Coup.Complete(GameAction)
        {
            Dictionary<Game.Faction, int> infChange = ((Coup.CoupVars)coup.parameters).influenceChange; 
            influenceChange.text = string.Empty;

            confirmButton.GetComponent<CanvasGroup>().alpha = 0;
            dieResult.text += $"+{((Coup.CoupVars)coup.parameters).roll}";

            string usInfluenceChange = $"US {(infChange[Game.Faction.USA] > 0 ? "+" : "" + infChange[Game.Faction.USA])} Influence";
            string ussrInfluenceChange = $"USSR {(infChange[Game.Faction.USSR] > 0 ? "+" : "" + infChange[Game.Faction.USSR])} Influence";

            if (infChange[Game.Faction.USA] != 0)
                influenceChange.text += usInfluenceChange;
            if (infChange[Game.Faction.USA] != 0 && infChange[Game.Faction.USSR] != 0)
                influenceChange.text += "\n";
            if (infChange[Game.Faction.USSR] != 0)
                influenceChange.text += ussrInfluenceChange;

            influenceChange.DOFade(1, animationDuration * 2);

            StartCoroutine(Finish(3f));
            IEnumerator Finish(float t)
            {
                yield return new WaitForSeconds(t);
                coup.callback.Invoke(coup);
                Unset(); 
            }
        }
    }
}
