using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Linq; 

public class Phase : SerializedMonoBehaviour
{    
    public string phaseName;
    public UnityAction phaseCallback;
    
    [SerializeField] Phase parentPhase; 
    [SerializeField] public List<UnityAction<UnityAction>> phaseStartActions, phaseEndActions;

    public void StartThread()
    {
        // We start the Game from this point with an empty callback. This will probably cause things to break if we don't start at the root of Turn Manager. This should probably exist somewhere else. 
        StartPhase(() => { }); 
    }

    private void Awake()
    {
        if(transform.parent)
            transform.parent.TryGetComponent(out parentPhase);
    }

    [Button] public virtual void StartPhase(UnityAction callback)
    {
        phaseCallback = callback;
        Game.currentPhase = this;

        if(parentPhase)
            Debug.Log($"Start {(parentPhase == null ? string.Empty : parentPhase.phaseName + "/")} {phaseName}");

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

        if (parentPhase)
            parentPhase.EndPhase(callback);
        else if (callback != null)
            callback.Invoke(); 
            
    }
}
