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
        Game.actionRoundStartEvent.AddListener(onActionRoundStart);
        Game.actionRoundEndEvent.AddListener(onActionRoundEnd);
        Game.headlineEvent.AddListener(onHeadlineStart);
        Game.headlineEvent.after.AddListener(onHeadlineEnd);
    }

    void onActionRoundStart(ActionRound ar) => arDropPanel.SetActive(true);
    void onActionRoundEnd(ActionRound ar) => arDropPanel.SetActive(false);
    void onHeadlineStart(HeadlinePhase headline) => headlineDropPanel.SetActive(true);
    void onHeadlineEnd(HeadlinePhase headline) => headlineDropPanel.SetActive(false);
}
