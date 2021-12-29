using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class FormosanRevolution : Card
    {
        [SerializeField] Country taiwan;
        [SerializeField] Card chinaCard;
        [SerializeField] ScoringCard asiaScoring;

        public override void CardEvent(GameCommand command)
        {
            taiwan.gameObject.AddComponent<FormosanRevolution>();

            //chinaCard.triggerEvent.AddListener(CancelFormosanResolution);
            asiaScoring.scoreEvent.AddListener(CountTaiwanAsBattleground);

            command.FinishCommand();

            void CountTaiwanAsBattleground(Scoring scoring)
            {
                if (taiwan.control == Game.Faction.USA)
                {
                    scoring.USbattlegrounds += 1;
                    scoring.totalBattlegrounds += 1;
                }
            }

            void CancelFormosanResolution()
            {
                asiaScoring.scoreEvent.RemoveListener(CountTaiwanAsBattleground);
            }
        }
    }
}