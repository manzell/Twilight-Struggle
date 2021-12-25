using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrack : MonoBehaviour
{
    public static int vp { get; private set; } = 0;

    private void Awake()
    {
        Game.AdjustVPs.before.AddListener(onAwardVP); 
    }

    public static void onAwardVP(int vpAmount)
    {
        vp += vpAmount; 

        if(Mathf.Abs(vp) >= 20)
        {
            Game.GameOver.Invoke(vp > 0 ? Game.Faction.USA : Game.Faction.USSR, "Victory Points"); 
        } 
    }
}
