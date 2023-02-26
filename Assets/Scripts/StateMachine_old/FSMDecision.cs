
using UnityEngine;

public abstract class FSMDecision : ScriptableObject
{
    //FSMAction: template of a decision
    public abstract bool Decide(BaseStateMachine baseStateMachine);
}
