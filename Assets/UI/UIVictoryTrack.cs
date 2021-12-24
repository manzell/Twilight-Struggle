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
            TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>();

            int index = 20 - child.GetSiblingIndex(); // 20 - 0 = 20. 
            // if(20 >= 4
            if (index >= VictoryTrack.vp)
            {
                // if 20 > 0 && 3 > 0
                if(index > 0 && VictoryTrack.vp > 0) // US Lead, US Chip
                {
                    Debug.Log($"Coloring in US VP {index} ({VictoryTrack.vp})");
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
                    Debug.Log($"Coloring in USSR VP {index} ({VictoryTrack.vp})");
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
        vpZero.GetComponentInChildren<TextMeshProUGUI>().color = VictoryTrack.vp == 0 ? Color.white : neutralColor;
    }
}
