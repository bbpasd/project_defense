using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandState
{
    NormalModeState,
    BuildModeState    
}

public class CommandStateMachine : StateMachine<CommandState>
{
    protected override void Awake() {
        foreach (CommandState state in Enum.GetValues(typeof(CommandState))) {
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
