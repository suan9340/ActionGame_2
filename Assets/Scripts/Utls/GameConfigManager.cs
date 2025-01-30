using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerClass
{
    [Header("기본 HP")] public float maxHp = 100;
    
    [Header("기본 Mp")] public float maxMP = 100;
    [Header("Mp 회복값")] public float mpValue = 10;
    [Header("Mp 회복시간")] public float mpTime = 5;
    [Header("Mp 사용값")] public float mpUseValue = 20f;
}
public class GameConfigManager : MonoBehaviourSingleton<GameConfigManager>
{
    [Header("플레이어 관련")]
    [SerializeField] private PlayerClass playerClass = null;
    public PlayerClass GetPlayerData => playerClass;

}
