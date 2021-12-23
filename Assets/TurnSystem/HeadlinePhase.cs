using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadlinePhase : Phase // There is just one of these
{
    public Dictionary<Game.Faction, HeadlineAction.HeadlineCommand> headlines = new Dictionary<Game.Faction, HeadlineAction.HeadlineCommand>();
    public GameEvent<HeadlinePhase> headlineEvent = new GameEvent<HeadlinePhase>();

    public override void OnPhase(UnityAction callback)
    {
        this.callback = callback; 
        FindObjectOfType<UIMessage>().Message("Headline Phase");
        //Game.setActiveFactionEvent.Invoke(Game.Faction.USSR);
    }
}
