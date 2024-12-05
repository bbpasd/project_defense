using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_HQ : Building_Base
{
    protected override void Init() {
        base.Init();
    }

    public override void OnAttack(float amount) {
        base.OnAttack(amount);
        //
        // gc.UpdateInfHQ(this);
        MyPlayerController.Instance.UpdateHQ();
    }

    public override void Destroy() {
        base.Destroy();
        
        ((UI_GameScene)(Managers.UI.SceneUI)).DisplayGameOverButton(true);
    }
}
