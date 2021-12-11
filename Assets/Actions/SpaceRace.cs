using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceRace : Action, IDropHandler
{
    SpaceTrack spaceTrack;

    public override void Play(UnityEngine.Events.UnityAction callback)
    {
        spaceTrack = FindObjectOfType<SpaceTrack>();

        if (card.opsValue < spaceTrack.spaceRaceTrack[spaceTrack.spaceRaceLevel[Game.phasingPlayer]].opsRequired) return;
        if (spaceTrack.attemptsRemaining[Game.phasingPlayer] <= 0) return;

        SpaceShot spaceShot = new SpaceShot();
        spaceShot.card = card;
        spaceShot.faction = Game.phasingPlayer;
        spaceShot.spaceRaceTrack = spaceTrack.spaceRaceTrack[spaceTrack.spaceRaceLevel[Game.phasingPlayer]];

        Game.currentActionRound.gameAction = spaceShot; 

        AttemptSpace(spaceShot); 
    }

    public class SpaceShot: IGameAction
    {
        public int roll;
        public Card card;
        public Game.Faction faction;
        public SpaceTrack.SpaceRaceTrack spaceRaceTrack;
    }

    public void AttemptSpace(SpaceShot attempt)
    {
        attempt.roll = Random.Range(0, 6) + 1; 
        Debug.Log($"{attempt.faction} attempting {attempt.spaceRaceTrack.name}. Rolled {attempt.roll}, needed {attempt.spaceRaceTrack.rollRequired}. {(attempt.roll <= attempt.spaceRaceTrack.rollRequired ? "Success!" : "Failure")}");
        spaceTrack.attemptsRemaining[attempt.faction]--;

        if(attempt.roll <= attempt.spaceRaceTrack.rollRequired)
        {
            int vpAward = attempt.spaceRaceTrack.vpAwards[attempt.spaceRaceTrack.acheived.Count];
            
            spaceTrack.AdvanceSpaceRace(attempt.faction);
            Game.AwardVictoryPoints.Invoke(attempt.faction == Game.Faction.USA ? vpAward : -vpAward);
        }

        FinishAction();
    }
}
