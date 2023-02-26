using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTransition : ScriptableObject
{
    public FSMDecision decision;
    public BaseState trueState;
    public BaseState falseState;
    public void Execute(BaseStateMachine baseStateMachine)
    {
        if (decision.Decide(baseStateMachine) && !(trueState is RemainInState))
        {
            //trueState is RemainInState => "is" trueState equal RemainInState
            baseStateMachine.currentState = trueState;
        }
        else if (!(falseState is RemainInState))
        {
            baseStateMachine.currentState = falseState;
        }
        
    }


}
