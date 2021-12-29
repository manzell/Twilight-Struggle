using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class HeadlineAction : GameAction, IActionTarget, IActionComplete
    {
        // We write the headline card directly to the current Headline Phase. 
        // Once we have both, we go through the Headline Events on each card 
        // and then the Events on each card, in order of Ops. I believe only UN Intervention, The China Card, and Defectors have non-standard headline behaviors. 


        public void Target(GameCommand command) // Get's called each time a card is deposited
        {
            HeadlineVars headlineVars = (HeadlineVars)command.parameters;

            if(headlineVars.headlines[command.faction])
            {

            }
            else
            {

            }

        }

        public void Complete(GameCommand command) // Gets called when both cards are placed
        {
            HeadlineVars headlineVars = (HeadlineVars)command.parameters; 
            Game.Faction initiative = headlineVars.headlines[Game.Faction.USSR].opsValue > headlineVars.headlines[Game.Faction.USA].opsValue ? Game.Faction.USSR : Game.Faction.USA;
            Game.Faction secondary = initiative == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA; 

            command.callback = SecondHeadline;
            headlineVars.headlines[initiative].CardEvent(command); 

            void SecondHeadline(GameCommand command)
            {
                command.callback = null; 
                headlineVars.headlines[initiative].CardEvent(command);
            }
        }

        public class HeadlineVars : ICommandVariables
        {
            public Dictionary<Game.Faction, Card> headlines; 
        }

        //    

        //    FirstHeadline();
        //    void FirstHeadline()
        //    {
        //        Game.phasingPlayer = initiative;
        //        headlineCommand.actingPlayer = initiative;
        //        headlineCommand.callback = SecondHeadline;

        //        Debug.Log($"Headlining {headlines[headlineCommand.actingPlayer].card.cardName}");
        //        headlines[headlineCommand.actingPlayer].card.CardEvent(headlineCommand);
        //    }

        //    void SecondHeadline()
        //    {
        //        Game.phasingPlayer = headlineCommand.enemyPlayer;
        //        headlineCommand.actingPlayer = headlineCommand.enemyPlayer;
        //        headlineCommand.callback = FinishHeadline;

        //        Debug.Log($"Headlining {headlines[headlineCommand.actingPlayer].card.cardName}");
        //        headlines[headlineCommand.actingPlayer].card.CardEvent(headlineCommand);
        //    }

        //    void FinishHeadline()
        //    {

        //        Debug.Log("Finishing Headline Phase");
        //        //Game.currentPhase.NextPhase(originalCallback);
        //        originalCallback.Invoke(); // the callback SHOULD BE Game.currentPhase.nextPhase 
        //    }
        //}

        //public void SetHeadline(Game.Faction faction, Card card)
        //{
        //    HeadlineCommand headlineCommand = new HeadlineCommand(card, this);
        //    Dictionary<Game.Faction, HeadlineCommand> headlines = Game.currentTurn.headlinePhase.headlines;
        //    Player player = FindObjectOfType<Game>().playerMap[faction];

        //    if (headlines.ContainsKey(faction))
        //    {
        //        // Readd the current Headline to the player's hand.             
        //        if (headlines[faction] != null)
        //            player.hand.Add(headlines[faction].card); // Todo We should never remove the card until we affirm we can headline it

        //        headlines[faction] = headlineCommand;
        //    }
        //    else
        //        headlines.Add(faction, headlineCommand);

        //    player.hand.Remove(card);

        //    // For now, automatically trigger the headlines if we have both; but we'll eventually put this into a button. 
        //    if (headlines[Game.Faction.USSR] != null && headlines[Game.Faction.USA] != null)
        //        ExecuteCommandAction(headlineCommand);
        //}

    }

    interface IHeadlineEvent
    {
        public void HeadlineEvent(TurnSystem.HeadlinePhase headline);
    }
}