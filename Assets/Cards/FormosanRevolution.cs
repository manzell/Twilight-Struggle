using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FormosanRevolution : Card
{
    [SerializeField] Country taiwan;
    [SerializeField] Card chinaCard;
    [SerializeField] ScoringCard asiaScoring;

    public override void CardEvent(GameAction.Command command)
    {
        taiwan.gameObject.AddComponent<FormosanRevolution>();

        //chinaCard.triggerEvent.AddListener(CancelFormosanResolution);
        asiaScoring.scoreEvent.AddListener(CountTaiwanAsBattleground);

        command.callback.Invoke();

        void CountTaiwanAsBattleground(Scoring scoring)
        {
            if (taiwan.control == Game.Faction.USA)
            {
                scoring.USSRbattlegrounds += 1;
                scoring.totalBattlegrounds += 1;
            }
        }
    }
}
