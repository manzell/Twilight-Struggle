using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro; 

public class SpaceTrack : SerializedMonoBehaviour
{
    public Dictionary<int, SpaceRaceTrack> spaceRaceTrack = new Dictionary<int, SpaceRaceTrack>();
    public Dictionary<Game.Faction, int> attemptsRemaining = new Dictionary<Game.Faction, int>();

    [SerializeField] TextMeshProUGUI ussrSpace, usaSpace; 

    private void Awake()
    {
        Game.phaseStartEvent.AddListener(ResetSpaceRaceAttempts); 
    }

    void ResetSpaceRaceAttempts(Phase phase)
    {
        if(phase is Turn)
        {
            attemptsRemaining[Game.Faction.USA] = 1;
            attemptsRemaining[Game.Faction.USSR] = 1;
        }
    }

    public Dictionary<Game.Faction, int> spaceRaceLevel
    {
        get {
            Dictionary<Game.Faction, int> tmp = new Dictionary<Game.Faction, int> { { Game.Faction.USA, 0 }, { Game.Faction.USSR, 0 } };

            foreach (SpaceRaceTrack spaceRaceTrack in spaceRaceTrack.Values)
            {
                if (spaceRaceTrack.acheived.Contains(Game.Faction.USA)) tmp[Game.Faction.USA]++;
                if (spaceRaceTrack.acheived.Contains(Game.Faction.USSR)) tmp[Game.Faction.USSR]++;
            }

            return tmp;
        }
    }

    public struct SpaceRaceTrack
    {
        public string name;
        public int opsRequired;
        public int rollRequired;
        public int firstVPaward, secondVPaward;
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

        UpdateSpaceRace(); // I'm aware this is tying our UI into the underlying game logic I'll refactor it out later. 
    } 

    void UpdateSpaceRace()
    {
        ussrSpace.text = spaceRaceLevel[Game.Faction.USSR].ToString();
        usaSpace.text = spaceRaceLevel[Game.Faction.USA].ToString();
    }
}
