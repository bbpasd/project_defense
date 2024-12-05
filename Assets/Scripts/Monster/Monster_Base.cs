using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

//TODO Monter_Base와 Building_Base의 부모로 Object_Base 생성 (나중에)
public class Monster_Base : Object_Base
{
    public float heightPos = 5.0f;
    // 각종 스탯

    // 공격 관련
    public bool canAttack;
    public float range;
    public bool isCoolTime = false;
    public float coolTime = 1.0f;
    
    // NavMesh
    protected NavMeshAgent _agent;
    public GameObject target;
    public Vector3 nextPos;
    public float navmeshSpeed;
    public Vector3 currentDes;
    
    // Animation
    protected Animator _animator;

    public bool isAlive = true;
    
    // 사망 이벤트
    public delegate void DeathHandler(Monster_Base monster);
    public event DeathHandler OnDeath;

    // TODO
    public bool endOneShot = false;
    
    private void Awake() {
        Init();
    }

    protected virtual void Init() {
        currentHP = maxHP;

        _animator = gameObject.GetComponent<Animator>();
        
        _agent = gameObject.GetOrAddComponent<NavMeshAgent>();
        // _agent.updatePosition = false;
        _agent.updateRotation = false;
        navmeshSpeed = _agent.speed;
        FindTarget();
    }

    protected virtual void Update() {
        if (!isAlive) return;
        
        if (target == null) {
            FindTarget();
        }
        
        if (target != null) {
            _agent.SetDestination(target.transform.position);
            currentDes = _agent.destination;
            
            // 코너 직전 속도 변경 (가속도 제거)
            if (_agent.remainingDistance <= _agent.stoppingDistance + 0.5f) {
                _agent.velocity = Vector3.zero;
            }
            else {
                _agent.velocity = _agent.desiredVelocity;
            }
            
            // 목적지 도착 후 공격
            if (_agent.pathStatus == NavMeshPathStatus.PathComplete && _agent.remainingDistance <= _agent.stoppingDistance) {
                Attack();
            }
            
            
            Vector3 direction = (_agent.destination - transform.position).normalized;
            if (direction != Vector3.zero) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),
                    _agent.angularSpeed * Time.deltaTime);
            }
        }
        else {
            Win();
        }
    }

    protected virtual void FindTarget() {
        if (MyPlayerController.Instance.hqBuilding == null) return;
        target = MyPlayerController.Instance.hqBuilding.gameObject;
        _agent.ResetPath();
        _agent.SetDestination(target.transform.position);
    }

    public virtual void UpdateTarget(Building_Base bb) {
        target = bb.gameObject;
        _agent.SetDestination(bb.transform.position);
    }
    

    protected virtual void Win() {
        // TODO
        if (endOneShot) return;
        endOneShot = true;
        ((UI_GameScene)Managers.UI.SceneUI).DisplayGameOverButton(true);
    }

    protected virtual void Run() {
        
    }

    protected virtual void Attack() {
        if (isCoolTime) return;
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine() {
        isCoolTime = true;
        
        // 공격
        Building_Base building = target.GetComponent<Building_Base>();
        building.OnAttack(attackPower);
        yield return new WaitForSeconds(coolTime);
        
        isCoolTime = false;
    }

    public override void OnAttack(float amount) {
        currentHP -= amount;
        base.OnAttack(amount);
        if (currentHP <= 0 && isAlive) {
            isAlive = false;
            Death();            
        }
    }

    public override void Death() {
        OnDeath?.Invoke(this);
        Managers.Resource.Destroy(gameObject);
    }
}
