using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building_Wall : Building_Base
{
    private NavMeshObstacle _navMeshObstacle;
    protected override void Init() {
        base.Init();

        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _navMeshObstacle.carving = true;
    }
}
