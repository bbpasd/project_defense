using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microlight.MicroBar;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum BuildEnum
{
    HQ,
    Tower,
    Wall
}

public class MyPlayerController : SceneSingleton<MyPlayerController>
{
    public CommandStateMachine state;
    public Material outlineMaterial;

    public bool hasHQ = false;
    
    // 건물 건설 관련
    public BuildEnum buildEnum;
    public GameObject[] buildingPrefabs;
    public GameObject buildingParent;
    
    // 본진 건물
    public Building_Base hqBuilding;
    public List<Building_Base> buildingList;
    
    // 보유 자원
    public int leftMoney = 10;

    public int Money {
        get { return leftMoney; }
        set {
            leftMoney = value; 
            UpdateMoneyToUI();
        }
    }
    
    // UI
    private UI_GameScene GameScene;
    public Object_Base selectedObject;

    public void Init() {
        state = gameObject.GetOrAddComponent<CommandStateMachine>();
        outlineMaterial = Managers.Resource.Load<Material>("Material/Custom_OutlineShader");

        buildingList = new();
        buildingPrefabs = new GameObject[System.Enum.GetValues(typeof(BuildEnum)).Length];
        buildingPrefabs[(int)BuildEnum.HQ] = Managers.Resource.LoadPrefab("Prefabs/Buildings/HQ");
        buildingPrefabs[(int)BuildEnum.Tower] = Managers.Resource.LoadPrefab("Prefabs/Buildings/Tower/Tower_a");
        buildingPrefabs[(int)BuildEnum.Wall] = Managers.Resource.LoadPrefab("Prefabs/Buildings/Wall");

        StartCoroutine(TODOMoney());

        GameScene = (UI_GameScene)Managers.UI.SceneUI;
    }
    
    // TODO
    private void Update() {
        // 건설 모드일땐 무시
        if (state.currentEnum == CommandState.BuildModeState) {
            selectedObject = null;
            return; 
        }
        
        // 클릭 감지
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Building") || hit.collider.CompareTag("Monster")) {
                    // 충돌한 오브젝트의 Building_Base 컴포넌트를 찾음
                    Object_Base bb = hit.collider.GetComponent<Object_Base>();
                    if (bb != null) {
                        selectedObject = bb;
                        
                        if (hit.collider.CompareTag("Building")) {
                            GameScene.DisplayChunkOnly(UI_GameScene.Chunks.Chunk_UnitSelect);
                        }
                        
                        GameScene.DisplayInfSelect(true);
                        SetSelectObject(bb);
                    }
                }
                else {
                    selectedObject = null;
                    GameScene.DisplayInfSelect(false);
                }
            }
        }
    }

    private IEnumerator TODOMoney() {
        yield return new WaitForSeconds(0.01f);
        leftMoney = 10;
        UpdateMoneyToUI();
    }

    public void ChangeState(CommandState stateEnum) {
        state.ChangeState(stateEnum);
    }

    public GameObject CreateBuilding(GridFloor gridFloor) {
        if (buildingParent == null) {
            buildingParent = Managers.Resource.CreateEmpty("@Buildings");
        }

        if (leftMoney <= 0) return null;
        // if (buildEnum == BuildEnum.HQ && hqBuilding != null) return null;   // 본진은 하나만 건설가능
        
        Vector3 pos = Vector3.zero;
        GameObject go = Managers.Resource.Instantiate(buildingPrefabs[(int)buildEnum], pos);
        go.transform.parent = buildingParent.transform;

        float height = go.GetComponent<Building_Base>().heightPos;

        Vector3 newPos = new Vector3(gridFloor.transform.position.x, height, gridFloor.transform.position.z);
        go.transform.position = newPos;

        // HQ는 건설 메뉴 취소
        if (buildEnum == BuildEnum.HQ) {
            hqBuilding = go.GetComponent<Building_Base>();
            GameScene.ClickBPCancleButton();
            GameScene.DisplayInfHQChunk(true);
            SetHQ();
        }
        
        buildingList.Add(go.GetComponent<Building_Base>());
        Money -= 1;

        if (Money <= 0) {
            GameScene.ClickBPCancleButton();
        }
        return go;
    }

    public void SetActiveAllBuildingRangeCollider(bool active) {
        foreach (Building_Base bb in buildingList) {
            bb.SetAttackColliderActive(active);
        }
    }

    public void UpdateMoneyToUI() {
        GameScene.UpdateLeftMoneyText(leftMoney);
    }

    public void UpdateEnemyCountUI(int enemyCount) {
        GameScene.UpdateLeftEnemyText(enemyCount);
    }

    public void UpdateSelectObjectData(UpdateAnim animType = UpdateAnim.Damage) {
        GameScene.UpdateInfSelectData(selectedObject, animType);
    }

    public void SetSelectObject(Object_Base ob) {
        GameScene.SetInfSelectData(ob);
        GameScene.UpdateInfSelectData(selectedObject);
    }

    public void SetHQ() {
        GameScene.SetInfHQ(hqBuilding);
        UpdateHQ(UpdateAnim.Heal);
    }
    
    public void UpdateHQ(UpdateAnim animType = UpdateAnim.Damage) {
        GameScene.UpdateInfHQ(hqBuilding, animType);
    }
}