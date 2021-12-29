using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class SuezCrisis : Card
    {
        [SerializeField] List<Country> countries;

        public override void CardEvent(GameCommand command)
        {
            uiManager.SetButton(uiManager.primaryButton, "Done repelling Tripartite aggression", Finish);
            RemoveInfluence(countries, Game.Faction.USA, 4, 2, Finish);

            void Finish()
            {
                uiManager.UnsetButtons();
                command.FinishCommand();
            }
        }
    }
}