using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace TwilightStruggle.UI
{
    public class CoupAnimation : MonoBehaviour
    {
        [SerializeField] Coup coupAction;
        [SerializeField] TextMeshProUGUI selectTargetText, influenceChange, defconChange, confirmText, dieResult;
        [SerializeField] Image dieGraphic;

        private void Awake()
        {
            coupAction.prepareEvent.AddListener(PrepareCoup);
            coupAction.targetEvent.AddListener(SetTarget);
            coupAction.completeEvent.AddListener(ExecuteCoup);
        }

        public void PrepareCoup(GameCommand coup) // this is called at the END of Coup.Prepare(GameAction)
        {
            Coup.CoupVars coupVars = (Coup.CoupVars)coup.parameters; 

            selectTargetText.DOFade(1, .5f);
            dieGraphic.DOFade(1, .5f);
            dieResult.text = coupVars.coupOps.ToString(); 
            dieResult.DOFade(.25f, .5f);

            CountryClickHandler.Setup(coupVars.eligibleTargets, OnClick);

            void OnClick(Country country)
            {
                CountryClickHandler.Close(); 
                coupVars.targetCountry = country;
                coup.parameters = coupVars;
                coupAction.Target(coup); 
            }
        }

        public void SetTarget(GameCommand coup)  // this is called at the END of Coup.Prepare(GameAction)
        {
            Coup.CoupVars coupVars = (Coup.CoupVars)coup.parameters;

            FadeSwapText(selectTargetText, $"{coupVars.targetCountry}\n[{coupVars.targetCountry.stability * 2}]", .8f);
            confirmText.DOFade(1, .5f);

            if (coupVars.targetCountry.isBattleground)
                defconChange.DOFade(1, .25f).SetDelay(.25f);

            // Normally we'd prompt for the Confirm Button here
            ExecuteCoup(coup); 
        }

        public void ExecuteCoup(GameCommand coup)  // this is called at the END of Coup.Prepare(GameAction)
        {
            Coup.CoupVars coupVars = (Coup.CoupVars)coup.parameters;

            influenceChange.text = string.Empty;
            string delta = string.Empty; 

            if (coupVars.influenceChange[Game.Faction.USA] != 0)
            {
                delta = coupVars.influenceChange[Game.Faction.USA] > 0 ? "+" : "" + coupVars.influenceChange[Game.Faction.USA].ToString();
            }

            if(delta != string.Empty)
                influenceChange.text = $"US {delta} Influence";

            if(influenceChange.text != string.Empty && coupVars.influenceChange[Game.Faction.USSR] != 0)
            {
                influenceChange.text += "\n";
                delta = coupVars.influenceChange[Game.Faction.USSR] > 0 ? "+" : "" + coupVars.influenceChange[Game.Faction.USSR].ToString();
                influenceChange.text += $"USSR {delta} Influence";
            }

            influenceChange.DOFade(1, 2f); 
        }

        public void FadeSwapText(TextMeshProUGUI text, string newText, float time)
        {
            text.DOFade(0f, time * .45f).OnComplete(() => { 
                text.text = newText; 
                text.DOFade(1f, time * .4f).SetDelay(time * .1f); 
            }); 
        }
    }


}
