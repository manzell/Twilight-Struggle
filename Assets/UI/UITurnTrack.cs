using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; 

namespace TwilightStruggle.UI
{
    public class UITurnTrack : SerializedMonoBehaviour
    {
        [SerializeField] Image _portrait;
        [SerializeField] TextMeshProUGUI _warPhaseText;

        private void Awake()
        {
            Game.phaseStartEvent.AddListener(OnTurnStart); 
        }

        [Button] void OnTurnStart(TurnSystem.Phase turn)
        {
            if(turn.TryGetComponent(out UIPhase phase))
            {
                _portrait.DOFade(0, .65f).OnComplete(() => {
                    _portrait.sprite = phase.phaseImage;
                    _portrait.DOFade(1, .35f).SetDelay(.25f);

                    switch (phase.gamePhase)
                    {
                        case Game.GamePhase.Setup:
                            _warPhaseText.text = "Setup";
                            break;
                        case Game.GamePhase.EarlyWar:
                            _warPhaseText.text = "Early War";
                            break;
                        case Game.GamePhase.Midwar:
                            _warPhaseText.text = "Midwar";
                            break;
                        case Game.GamePhase.LateWar:
                            _warPhaseText.text = "Late War";
                            break;
                        case Game.GamePhase.FinalScoring:
                            _warPhaseText.text = "Final Scoring";
                            break;
                    }
                });
            }
        }
    }
}
