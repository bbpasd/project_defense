using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public UI_Base sceneUI;

    public UI_Base SceneUI {
        get {
            if (sceneUI == null) SetSceneUI();
            return sceneUI;
        }
    }

    public void Init() {
        SetSceneUI();
    }

    public void SetSceneUI(string uiName = "@UI_Scene") {
        sceneUI = GameObject.Find(uiName).GetComponent<UI_Base>();
    }

}