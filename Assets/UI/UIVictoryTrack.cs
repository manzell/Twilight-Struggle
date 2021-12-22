using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class UIVictoryTrack : MonoBehaviour
{
    [SerializeField] Color usColor, ussrColor, neutralColor;
    [SerializeField] Transform vpZero; 

    private void Awake()
    {
        Game.AdjustVPs.after.AddListener(UpdateVictoryTrack); 
    }

    void UpdateVictoryTrack(int amount)
    {
        foreach(Transform child in transform)
        {
            Image image = child.GetComponent<Image>();
            TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();

            int index = 20 - child.GetSiblingIndex();
            if (index >= VictoryTrack.vp)
            {
                if(index > 0 && VictoryTrack.vp > 0) // US Lead, US Chip
                {
                    image.color = usColor;
                    text.color = Color.white; 
                }
                else if(index > 0) // US Chip, Tied or US Lead
                {
                    image.color = Color.white;
                    text.color = usColor; 
                }
                
                if(index < 0 && VictoryTrack.vp < 0) // USSR Lead, USSR Chip
                {
                    image.color = ussrColor;
                    text.color = Color.white;
                }
                else if (index < 0)
                {
                    image.color = Color.white; // USSR Chip, US Lead or Tied. 
                    text.color = ussrColor;
                }
            }
        }

        vpZero.GetComponent<Image>().color = VictoryTrack.vp == 0 ? neutralColor : Color.white;
        vpZero.GetComponent<TextMeshProUGUI>().color = VictoryTrack.vp == 0 ? Color.white : neutralColor;
    }
}