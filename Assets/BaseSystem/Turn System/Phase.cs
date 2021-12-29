using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace TwilightStruggle.TurnSystem
{
    public class Phase : SerializedMonoBehaviour
    {
        public string phaseName;
        public UnityAction callback;
        public UnityEvent<Phase>
            phaseStartEvent = new UnityEvent<Phase>(),
            phaseEndEvent = new UnityEvent<Phase>();

        public List<IPhaseAction>
            beforePhaseActions = new List<IPhaseAction>(),
            onPhaseActions = new List<IPhaseAction>(),
            afterPhaseActions = new List<IPhaseAction>();

        public Phase prevPhase; 
        public Phase nextSibling
        {
            get
            {
                int i = transform.GetSiblingIndex();

                if (transform.parent && transform.parent.GetComponent<Phase>() && transform.parent.childCount > i + 1) // let's say we're the 5th of 5 items; i = 4.  5 > 
                    return transform.parent.GetChild(i + 1).GetComponent<Phase>();
                return null;
            }
        }

        public Phase nextChild
        {
            get
            {
                if (transform.childCount > 0)
                    return transform.GetChild(0).GetComponent<Phase>();
                return null;
            }
        }

        public Phase parentPhase
        {
            get
            {
                if (transform.parent && transform.parent.GetComponent<Phase>())
                    return transform.parent.GetComponent<Phase>();
                return null;
            }
        }

        public void StartThread()
        {
            Game.gameStartEvent.Invoke();
            StartPhase(() => Debug.Log("Game Over"));
        }

        public virtual void StartPhase(UnityAction callback)
        {
            this.callback = callback;

            if (parentPhase)
                Debug.Log($"Start {parentPhase.name}/{this.name}");
            else
                Debug.Log($"Start Phase: {this}");

            // Game State Vars:
            Game.currentPhase = this;

            // PhaseStartEvents
            Game.phaseStartEvent.Invoke(this);
            phaseStartEvent.Invoke(this);

            ProcessPhaseActions(beforePhaseActions, () =>
                ProcessPhaseActions(onPhaseActions, () => 
                    OnPhase(callback)));
        }

        public virtual void OnPhase(UnityAction callback) =>
            NextPhase(callback);

        public void NextPhase(UnityAction callback)
        {
            if (nextChild)
            {
                nextChild.prevPhase = this; 
                nextChild.StartPhase(() => EndPhase(callback)); // Pass our own end Phase to Children
            }
            else
                EndPhase(callback);
        }

        public virtual void EndPhase(UnityAction callback)
        {
            phaseEndEvent.Invoke(this);
            Game.phaseEndEvent.Invoke(this);

            ProcessPhaseActions(afterPhaseActions, () => Finalize(callback));
        }

        void Finalize(UnityAction callback)
        {
            if (nextSibling)
            {
                nextSibling.prevPhase = this;
                nextSibling.StartPhase(callback);
            }
            else if (parentPhase)
                parentPhase.EndPhase(callback);
            else
            {
                callback.Invoke(); // this is Game Over if it happens. 
            }
        }

        void ProcessPhaseActions(List<IPhaseAction> onPhaseActions, UnityAction callback)
        {
            if (onPhaseActions.Count > 0)
            {
                IPhaseAction onPhaseAction = onPhaseActions[0];
                onPhaseActions.Remove(onPhaseAction);
                onPhaseAction.OnPhase(this, () => ProcessPhaseActions(onPhaseActions, callback));
                Game.phaseActionEvent.Invoke(onPhaseAction);
            }
            else
                callback.Invoke();
        }
    }

    public interface IPhaseAction
    {
        public void OnPhase(Phase phase, UnityAction callback);
    }
}