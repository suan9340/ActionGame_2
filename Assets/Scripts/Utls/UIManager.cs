using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField] private Canvas UICanvas = null;
    [SerializeField] private Camera UICamera = null;


    [Space(10)]
    [SerializeField] private UIMain uiMain = null;
    public UIMain GetUIMain => uiMain;
    public Canvas GetUICanvas => UICanvas;
    public Camera GetUICamera => UICamera;


    public void Init()
    {
        uiMain.Initialize();
    }
}
