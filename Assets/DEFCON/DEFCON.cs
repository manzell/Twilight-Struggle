using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.UI; 

public class DEFCON : SerializedMonoBehaviour
{
    public static UnityEvent<int> AdjustDEFCON = new UnityEvent<int>();
    [SerializeField] GameObject[] DefconStatuses; 

    public static int Status = 5; 
    public int status
    {
        get { return Status; }
        set { Status = value; }
    }

    public static Dictionary<Country.Continent, int> defconRestrictions = new Dictionary<Country.Continent, int>
    {
        { Country.Continent.Europe, 4 },
        { Country.Continent.Asia, 3 },
        { Country.Continent.MiddleEast, 2 },
    };

    private void Awake()
    {
        Game.AdjustDEFCON.AddListener(onAdjustDefcon);
    }

    public void onAdjustDefcon(Game.Faction faction, int amount)
    {
        status = Mathf.Clamp(status + amount, 1, 5);

        AdjustDEFCON.Invoke(amount); 

        if(status == 1)
        {
            Game.Faction winner = Game.phasingPlayer == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;
            Game.GameOver.Invoke(winner, "Global Thermonuclear War");
        }

        UpdateDefcon(); 
    }

    private void UpdateDefcon()
    {
        for(int i = 0; i < DefconStatuses.Length; i++)
        {
            Color color = DefconStatuses[i].GetComponent<Image>().color;

            if (Status == 5 - i)
                DefconStatuses[i].GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);
            else
                DefconStatuses[i].GetComponent<Image>().color = new Color(color.r, color.g, color.b, .05f);
        }
    }
}
