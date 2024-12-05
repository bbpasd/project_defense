using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scene_GameScene : MonoBehaviour
{
    private void Awake() {
        MyPlayerController.Instance.Init();
        PhaseManager.Instance.Init();
        Managers.UI.SetSceneUI("@UI_Scene");
    }
}
