using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class HeadlinePhase : Phase // There is just one of these
{
    public Dictionary<Game.Faction, HeadlineAction.HeadlineCommand> headlines = new Dictionary<Game.Faction, HeadlineAction.HeadlineCommand>();
    public GameEvent<HeadlinePhase> headlineEvent = new GameEvent<HeadlinePhase>();
    public bool skipHeadline = false; 

    public override void StartPhase(UnityAction callback)
    {
        FindObjectOfType<UIMessage>().Message("Headline Phase");
        base.StartPhase(callback);
    }
}
