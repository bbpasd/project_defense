using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class Weapon_Base : MonoBehaviour
{
    public float attackPower;
    public GameObject target;

    public float speed = 10.0f;
    public float rotateSpeed = 50.0f;

    public bool isInitialized = false;
    public Building_Base owner;
        
    private void Awake() {
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine() {
        yield return new WaitForSeconds(0.01f);
        Init();
    }

    protected virtual void Init() {
        isInitialized = true;
        if (target == null) return;
        
        // 만약 몬스터의 타겟이 본진이라면 이 Weapon의 Owner로 설정 (공격하는 타워로 어그로 변경)
        Monster_Base mb = target.GetComponent<Monster_Base>();
        if (mb != null) {
            if (mb.target == MyPlayerController.Instance.hqBuilding.gameObject) {
                mb.UpdateTarget(owner);
                // Debug.Log($"어그로 변경!{owner.name}");
            }
        }
    }

    private void Update() {
        if (!isInitialized) return;
        if (target == null) {
            OnDestroy();
            return;
        }
        
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        
        // Quaternion targetRotation = Quaternion.LookRotation(direction);
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        
        // 천천히 회전이 아닌 즉시 회전으로 수정
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;

    }

    public void SetTarget(GameObject go) {
        target = go;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == target) {
            Hit();
        }
    }

    private void Hit() {
        Monster_Base monster = target.GetComponent<Monster_Base>();
        
        monster.OnAttack(attackPower);
        OnDestroy();
    }

    private void OnDestroy() {
        Managers.Resource.Destroy(gameObject);
    }
}
