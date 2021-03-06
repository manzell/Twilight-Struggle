using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace TwilightStruggle
{
    public class SpaceTrack : SerializedMonoBehaviour
    {
        public Dictionary<int, SpaceMission> spaceRaceTrack = new Dictionary<int, SpaceMission>();
        public Dictionary<Game.Faction, int> attemptsRemaining = new Dictionary<Game.Faction, int>();

        [SerializeField] TextMeshProUGUI ussrSpace, usaSpace;

        private void Awake()
        {
            Game.phaseStartEvent.AddListener(ResetSpaceRaceAttempts);
        }

        void ResetSpaceRaceAttempts(TurnSystem.Phase phase)
        {
            if (phase is TurnSystem.Turn)
            {
                attemptsRemaining[Game.Faction.USA] = 1;
                attemptsRemaining[Game.Faction.USSR] = 1;
            }
        }

        public Dictionary<Game.Faction, SpaceMission> nextMission
        {
            get
            {
                Dictionary<Game.Faction, SpaceMission> tmp = new Dictionary<Game.Faction, SpaceMission>();
                
                for(int i = 0; i < spaceRaceTrack.Count; i++)
                {
                    if(tmp.ContainsKey(Game.Faction.USA) == false && spaceRaceTrack[i].acheived.Contains(Game.Faction.USA) == false)
                        tmp.Add(Game.Faction.USA, spaceRaceTrack[i]);
                    if (tmp.ContainsKey(Game.Faction.USSR) == false && spaceRaceTrack[i].acheived.Contains(Game.Faction.USSR) == false)
                        tmp.Add(Game.Faction.USSR, spaceRaceTrack[i]);
                    if (tmp.Count == 2) 
                        break; 
                }

                return tmp; 
            }
        }

        public Dictionary<Game.Faction, int> spaceRaceLevel
        {
            get
            {
                Dictionary<Game.Faction, int> tmp = new Dictionary<Game.Faction, int> { { Game.Faction.USA, 0 }, { Game.Faction.USSR, 0 } };

                foreach (SpaceMission spaceMission in spaceRaceTrack.Values)
                {
                    if (spaceMission.acheived.Contains(Game.Faction.USA)) tmp[Game.Faction.USA]++;
                    if (spaceMission.acheived.Contains(Game.Faction.USSR)) tmp[Game.Faction.USSR]++;
                }

                return tmp;
            }
        }

        public struct SpaceMission
        {
            public string missionName;
            public int opsRequired;
            public int rollRequired;
            public int[] vpAwards;
            public GameAction ability;
            public List<Game.Faction> acheived;
        }

        public void AdvanceSpaceRace(Game.Faction faction) // All this does is Advance the Marker/set the acheived status. It does not grant abilities or victory points! 
        {
            for (int i = 0; i < spaceRaceTrack.Count; i++)
            {
                if (!spaceRaceTrack[i].acheived.Contains(faction))
                {
                    spaceRaceTrack[i].acheived.Add(faction);
                    break;
                }
            }

            UpdateSpaceRace(); // TODO Refactor this to a UI component
        }

        void UpdateSpaceRace()
        {
            Debug.Log("TODO: Update Space Race Track!"); 
        }
    }
}