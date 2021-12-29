using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class Defectors : Card, IHeadlineEvent
    {
        bool headlined = false;

        public override void CardEvent(GameCommand command)
        {
            if (Game.phasingPlayer == Game.Faction.USSR)
                VictoryTrack.AdjustVPs(1);

            command.FinishCommand();
        }

        public void HeadlineEvent(TurnSystem.HeadlinePhase headline)
        {
            //if (headline.headlines[Game.Faction.USA].card == this)
            //{
            //    // Discard the USSR Headline Card
            //    Game.deck.Add(headline.headlines[Game.Faction.USSR].card);

            //    // TODO: Skip Headline Phase

            //    Message($"USSR Headline ({headline.headlines[Game.Faction.USSR].card.cardName}) is canceled by Defectors!");
            //}
        }
    }
}