using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Linq; 

public class Phase : SerializedMonoBehaviour
{
    public string phaseName;
    Phase parent;
    public bool skipPhase;

    public UnityAction phaseCallback;

    [SerializeField] public List<UnityAction<UnityAction>> phaseStartActions, phaseEndActions;

    public void StartThread()
    {
        // We start from here with an empty callback
        StartPhase(() => { }); 
    }

    [Button] public virtual void StartPhase(UnityAction callback)
    {
        this.phaseCallback = callback; 
        Game.currentPhase = this; 
        parent = transform.parent?.GetComponent<Phase>(); 

        Debug.Log($"Start {(parent == null ? "" : parent.phaseName + " / ")}{phaseName}");
        phaseStartActions?.Process(() => NextPhase(callback));
    }

    public virtual void NextPhase(UnityAction callback)
    {
        List<Phase> subphases = GetComponentsInChildren<Phase>().ToList();
        subphases.Remove(this); // Always remove self. 

        // TODO - this is based on nesting within the inspector. Should it be?
        if (subphases.Count > 0)
            subphases[0].StartPhase(callback);
        else if (transform.parent?.childCount > transform.GetSiblingIndex() + 1)
            transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Phase>()?.StartPhase(callback);
        else
            EndPhase(callback);  
    }

    // TODO CHECK THE HECK OUT OF THIS!! I don't think it works properly. 
    public virtual void EndPhase(UnityAction callback)
    {
        phaseEndActions?.Process(callback);
        Game.currentPhase = null;

        if (parent)
            parent.EndPhase(callback);
        else if (callback != null)
            callback.Invoke(); 
            
    }
}
