using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject arDropPanel;
    [SerializeField] GameObject headlineDropPanel;
    [SerializeField] public Button button;


    private void Awake()
    {
        Game.ActionRoundStart.AddListener(onActionRoundStart);
        Game.ActionRoundEnd.AddListener(onActionRoundEnd);
        Game.HeadlineStart.AddListener(onHeadlineStart);
        Game.HeadlineEnd.AddListener(onHeadlineEnd);
    }

    void onActionRoundStart(ActionRound ar) => arDropPanel.SetActive(true);
    void onActionRoundEnd(ActionRound ar) => arDropPanel.SetActive(false);
    void onHeadlineStart(Headline headline) { Debug.Log("OnHeadline Start");  headlineDropPanel.SetActive(true);  }
    void onHeadlineEnd(Headline headline) { Debug.Log("OnHeadline End"); headlineDropPanel.SetActive(false); }

}
