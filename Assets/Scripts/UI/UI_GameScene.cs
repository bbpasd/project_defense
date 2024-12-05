using System;
using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary> 게임을 진행하는 씬의 UI </summary>
public class UI_GameScene : UI_Base
{
    enum TextmeshPro
    {
        Text_Score,
        Text_LeftMoney,
        Text_LeftEnemy,
        Text_Inf_HQ_HP,
        Text_Inf_Select_Name,
        Text_Inf_Select_HP,
        Text_Inf_UpgradeLevel_Value,
        Text_Inf_AttackPower_Value
    }

    enum Buttons
    {
        Btn_GameOver,
        Btn_PhaseStart,
        Btn_M_BuildMode,
        Btn_B_Cancle,
        Btn_B_HQ,
        Btn_B_Tower1,
        Btn_B_Wall,
        Btn_BP_Cancle,
        Btn_US_Cancle,
        Btn_US_Upgrade,
        Btn_US_Rotate,
        Btn_US_Heal,
        Btn_US_Destroy
    }

    public enum Chunks
    {
        Chunk_MainMenu,
        Chunk_BuildMenu,
        Chunk_BuildProgress,
        Chunk_UnitSelect,
        Chunk_Inf_HQ,
        Chunk_Inf_Select
    }

    enum Micro
    {
        MB_Inf_HQ,
        MB_Inf_Select
    }
    
    public bool isInitialized = false;
    public Object_Base selectedObject;
    
    public override void Init() {
        if (isInitialized) return;
        
        Bind<TextMeshProUGUI>(typeof(TextmeshPro));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(Chunks));
        Bind<MicroBar>(typeof(Micro));
        
        // Main Menu
        BindEvent(GetButton((int)Buttons.Btn_M_BuildMode), data => ClickMBuildModeButton());
        
        // Build Menu
        BindEvent(GetButton((int)Buttons.Btn_B_Cancle), data => ClickBCancleButton());
        BindEvent(GetButton((int)Buttons.Btn_B_HQ), data => ClickBBuildHQButton());
        BindEvent(GetButton((int)Buttons.Btn_B_Tower1), data => ClickBBuildTower1Button());
        BindEvent(GetButton((int)Buttons.Btn_B_Wall), data => ClickBBuildWallButton());
        
        // Build Progress
        BindEvent(GetButton((int)Buttons.Btn_BP_Cancle), data => ClickBPCancleButton());
        
        // Other Button
        BindEvent(GetButton((int)Buttons.Btn_PhaseStart), data => ClickPhaseStartButton());
        BindEvent(GetButton((int)Buttons.Btn_GameOver), data => ClickGameOverButton());
        
        // Unit Select
        BindEvent(GetButton((int)Buttons.Btn_US_Cancle), data => ClickUnitSelectCancleButton());
        BindEvent(GetButton((int)Buttons.Btn_US_Upgrade), data => ClickUnitSelectUpgradeButton());
        BindEvent(GetButton((int)Buttons.Btn_US_Rotate), data => ClickUnitSelectRotateButton());
        BindEvent(GetButton((int)Buttons.Btn_US_Heal), data => ClickUnitSelectHealButton());
        BindEvent(GetButton((int)Buttons.Btn_US_Destroy), data => ClickUniteSelectDestroyButton());
        
        
        // Display
        DisplayChunkOnly(Chunks.Chunk_MainMenu);
        
        
        
        DisplayInfHQChunk(false);
        DisplayGameOverButton(false);
        
