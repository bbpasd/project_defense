using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridFloor : MonoBehaviour
{
    public Material _originMaterial;
    public Renderer _renderer;

    public GameObject CurrentBuilding;

    // Start is called before the first frame update
    void Awake() {
        _renderer = GetComponent<Renderer>();
        _originMaterial = _renderer.material;
    }

    private void OnMouseDown() {
        if (CurrentBuilding != null) return;
        if (MyPlayerController.Instance.state.currentEnum == CommandState.BuildModeState) {
            _renderer.material = MyPlayerController.Instance.outlineMaterial;

            CurrentBuilding = MyPlayerController.Instance.CreateBuilding(this);
        }
    }

    private void OnMouseEnter() {
        if (CurrentBuilding != null) return;
        if (MyPlayerController.Instance.state.currentEnum == CommandState.BuildModeState) {
            _renderer.material = MyPlayerController.Instance.outlineMaterial;
        }
    }

    private void OnMouseExit() {
        _renderer.material = _originMaterial;
    }
}