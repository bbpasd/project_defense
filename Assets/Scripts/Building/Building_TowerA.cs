using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_TowerA : Building_Base
{
    protected override void Init() {
        base.Init();

        weaponPrefab = Managers.Resource.LoadPrefab("Prefabs/Weapon/Arrow");
    }
}
