using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MilOpsTrack : SerializedMonoBehaviour
{
    public int requiredMilOps => DEFCON.status; 

    public Dictionary<Game.Faction, int> milOps = new Dictionary<Game.Faction, int>
    {
        {Game.Faction.USA, 0 },
        {Game.Faction.USSR, 0 }
    };

    public void onTurnStart()
    {
        milOps[Game.Faction.USA] = 0;
        milOps[Game.Faction.USSR] = 0;
    }

    public void GiveMilOps(Game.Faction faction, int i) => milOps[faction] = Mathf.Clamp(milOps[faction] + i, 0, 5); 

    public void scoreMilOps()
    {
        int usVPadjustment = Mathf.Clamp(milOps[Game.Faction.USA] - requiredMilOps, -5, 0);
        int ussrVPadjustment = Mathf.Clamp(milOps[Game.Faction.USSR] - requiredMilOps, -5, 0);

        Game.AwardVictoryPoints.Invoke(usVPadjustment - ussrVPadjustment);
    }
}