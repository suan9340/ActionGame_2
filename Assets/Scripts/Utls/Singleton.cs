using UnityEngine;
using System.Collections;
using System;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    protected static T m_instance = null;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType(typeof(T)) as T;
                m_instance.Awake();
                if (m_instance == null)
                    Debug.LogError("MonoBehaviourSingleton Instance Init ERROR - " + typeof(T).ToString());
            }
            return m_instance;
        }
    }

    public static bool IsInstance()
    {
        return m_instance != null;
    }

    protected bool m_initialize;

    protected void Awake()
    {
        if (m_initialize)
            return;

        Initialize();
    }

    protected virtual void Initialize()
    {
        m_initialize = true;
        m_instance = this as T;
    }

    public virtual void Load(System.Action complete)
    {
        Debug.Log("Initalize - " + typeof(T).Name);
        complete();
    }

    public virtual void LoadComplete()
    {

    }

    protected virtual void OnDestroy()
    {
        m_instance = null;
    }

    private void OnApplicationQuit()
    {
        m_instance = null;
    }
}