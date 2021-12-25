using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceRace : GameAction
{
    public override Command GetCommand(Card card, GameAction action) => new SpaceShotCommand(card, action);

    public override void ExecuteCommandAction(Command command)
    {
        SpaceTrack spaceTrack = FindObjectOfType<SpaceTrack>();
        SpaceShotCommand spaceShot = command as SpaceShotCommand;

        // Assume we've already checked if the player has more space attempts remaining && if the card has enough Ops
        int spaceRaceLevel = spaceTrack.spaceRaceLevel[spaceShot.phasingPlayer];

        if(spaceShot.roll <= spaceTrack.spaceRaceTrack[spaceRaceLevel].rollRequired)
        {
            spaceShot.success = true;

            spaceTrack.AdvanceSpaceRace(spaceShot.phasingPlayer);

            int vpAward = spaceTrack.spaceRaceTrack[spaceRaceLevel].vpAwards[spaceTrack.spaceRaceTrack[spaceRaceLevel].acheived.Count];
            Game.AdjustVPs.Invoke(spaceShot.phasingPlayer == Game.Faction.USA ? vpAward : -vpAward);
        }

        Debug.Log($"{spaceShot.phasingPlayer} attempting {spaceTrack.spaceRaceTrack[spaceRaceLevel].name}. Rolled {spaceShot.roll}, needed {spaceTrack.spaceRaceTrack[spaceRaceLevel].rollRequired}. " +
            $"{(spaceShot.roll <= spaceTrack.spaceRaceTrack[spaceRaceLevel].rollRequired ? "Success!" : "Failure")}");

        spaceShot.callback.Invoke(); 
    }

    public class SpaceShotCommand : Command
    {
        public bool success; 
        public int roll;

        public SpaceShotCommand(Card c, GameAction a) : base(c, a)
        {
            roll = Random.Range(0, 6) + 1;
        }
    }
}
