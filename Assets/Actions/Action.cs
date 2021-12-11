using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class Action : SerializedMonoBehaviour, IDropHandler
{
    protected Game.Faction opponent { get { return Game.phasingPlayer == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA; } }
    public Card card;
    protected CountryClickHandler countryClickHandler;
    protected UnityAction callback;
    [SerializeField] protected GameObject cardSlot;

    public static Action currentAction;
    public ActionRound actionRound;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject cardObject = Instantiate(eventData.selectedObject, cardSlot.transform);

        cardObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        cardObject.transform.position = Vector3.zero;
        cardObject.transform.localPosition = Vector3.zero;
        Debug.Log($"Dropped {cardObject.GetComponent<UICardDisplay>().card}");

        if (card = cardObject.GetComponent<UICardDisplay>().card)
            Play(callback);

        Destroy(cardObject.GetComponent<UICardDisplay>());
    }

    public void SetActionRound(ActionRound actionRound)
    {
        this.actionRound = actionRound;
        callback = actionRound.phaseCallback; 
    }

    public abstract void Play(UnityAction callback);

    [Button]
    public void Trigger()
    {
        currentAction = this;
        Game.currentActionRound.action = this;

        Play(() => Game.currentActionRound.NextPhase(callback));
    }

    protected void FinishAction()
    {
        countryClickHandler?.Close(); 

        foreach(Transform t in transform)
            Destroy(t.gameObject); 

        callback?.Invoke();
    }
}

public interface IGameAction { }