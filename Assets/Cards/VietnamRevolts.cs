using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VietnamRevolts : Card
{
    [SerializeField] Country vietnam;
    [SerializeField] List<Country> southeastAsia = new List<Country>();  

    public override void Event(UnityEngine.Events.UnityAction? callback)
    {
        /*
        if (!vietnam) return;

        vietnam.influence[Game.Faction.USSR] += 2;
        Game.AdjustInfluence.Invoke(vietnam, Game.Faction.USSR, 2);

        // Add an Influence Pip to Coups, Realigns, and Influece Placement

        Game.CoupConfirm.AddListener(BonusCoupOp);
        Game.TurnEnd.AddListener(p => Game.CoupConfirm.RemoveListener(BonusCoupOp));

        Game.InfluencePlacement.AddListener(BonusInfluence);
        Game.TurnEnd.AddListener(p => Game.InfluencePlacement.RemoveListener(BonusInfluence));

        Game.Realign.AddListener(BonusRealignment);
        Game.TurnEnd.AddListener(p => Game.Realign.RemoveListener(BonusRealignment)); 
        */
        callback.Invoke(); 
    }
    
    public void BonusInfluence(PlaceInfluence placement) 
    {
        /*
        if (placement.influencePlacement.Count == 0) return;
        if (placement.ops > 0) return; 

        bool allSEAsia = true; 

        foreach(Country country in placement.influencePlacement.Keys)
            allSEAsia &= southeastAsia.Contains(country); 

        if(allSEAsia)
        {
            placement.ops++;
            placement.NextInfluence(); 
        }
        */
    }

    public void BonusRealignment(Realign realign)
    {
        /*
        if (realign.realignAttempts.Count == 0) return;
        if (realign.realigns > 0) return;

        bool allSEAsia = true;

        foreach (Realign.RealignAttempt realignAttempt in realign.realignAttempts)
            allSEAsia &= southeastAsia.Contains(realignAttempt.targetCountry);             

        if (allSEAsia)
        {
            realign.realigns++;
            realign.NextRealign();
        }
        */
    }

    public void BonusCoupOp(Coup coup)
    {
        /*
        if(southeastAsia.Contains(coup.targetCountry))
        {
            coup.coupStrength += 1; 
        }
        */
    }
}
