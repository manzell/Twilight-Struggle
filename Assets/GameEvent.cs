using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class GameEvent : UnityEvent
{
    public UnityEvent before, after;

    public GameEvent() : base()
    {
        before = new UnityEvent(); 
        after = new UnityEvent();
    }

    public new void Invoke()
    {
        if (before != null) before.Invoke();
        base.Invoke();
        if (after != null) after.Invoke();
    }
}

public class GameEvent<T> : UnityEvent<T>
{
    public UnityEvent<T> before, after;

    public GameEvent() : base()
    {
        before = new UnityEvent<T>();
        after = new UnityEvent<T>();
    }

    public new void Invoke(T t)
    {
        before.Invoke(t);
        base.Invoke(t);
        after.Invoke(t);
    }
}

public class GameEvent<T, U> : UnityEvent<T, U>
{
    public UnityEvent<T, U> before, after;

    public GameEvent() : base()
    {
        before = new UnityEvent<T, U>();
        after = new UnityEvent<T, U>();
    }

    public new void Invoke(T t, U u)
    {
        before.Invoke(t, u);
        base.Invoke(t, u);
        after.Invoke(t, u);
    }
}

public class GameEvent<T, U, V> : UnityEvent<T, U, V>
{
    public UnityEvent<T, U, V> before, after;

    public GameEvent() : base()
    {
        before = new UnityEvent<T, U, V>();
        after = new UnityEvent<T, U, V>();
    }

    public new void Invoke(T t, U u, V v)
    {
        before.Invoke(t, u, v);
        base.Invoke(t, u, v);
        after.Invoke(t, u, v);
    }
}