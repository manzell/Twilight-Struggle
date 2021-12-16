using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class VietnamRevolts : Card
{
    [SerializeField] Country vietnam;
    [SerializeField] List<Country> southeastAsia = new List<Country>();

    public override void CardEvent(GameAction.Command command)
    {
        Game.AdjustInfluence.Invoke(vietnam, Game.Faction.USSR, 2); 


    } 
}
