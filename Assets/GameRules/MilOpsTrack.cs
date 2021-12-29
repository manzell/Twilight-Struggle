using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TwilightStruggle
{
    public class MilOpsTrack : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI USAmilOps, USSRmilOps, reqdMilOps;
        public static int requiredMilOps => DEFCONtrack.Status;
        public Dictionary<Game.Faction, int> milOps;

        void Awake()
        {
            Game.phaseStartEvent.AddListener(onTurnStart);
            Game.phaseEndEvent.AddListener(ScoreMilOps);

            Game.AdjustMilOps.after.AddListener(onAdjustMilOps);
        }

        void onAdjustMilOps(Game.Faction faction, int i)
        {
            //USAmilOps.text = milOps[Game.Faction.USA].ToString();
            //USSRmilOps.text = milOps[Game.Faction.USSR].ToString();
            //reqdMilOps.text = requiredMilOps.ToString(); 
        }

        void onTurnStart(TurnSystem.Phase phase)
        {
            if (phase is not TurnSystem.Turn) return;

            milOps = new Dictionary<Game.Faction, int>
        {
            {Game.Faction.USA, 0 },
            {Game.Faction.USSR, 0 }
        };

            onAdjustMilOps(Game.Faction.USA, 0);
        }

        public void GiveMilOps(Game.Faction faction, int i) => milOps[faction] = Mathf.Clamp(milOps[faction] + i, 0, 5);

        public void ScoreMilOps(TurnSystem.Phase phase)
        {
            if (phase is not TurnSystem.Turn) return;

            int usVPadjustment = Mathf.Clamp(milOps[Game.Faction.USA] - requiredMilOps, -5, 0);
            int ussrVPadjustment = Mathf.Clamp(milOps[Game.Faction.USSR] - requiredMilOps, -5, 0);

            int vpAdjustment = usVPadjustment - ussrVPadjustment;

            if (vpAdjustment > 0)
                Debug.Log($"USA gains {vpAdjustment} {(vpAdjustment == 1 ? "VP" : "VPs")} from MilOps");
            else if (vpAdjustment < 0)
                Debug.Log($"USSR gains {vpAdjustment} {(vpAdjustment == 1 ? "VP" : "VPs")} from MilOps");
            else
                Debug.Log("No MilOps VP points lose");

            Game.AdjustVPsEvent.Invoke(vpAdjustment);
        }
    }
}