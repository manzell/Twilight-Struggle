using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using DG.Tweening; 

namespace TwilightStruggle.UI
{
    public class SpaceRaceAnims : Animations
    {
        [SerializeField] SpaceRace spaceRaceAction;
        [SerializeField] TextMeshProUGUI spaceRaceRoll, spaceRaceRollTarget, spaceRaceOpsNeeded, spaceRaceStage, spaceRaceSucess;
        [SerializeField] Image dieGraphic;
        [SerializeField] float animationDuration = .4f;
        [SerializeField] SpaceTrack spaceTrack;
        [SerializeField] Color targetHighlightColor, targetBaseColor; 

        private void Awake()
        {
            spaceRaceAction.prepareEvent.AddListener(AttemptMission); 
            GetComponent<CardDropHandler>().showEvent.AddListener(Unset);
        }

        public void Unset(Card card)
        {
            spaceRaceRoll.text = "6";
            spaceRaceRollTarget.text = spaceTrack.nextMission[Game.actingPlayer].rollRequired.ToString();
            spaceRaceOpsNeeded.text = spaceTrack.nextMission[Game.actingPlayer].opsRequired.ToString();
            spaceRaceStage.text = spaceTrack.nextMission[Game.actingPlayer].missionName.ToString();
            spaceRaceSucess.text = string.Empty;
            dieGraphic.SetAlpha(0);
        }

        void AttemptMission(GameCommand command)
        {
            StartCoroutine(Countdown(.7f, .3f));
            IEnumerator Countdown(float interval, float phaseLength)
            {
                int roll = ((SpaceRace.SpaceVars)command.parameters).roll; 
                for (int i = 6; i >= ((SpaceRace.SpaceVars)command.parameters).roll; i--)
                {
                    if (i < 6) 
                        FadeSwapText(spaceRaceRoll, i.ToString(), phaseLength);
                    
                    if (i == spaceTrack.nextMission[Game.actingPlayer].rollRequired)
                        FadeSwapText(spaceRaceSucess, "Success!", .3f);
                    if (roll <= spaceTrack.nextMission[Game.actingPlayer].rollRequired)
                        spaceRaceRoll.CrossFadeColor(targetHighlightColor, 3f, false, true); 

                    yield return new WaitForSeconds(interval); 
                }

                if(((SpaceRace.SpaceVars)command.parameters).roll > spaceTrack.nextMission[Game.actingPlayer].rollRequired)
                    FadeSwapText(spaceRaceSucess, "Mission Failed", .3f);

                yield return new WaitForSeconds(interval * 5);
                command.callback.Invoke(command); 
            }
        }
    }
}
