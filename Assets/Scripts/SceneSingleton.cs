using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ASingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                GameObject go = Managers.Resource.CreateEmpty($"@{typeof(T).Name}");
                // GameObject prefab = Resources.Load(typeof(T).Name) as GameObject;
                // GameObject singleton = Instantiate(prefab);
                _instance = go.AddComponent<T>();
            }

            return _instance;
        }
    }

    protected virtual void Awake() {
        if (_instance == null) {
            _instance = this as T;
        }
    }
}

public class SceneSingleton<T> : ASingleton<T> where T : MonoBehaviour
{
    protected override void Awake() {
        base.Awake();
    }
}