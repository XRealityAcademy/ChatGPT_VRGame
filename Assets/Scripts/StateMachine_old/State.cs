using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/State")]
public sealed class State : BaseState
{
    public List<FSMAction> actions = new List<FSMAction>();
    public List<FSMTransition> transitions = new List<FSMTransition>();
    public override void Execute(BaseStateMachine baseStateMachine)
    {
        foreach (var action in actions)
        {
            action.Execute(baseStateMachine);
   
            
        }
        foreach (var transition in transitions)
        {
            transition.Execute(baseStateMachine);
        }



       


    }


}
