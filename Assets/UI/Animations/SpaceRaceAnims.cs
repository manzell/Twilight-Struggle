using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

namespace TwilightStruggle.UI
{
    public class SpaceRaceAnims : Animations
    {
        [SerializeField] SpaceRace spaceRaceAction;
        [SerializeField] TextMeshProUGUI spaceRaceRoll, spaceRaceRollTarget, spaceRaceStage, spaceRaceSucess;
        [SerializeField] Image dieGraphic;
        [SerializeField] float animationDuration = .4f;
        [SerializeField] SpaceTrack spaceTrack; 

        private void Awake()
        {
            spaceRaceAction.targetEvent.AddListener(TargetMission); 
            GetComponent<CardDropHandler>().showEvent.AddListener(c => Unset());
        }

        private void Start()
        {
            Unset();
        }

        public void Unset()
        {
            spaceRaceRoll.text = "6";
            spaceRaceRollTarget.text = spaceTrack.nextMission[Game.actingPlayer].opsRequired.ToString(); 
            spaceRaceStage.text = spaceTrack.nextMission[Game.actingPlayer].missionName.ToString();
            spaceRaceSucess.text = string.Empty;
            dieGraphic.SetAlpha(0);
        }

        void TargetMission(GameCommand command)
        {
            FadeSwapText(spaceRaceRoll, ((SpaceRace.SpaceVars)command.parameters).roll.ToString(), .8f); 


            command.callback.Invoke(command); 
        }
    }
}
