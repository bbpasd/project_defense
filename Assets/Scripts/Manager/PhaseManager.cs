using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PhaseMonsterEnum
{
    DireWolf
}

public class PhaseManager : SceneSingleton<PhaseManager>
{
    public int nowPhase = 0;
    public PhaseStateMachine state;
    public List<Monster_Base> monsterList;

    public Transform monsterParent;
    
    // Spawn Data
    public Vector3 spwanPos = new Vector3(0.0f, 5.0f, 100.0f);
    public float spawnDelay = 1.0f;
    
    // Monster Data
    public GameObject[] monsterPrefab;
    
    // Spawn
    private int totalMonsterCount;
    
    public void Init() {
        state = gameObject.GetOrAddComponent<PhaseStateMachine>();
        monsterList = new();

        monsterParent = Managers.Resource.CreateEmpty("@Monsters").transform;

        totalMonsterCount = 0;
        
        // TODO enum으로 수정
        monsterPrefab = new GameObject[10];
        monsterPrefab[0] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
        monsterPrefab[1] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
        monsterPrefab[2] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
        monsterPrefab[3] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
        monsterPrefab[4] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
        monsterPrefab[5] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
        monsterPrefab[6] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
        monsterPrefab[7] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
        monsterPrefab[8] = Managers.Resource.LoadPrefab("Prefabs/Monster/DireWolf");
    }

    public void StartPhase() {
        state.ChangeState(PhaseState.Phase01State);
        Debug.Log(PhaseState.Phase01State);
    }

    public void SpawnMonster(PhaseMonsterEnum monster, int amount) {
        totalMonsterCount += amount;
        GameObject prefab = monsterPrefab[(int)monster];
        StartCoroutine(SpawnCoroutine(prefab, amount));
    }

    IEnumerator SpawnCoroutine(GameObject monster, int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject go = Managers.Resource.Instantiate(monster, spwanPos);
            Monster_Base mb = go.GetComponent<Monster_Base>();
            monsterList.Add(mb);
            mb.OnDeath += OnMonsterDeath;
            go.transform.parent = monsterParent;
            UpdateEnemyCount();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void OnMonsterDeath(Monster_Base monster) {
        monsterList.Remove(monster);
        totalMonsterCount--;
        UpdateEnemyCount();
        if (monsterList.Count <= 0) {
            state.ChangeState(PhaseState.PhaseStayState);
        }
    }

    private void UpdateEnemyCount() {
        MyPlayerController.Instance.UpdateEnemyCountUI(totalMonsterCount);
    }
}
