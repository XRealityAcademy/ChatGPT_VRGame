
using UnityEngine;

public abstract class FSMAction : ScriptableObject
{
    //FSMAction: template of a function of any state
    //abstract = interface (even more basic set of instruction)
    
    public abstract void Execute(BaseStateMachine baseStateMachine);





}
