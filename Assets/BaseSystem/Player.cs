using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TwilightStruggle
{
    public class Player : MonoBehaviour
    {
        public static Player USA, USSR;

        public Game.Faction faction;
        public List<Card> hand;

        private void Awake()
        {
            if (faction == Game.Faction.USA) USA = this;
            else if (faction == Game.Faction.USSR) USSR = this;
        }
    }
}
