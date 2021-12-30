using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace TwilightStruggle
{
    public class DEFCONtrack : SerializedMonoBehaviour
    {
        public static UnityEvent<int> adjustDefconEvent = new UnityEvent<int>();

        public static int status = 5;

        public static Dictionary<Country.Continent, int> defconRestrictions = new Dictionary<Country.Continent, int>
        {
            { Country.Continent.Europe, 4 },
            { Country.Continent.Asia, 3 },
            { Country.Continent.MiddleEast, 2 },
            { Country.Continent.Africa, 1 },
            { Country.Continent.SouthAmerica, 1 },
            { Country.Continent.CentralAmerica, 1 }
        };

        public static void AdjustDefcon(Game.Faction faction, int amount)
        {
            status = Mathf.Clamp(status + amount, 1, 5);

            adjustDefconEvent.Invoke(amount);

            if (status == 1)
            {
                Game.Faction winner = faction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;
                Game.GameOver.Invoke(winner, "Global Thermonuclear War");
            }
        }
    }
}