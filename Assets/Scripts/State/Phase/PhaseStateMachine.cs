using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhaseState
{
    PhaseStayState,
    Phase01State    
}

public class PhaseStateMachine : StateMachine<PhaseState>
{
    protected override void Awake() {
        foreach (PhaseState state in Enum.GetValues(typeof(PhaseState))) {
            string className = state.ToString();
            Type stateType = Type.GetType(className);
        
            if (stateType != null) {
                gameObject.AddComponent(stateType);
            }
            else {
                Debug.LogWarning($"{GetType().Name} | State class '{className}' not found.");
            }
        }
    
        base.Awake();
    }

}