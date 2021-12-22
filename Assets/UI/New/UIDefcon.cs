using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

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
        activeImage.DOCrossfadeImage(defconSprites[DEFCON.Status - 1], 2.5f).SetEase(Ease.Linear);
    }
}