        isInitialized = true;
    }

    #region Information

    public void SetInfHQ(Building_Base bb) {
        GetSomething<MicroBar>((int)Micro.MB_Inf_HQ).Initialize(bb.maxHP);
        GetTMP((int)TextmeshPro.Text_Inf_HQ_HP).text = $"{bb.currentHP}/{bb.maxHP}";
    }

    public void UpdateInfHQ(Object_Base bb, UpdateAnim animType = UpdateAnim.Damage) {
        GetSomething<MicroBar>((int)Micro.MB_Inf_HQ).UpdateBar(bb.currentHP, false, animType);
        GetTMP((int)TextmeshPro.Text_Inf_HQ_HP).text = $"{bb.currentHP}/{bb.maxHP}";
    }

    public void DisplayInfSelect(bool isVisible) {
        GetObject((int)Chunks.Chunk_Inf_Select).gameObject.SetActive(isVisible);
    }

    public void SetInfSelectData(Object_Base ob) {
        if (ob != null && selectedObject != ob) {
            selectedObject = ob;
            GetTMP((int)TextmeshPro.Text_Inf_Select_Name).text = ob.name;
            GetTMP((int)TextmeshPro.Text_Inf_Select_HP).text = $"{ob.currentHP}/{ob.maxHP}";
            GetSomething<MicroBar>((int)Micro.MB_Inf_Select).Initialize(ob.maxHP);
            GetSomething<MicroBar>((int)Micro.MB_Inf_Select).UpdateBar(ob.currentHP, true);
        }
    }

    public void UpdateInfSelectData(Object_Base ob, UpdateAnim animType = UpdateAnim.Damage) {
        GetSomething<MicroBar>((int)Micro.MB_Inf_Select).UpdateBar(ob.currentHP, false, animType);
        GetTMP((int)TextmeshPro.Text_Inf_Select_HP).text = $"{ob.currentHP}/{ob.maxHP}";
        GetTMP((int)TextmeshPro.Text_Inf_UpgradeLevel_Value).text = ob.upgradeLevel.ToString();
        GetTMP((int)TextmeshPro.Text_Inf_AttackPower_Value).text = ob.attackPower.ToString();
    }
    
    #endregion
    
    #region Display

    public void DisplayInfHQChunk(bool isVisible) {
        GetObject((int)Chunks.Chunk_Inf_HQ).gameObject.SetActive(isVisible);    
    }
    
    private void DisplayChunk(Chunks chunkIdx, bool isVisible) {
        GetObject((int)chunkIdx).gameObject.SetActive(isVisible);
    }

    public void DisplayChunkOnly(Chunks chunkIdx) {
        foreach (Chunks chunk in Enum.GetValues(typeof(Chunks))) {
            if (chunk == Chunks.Chunk_Inf_HQ) continue;
            
            bool isVisible = (chunk == chunkIdx);
            DisplayChunk(chunk, isVisible);
        }
    }

    private void SetButtonInteractable (Buttons buttonIdx, bool isInteractable) {
        Button btn = GetButton((int)buttonIdx); 
        btn.interactable = isInteractable;

        TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>();
        Color color = text.color;
        color.a = isInteractable ? 1f : 0.3f;
        text.color = color;
    }

    public void DisplayPhaseStartButton(bool isVisible) {
        GetButton((int)Buttons.Btn_PhaseStart).gameObject.SetActive(isVisible);
    }

    public void DisplayGameOverButton(bool isVisible) {
        GetButton((int)Buttons.Btn_GameOver).gameObject.SetActive(isVisible);
    }
    
    #endregion
    
    #region Button Other 

    public void ClickPhaseStartButton() {
        PhaseManager.Instance.StartPhase();
        DisplayPhaseStartButton(false);
    }

    public void ClickGameOverButton() {
        
    }
    
    #endregion
    
    #region Button MainPage 
    private void ClickMBuildModeButton() {
        if (PhaseManager.Instance.state.currentEnum != PhaseState.PhaseStayState) return;
        DisplayChunkOnly(Chunks.Chunk_BuildMenu);
    }
    #endregion
    
    #region Button BuildPage 
    // 건물 건설 페이지
    public void ClickBCancleButton() {
        DisplayChunkOnly(Chunks.Chunk_MainMenu);
    }

    public void ClickBBuildHQButton() {
        if (MyPlayerController.Instance.hqBuilding != null) return;
        if (MyPlayerController.Instance.Money <= 0) return;
        BuildButtons(BuildEnum.HQ);
    }
    
    public void ClickBBuildTower1Button() {
        if (MyPlayerController.Instance.Money <= 0) return;
        BuildButtons(BuildEnum.Tower);
    }
    
    public void ClickBBuildWallButton() {
        if (MyPlayerController.Instance.Money <= 0) return;
        BuildButtons(BuildEnum.Wall);
    }

    private void BuildButtons(BuildEnum buildEnum) {
        DisplayChunkOnly(Chunks.Chunk_BuildProgress);
        MyPlayerController.Instance.ChangeState(CommandState.BuildModeState);
        MyPlayerController.Instance.buildEnum = buildEnum;
    }
    #endregion

    #region Button BuildProgressPage 
    public void ClickBPCancleButton() {
        MyPlayerController.Instance.ChangeState(CommandState.NormalModeState);
        DisplayChunkOnly(Chunks.Chunk_BuildMenu);
    }
    
    #endregion

    #region Button Unit Select

    private void ClickUnitSelectCancleButton() {
        DisplayChunkOnly(Chunks.Chunk_MainMenu);
    }

    private void ClickUnitSelectUpgradeButton() {
        // Upgrade
        if (selectedObject is Building_Base) {
            Building_Base bb = (Building_Base)selectedObject;

            if (MyPlayerController.Instance.Money >= selectedObject.upgradeCost) {
                MyPlayerController.Instance.Money -= selectedObject.upgradeCost;
                bb.UpgradeBuilding();
            }
        }
    }

    private void ClickUnitSelectRotateButton() {
        Quaternion currentRotation = MyPlayerController.Instance.selectedObject.transform.rotation;
        Quaternion rotation90Degrees = Quaternion.Euler(currentRotation.eulerAngles + new Vector3(0, 90, 0));
        MyPlayerController.Instance.selectedObject.transform.rotation = rotation90Degrees;
    }

    private void ClickUnitSelectHealButton() {
        if (MyPlayerController.Instance.Money >= selectedObject.healCost) {
            MyPlayerController.Instance.Money -= selectedObject.healCost;
            selectedObject.Heal(50.0f);
        }
    }
    
    private void ClickUniteSelectDestroyButton() {
        MyPlayerController.Instance.Money += 1;
        ((Building_Base)selectedObject).Destroy();
        selectedObject = null;
        DisplayChunkOnly(Chunks.Chunk_MainMenu);
    }

    #endregion
    
    
    #region Set Text

    public void UpdateLeftMoneyText(int amount) {
        GetTMP((int)TextmeshPro.Text_LeftMoney).text = $"남은 자원 : {amount.ToString()}";
    }
    
    public void UpdateLeftEnemyText(int amount) {
        GetTMP((int)TextmeshPro.Text_LeftEnemy).text = $"남은 적 : {amount.ToString()}";
    }
    
    #endregion
}