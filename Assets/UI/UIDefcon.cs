using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDefcon : MonoBehaviour
{
    [SerializeField] Image activeImage; 
    [SerializeField] Sprite[] defconSprites;

    private void Awake()
    {
        Game.AdjustDEFCON.after.AddListener(UpdateDefcon); 
    }

    void UpdateDefcon(Game.Faction faction, int amount)
    {
        activeImage.sprite = defconSprites[DEFCON.Status - 1];
    }
}
