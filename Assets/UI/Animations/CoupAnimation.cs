using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening; 

namespace TwilightStruggle
{
    public class CoupAnimation : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI selectTargetText, influenceChange, defconChange, confirmText, dieResult;
        [SerializeField] Image dieGraphic; 

        public void PrepareCoup(Coup.CoupCommand coup)
        {
            selectTargetText.DOFade(1, .5f);
            dieGraphic.DOFade(1, .5f);
            dieResult.text = coup.card.OpsValue.ToString();
            dieResult.DOFade(.25f, .5f); 
        }

        public void SetTarget(Coup.CoupCommand coup)
        {
            FadeSwapText(selectTargetText, $"{coup.targetCountry.countryName}\n[{coup.targetCountry.stability * 2}]", .8f);
            confirmText.DOFade(1, .5f);

            if (coup.targetCountry.isBattleground)
                defconChange.DOFade(1, .25f).SetDelay(.25f); 
        }

        public void ExecuteCoup(Coup.CoupCommand coup)
        {
            influenceChange.text = string.Empty; 

            if(coup.influenceAdjusted[Game.Faction.USA] != 0)
            {
                string change = coup.influenceAdjusted[Game.Faction.USA] > 0 ? 
                    $"+{coup.influenceAdjusted[Game.Faction.USA]}" : coup.influenceAdjusted[Game.Faction.USA].ToString();

                influenceChange.text += $"US {change} Influence";

                if (coup.influenceAdjusted[Game.Faction.USSR] != 0) 
                    influenceChange.text += "\n";
            }
            if (coup.influenceAdjusted[Game.Faction.USSR] != 0)
            {
                string change = coup.influenceAdjusted[Game.Faction.USSR] > 0 ?
                    $"+{coup.influenceAdjusted[Game.Faction.USSR]}" : coup.influenceAdjusted[Game.Faction.USSR].ToString();

                influenceChange.text += $"USSR {change} Influence";
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
