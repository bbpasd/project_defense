using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public abstract class Building_Base : Object_Base
{
    public float heightPos = 5.0f;
    

    // 공격 관련
    public GameObject weaponPrefab;
    
    public bool canAttack;
    public GameObject weaponSpawnPoint;
    public Vector3 weaponSpawnPos;
    public float range;
    public SphereCollider attackCollider;

    public bool isCoolTime = false;
    public float coolTime = 3.0f;
    public GameObject target;

    public List<GameObject> targetList;
    
    // 업그레이드 관련
    
    public float upgradeAttackPowerAmount = 5.0f;
    public float upgradeMaxHPAmount = 100.0f;
    public float upgradeScalePercentageAmount = 1.0f; // 스케일 보류 
    public float initialScale;

    private void Awake() {
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine() {
        yield return new WaitForSeconds(0.1f);
        Init();
    }

    protected virtual void Init() {
        currentHP = maxHP;

        if (canAttack) {
            targetList = new();
            if (weaponSpawnPoint != null) {
                weaponSpawnPos = weaponSpawnPoint.transform.position;
                // weaponSpawnPos = transform.position + new Vector3(0, 10, 0);    // TODO
            }
            // Debug.Log(transform.position);
            // Debug.Log(weaponSpawnPoint.transform.position);
            SetAttackRange(range);
            SetAttackColliderActive(false);
        }

        initialScale = transform.localScale.x;
    }

    protected virtual void Update() {
        if (target == null) {
            SetNewTarget();
        }

        if (target != null) {
            if (!isCoolTime) {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    public override void OnAttack(float amount) {
        // Debug.Log($"OnAttack! {amount}");
        currentHP -= amount;
        base.OnAttack(amount);
        if (currentHP <= 0) {
            currentHP = 0;
            Destroy();
        }
    }

    protected virtual void Attack() {
        if (weaponPrefab == null) {
            Debug.Log($"{gameObject.name} Weapon Missing");
            return;
        }

        Weapon_Base wb = Managers.Resource.Instantiate(weaponPrefab, weaponSpawnPos).GetComponent<Weapon_Base>();
        wb.attackPower = attackPower;
        wb.target = target;
        wb.owner = this;
    }
    
    IEnumerator AttackCoroutine() {
        isCoolTime = true;
        // 공격
        Attack();
        yield return new WaitForSeconds(coolTime);
        isCoolTime = false;
    }

    public virtual void Destroy() {
        Debug.Log($"{gameObject.name} 파괴");
        Managers.Resource.Destroy(gameObject);
    }

    public virtual void SetAttackColliderActive(bool active) {
        if (!attackCollider) return;
        attackCollider.enabled = active;
    }

    public virtual void SetAttackRange(float amount) {
        range = amount;
        if (!attackCollider) return;

        attackCollider.radius = range;
    }

    public virtual void SetNewTarget() {
        targetList = targetList.Where(t => t != null).ToList();
        if (targetList.Count > 0) {
            target = targetList[0]; // TODO 현재 무조건 1번 인덱스를 타겟으로 잡음
        }
        else target = null;
    }

    public virtual void UpgradeBuilding() {
        upgradeLevel++;

        if (canAttack) {
            attackPower += upgradeAttackPowerAmount;    
        }
        currentHP += upgradeMaxHPAmount;
        maxHP += upgradeMaxHPAmount;

        float scaleAmount = initialScale + (upgradeScalePercentageAmount / 100.0f * upgradeLevel);
        transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
        
        if (this == MyPlayerController.Instance.selectedObject) {
            MyPlayerController.Instance.SetSelectObject(this);
        }

        if (this == MyPlayerController.Instance.hqBuilding) {
            MyPlayerController.Instance.SetHQ();
        }
        
    }

    #region Trigger Collider
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Monster")) {
            targetList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Monster")) {
            if (targetList.Contains(other.gameObject)) {
                targetList.Remove(other.gameObject);
            }

            if (other.gameObject == target) {
                SetNewTarget();
            }
        }
    }
    #endregion
}
