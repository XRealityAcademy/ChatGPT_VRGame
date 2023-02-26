using UnityEngine;

public class BaseState : ScriptableObject
{
    //BaseState allows to create new state
    //A template for each state
    //abstract: cannot create Base Class on its own
    public virtual void Execute(BaseStateMachine baseStateMachine)
    {
        //default function
        //virtual needs to provide body.
        Debug.Log("Excuting the BaseState");
    }


}
