using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public event Action<State> OnStateChange;

    private Coroutine switchStateCoroutine;

    public State CurrentState { get; private set; }

    public bool IsCurrentStateInterruptible
    {
        get
        {
            if (CurrentState is MeleeImpactState) { return false; }
            return true;
        }
    }

    public void SwitchState(State newState, float delay = 0.0f)
    {
        if (switchStateCoroutine != null)
        {
            StopCoroutine(switchStateCoroutine);
        }

        if (delay > 0.0f)
        {
            switchStateCoroutine = StartCoroutine(SwitchStateCoroutine(newState, delay));
        }
        else
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
            OnStateChange?.Invoke(CurrentState);
        }
    }

    private IEnumerator SwitchStateCoroutine(State newState, float delay)
    {
        yield return new WaitForSeconds(delay);

        SwitchState(newState);
    }

    private void Update()
    {
        CurrentState?.Tick(Time.deltaTime);
    }
}
