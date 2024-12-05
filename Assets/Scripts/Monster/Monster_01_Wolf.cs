using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfHashingValue
{
    public static readonly int RunStringHash = Animator.StringToHash("Run");
    public static readonly int AttackStringHash = Animator.StringToHash("Attack");
    public static readonly int RoarStringHash = Animator.StringToHash("Roar");
}

public class Monster_01_Wolf : Monster_Base
{
    protected override void Init() {
        base.Init();
        
    }

    protected override void Update() {
        base.Update();
    }

    protected override void Run() {
        _animator.CrossFade(WolfHashingValue.RunStringHash, 0.1f, 0);
    }

    protected override void Attack() {
        base.Attack();
        
        var stateInfo = _animator?.GetCurrentAnimatorStateInfo(0);
        if (stateInfo?.fullPathHash == WolfHashingValue.RunStringHash ||
            stateInfo?.shortNameHash == WolfHashingValue.RunStringHash) {
            _animator.CrossFade(WolfHashingValue.AttackStringHash, 0.1f, 0);
        }
    }

    // TODO
    protected override void Win() {
        var stateInfo = _animator?.GetCurrentAnimatorStateInfo(0);
        if (stateInfo?.fullPathHash == WolfHashingValue.AttackStringHash ||
            stateInfo?.shortNameHash == WolfHashingValue.AttackStringHash ||
            stateInfo?.fullPathHash == WolfHashingValue.RunStringHash ||
            stateInfo?.shortNameHash == WolfHashingValue.RunStringHash) {
            _animator.CrossFade(WolfHashingValue.RoarStringHash, 0.1f, 0);
        }
    }
    
}
