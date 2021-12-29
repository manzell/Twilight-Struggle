using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

namespace TwilightStruggle.UI
{
    public class UIActionRound : MonoBehaviour
    {
        [SerializeField] Image USAmarker, USSRmarker;
        [SerializeField] TextMeshProUGUI usaText, ussrText;

        private void Awake()
        {
            Game.phaseStartEvent.AddListener(OnActionRoundStart); 
        }

        void OnActionRoundStart(TurnSystem.Phase phase)
        {
            if (phase is not TurnSystem.ActionRound)
            {
                usaText.text = string.Empty; 
                ussrText.text = string.Empty;

                StartCoroutine(FadeAlpha(USAmarker, .1f, .3f));
                StartCoroutine(FadeAlpha(USSRmarker, .1f, .3f));
            }
            else
            {
                TurnSystem.ActionRound actionRound = phase as TurnSystem.ActionRound;

                List<TurnSystem.ActionRound> actionRounds = phase.transform.parent.GetComponentsInChildren<TurnSystem.ActionRound>().ToList();
                int actionRoundNumber = actionRounds.IndexOf(actionRound) / 2 + 1; 

                usaText.text = actionRoundNumber.ToString();
                ussrText.text = actionRoundNumber.ToString();

                if (actionRound.phasingPlayer == Game.Faction.USA)
                {
                    StartCoroutine(FadeAlpha(USAmarker, 1f, .3f));
                    StartCoroutine(FadeAlpha(USSRmarker, .1f, .3f));
                }
                else
                {
                    StartCoroutine(FadeAlpha(USAmarker, .1f, .3f));
                    StartCoroutine(FadeAlpha(USSRmarker, 1f, .3f));
                }
            } 
        }

        static IEnumerator FadeAlpha(Image image, float targetAlpha, float time)
        {
            float t = 0f;
            float initAlpha = image.color.a;
            float delta = targetAlpha - initAlpha;

            while (t < time)
            {
                float pctComplete = t / time;
                image.SetAlpha(initAlpha + delta * pctComplete);

                t += Time.deltaTime;
                yield return null;
            }
        }
    }
}
