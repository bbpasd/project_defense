using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



/// <summary> 각종 매니져 관리, Singleton으로 동작 </summary>
public class Managers : MonoBehaviour
{
    private static Managers _instance;

    #region Managers
    public static Managers Instance => Init();

    public static ResourceManager Resource {
        get {
            if (Instance._resource != null) return Instance._resource;
            return Instance._resource = Instance.GetOrAddComponent<ResourceManager>();
        }
    }
    
    public static UIManager UI{
        get {
            if (Instance._ui != null) return Instance._ui;
            return Instance._ui = Instance.GetOrAddComponent<UIManager>();
        }
    }
    

    private ResourceManager _resource;
    private UIManager _ui;
    
    
    #endregion
    
    #region State
    // public static StateMachine State => Instance.GetOrAddComponent<StateMachine>();
    #endregion
    
    private static Managers Init() {
        if (_instance == null) {
            _instance = FindObjectOfType<Managers>();
            
            if (_instance == null) {
                GameObject singletonObject = new GameObject("@Managers");
                _instance = singletonObject.AddComponent<Managers>();
            }
        }
        return _instance;
    }
}
