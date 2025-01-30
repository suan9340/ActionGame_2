using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerClass
{
    [Header("�⺻ HP")] public float maxHp = 100;
    
    [Header("�⺻ Mp")] public float maxMP = 100;
    [Header("Mp ȸ����")] public float mpValue = 10;
    [Header("Mp ȸ���ð�")] public float mpTime = 5;
    [Header("Mp ��밪")] public float mpUseValue = 20f;
}
public class GameConfigManager : MonoBehaviourSingleton<GameConfigManager>
{
    [Header("�÷��̾� ����")]
    [SerializeField] private PlayerClass playerClass = null;
    public PlayerClass GetPlayerData => playerClass;

}
