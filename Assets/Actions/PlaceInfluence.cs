using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class PlaceInfluence : GameAction
{
    public override Command GetCommand(Card card, GameAction action) => new PlaceInfluenceCommand(card, action);

    public override void onCommandExecute(Command command) // This is called when we say "Use This Card for Realignments" - before specifying which countries. 
    {
        // the command already is a PlaceInfluenceCommand, we're just Upcasting it. This feels like a shitty hack. 
        PlaceInfluenceCommand placement = command as PlaceInfluenceCommand;

        List<Country> eligibleCountries = new List<Country>();

        foreach (Country country in FindObjectsOfType<Country>())
        {
            if (eligibleCountries.Contains(country) || country.GetComponent<MayNotPlaceInfluence>()) continue; 

            if(country.influence[placement.phasingPlayer] > 0)
                eligibleCountries.Add(country);
            else
                foreach(Country c in country.adjacentCountries)
                    if (c.influence[placement.phasingPlayer] > 0)
                    {
                        eligibleCountries.Add(country);
                        break;
                    }
        }

        countryClickHandler = new CountryClickHandler(eligibleCountries, onInfluencePlace);

        void onInfluencePlace(Country country)
        {
            int opsCost = country.control == placement.enemyPlayer ? 2 : 1; 

            if(placement.cardOpsValue >= placement.opsPlaced + opsCost)
            {
                placement.opsPlaced += opsCost;

                if (placement.influencePlacement.ContainsKey(country))
                    placement.influencePlacement[country] += 1;
                else
                    placement.influencePlacement.Add(country, 1);

                placement.placeInfluenceEvent.Invoke(placement);
                Game.AdjustInfluence.Invoke(country, placement.phasingPlayer, 1);
            }

            if(placement.cardOpsValue - placement.opsPlaced == 1)
            {
                //Remove any opponent-controlled countries if we don't have at least 2 ops remaining. 
                foreach(Country c in eligibleCountries)
                    if (c.control == placement.enemyPlayer)
                    {
                        eligibleCountries.Remove(c);
                        countryClickHandler.Remove(c); 
                    }
            }
            else if(placement.cardOpsValue - placement.opsPlaced == 0)
            {
                countryClickHandler.Close();
                placement.callback.Invoke(); 
            }
        }
    }

    public class PlaceInfluenceCommand : Command
    {
        public int opsPlaced = 0; 
        public Dictionary<Country, int> influencePlacement = new Dictionary<Country, int>();
        public GameEvent<PlaceInfluenceCommand> placeInfluenceEvent = new GameEvent<PlaceInfluenceCommand>();

        public PlaceInfluenceCommand(Card c, GameAction a) : base(c, a) { }
    }
}
