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
        [HideInInspector] public UnityEvent<Phase>
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

                if (transform.parent && transform.parent.GetComponent<Phase>() && transform.parent.childCount > i + 1)
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
            Debug.Log($"Start Phase {this}"); 
            this.callback = callback;

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
                nextChild.StartPhase(() => EndPhase(callback));
            }
            else
                EndPhase(callback);
        }

        public virtual void EndPhase(UnityAction callback)
        {
            Debug.Log($"End Phase {this}"); 
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
                callback.Invoke(); // this is Game Over if it happens. 
        }

        void ProcessPhaseActions(List<IPhaseAction> onPhaseActions, UnityAction callback)
        {
            if (onPhaseActions.Count > 0)
            {
                IPhaseAction onPhaseAction = onPhaseActions[0];
                onPhaseActions.Remove(onPhaseAction);
                onPhaseAction.Action(this, () => ProcessPhaseActions(onPhaseActions, callback));
                Game.phaseActionEvent.Invoke(onPhaseAction);
            }
            else
                callback.Invoke();
        }
    }

    public interface IPhaseAction
    {
        public void Action(Phase phase, UnityAction callback);
    }
}