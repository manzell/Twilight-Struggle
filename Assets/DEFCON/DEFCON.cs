using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class DEFCON : SerializedMonoBehaviour
{
    public static int status = 5;

    public static Dictionary<Country.Continent, int> defconRestrictions = new Dictionary<Country.Continent, int>
    {
        { Country.Continent.Europe, 4 },
        { Country.Continent.Asia, 3 },
        { Country.Continent.MiddleEast, 2 },
    };

    public static UnityEvent<int> AdjustDEFCON = new UnityEvent<int>();

    public static void Adjust(int d)
    {
        status = Mathf.Clamp(status + d, 1, 5);

        AdjustDEFCON.Invoke(d); 

        if(status == 1)
        {
            Game.Faction winner = Game.phasingPlayer == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;
            Game.GameOver.Invoke(winner, "DEFCON Moved to 1");
        }
    }
}
