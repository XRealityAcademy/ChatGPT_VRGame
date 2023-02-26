using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    [SerializeField]
    //dosen't want the initialState be accessible by other script
    //knows which state is and change it
    private BaseState initialState;
    public BaseState currentState;

    void Awake()
    {
        currentState = initialState;
    }

    private void Update()
    {
        Execute();
    }

    public virtual void Execute()
    {
        //if(currentState(!null) -> currentState.Execute();
        currentState?.Execute(this);
        

    }


}
