using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] private Player player = null;
    public Player GetPlayer => player;

    public void Start()
    {
        UIManager.Instance.Init();
        EnemyManager.Instance.Init();
    }
}
