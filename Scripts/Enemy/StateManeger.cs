using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManeger : MonoBehaviour
{
    public State currentState;
    
    void Update()
    {
        runStateMachine();
    }
    private void runStateMachine()
    {
        State nextState = currentState?.runCurrentState();
        if(nextState != null)
        {
            switchToTheNextState(nextState);
        }
    }
    private void switchToTheNextState(State nextState)
    {
        currentState = nextState;
    }
}
