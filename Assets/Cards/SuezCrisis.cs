using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuezCrisis : Card
{
    [SerializeField] List<Country> countries;

    public override void CardEvent(GameAction.Command command)
    {
        uiManager.SetButton(uiManager.primaryButton, "Done repelling Tripartite aggression", Finish);
        RemoveInfluence(countries, Game.Faction.USA, 4, 2, Finish); 

        void Finish()
        {
            uiManager.UnsetButton(uiManager.primaryButton);
            command.callback.Invoke(); 
        }
    }
}
