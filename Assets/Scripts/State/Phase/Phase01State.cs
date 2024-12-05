using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase01State : VMyState<PhaseState>
{
    public override PhaseState StateEnum => PhaseState.Phase01State;
    public int incomeAmount = 10;
    protected override void EnterState() {
        int phaseMonsterCount = 5 * (1 + PhaseManager.Instance.nowPhase);
        PhaseManager.Instance.SpawnMonster(PhaseMonsterEnum.DireWolf, phaseMonsterCount);
        MyPlayerController.Instance.SetActiveAllBuildingRangeCollider(true);

        UI_GameScene gc = (UI_GameScene)Managers.UI.SceneUI;
        gc.DisplayChunkOnly(UI_GameScene.Chunks.Chunk_MainMenu);
    }

    protected override void ExcuteState() {
    }

    protected override void ExitState() {
        UI_GameScene gc = (UI_GameScene)Managers.UI.sceneUI;
        gc.DisplayPhaseStartButton(true);   
        MyPlayerController.Instance.SetActiveAllBuildingRangeCollider(false);
        MyPlayerController.Instance.Money += incomeAmount;
        PhaseManager.Instance.nowPhase++;
    }
}