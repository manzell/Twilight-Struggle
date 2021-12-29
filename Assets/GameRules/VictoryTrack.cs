using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class VictoryTrack : MonoBehaviour
    {
        public static GameEvent<int> adjustVPevent = new GameEvent<int>();
        public static int vp = 0;

        private void Awake()
        {
            adjustVPevent.AddListener(onAwardVP);
        }

        public static void onAwardVP(int vpAmount)
        {
            if (Mathf.Abs(vp) >= 20)
            {
                Game.GameOver.Invoke(vp > 0 ? Game.Faction.USA : Game.Faction.USSR, "Victory Points");
            }
        }

        public static void AdjustVPs(int i)
        {
            vp += i;
            adjustVPevent.Invoke(i); 
        }
    }
}