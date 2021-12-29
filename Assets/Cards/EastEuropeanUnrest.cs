using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class EastEuropeanUnrest : Card
    {
        [SerializeField] List<Country> easternEurope;

        public override void CardEvent(GameCommand command)
        {
            int count = 3;
            int amount = Game.gamePhase == Game.GamePhase.LateWar ? 2 : 1;
            List<Country> eligibleCountries = new List<Country>();

            foreach (Country country in easternEurope)
                if (country.influence[Game.Faction.USSR] > 0)
                    eligibleCountries.Add(country);

            FindObjectOfType<UI.UIManager>().SetButton(FindObjectOfType<UI.UIManager>().primaryButton, "Unrest Abates", Finish);

            RemoveInfluence(eligibleCountries, Game.Faction.USSR, count, 1, Finish); 

            void Finish()
            {
                FindObjectOfType<UI.UIManager>().UnsetButton(FindObjectOfType<UI.UIManager>().primaryButton);
                UI.CountryClickHandler.Close();
                command.FinishCommand();
            }
        }
    }
}