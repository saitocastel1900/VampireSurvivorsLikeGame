using System;
using UniRx;
using UnityEngine.InputSystem;

public static class InputSystemExtension
{
    public static IObservable<InputAction.CallbackContext> GetStartedObservable(this InputAction inputAction)
    {
        return Observable.FromEvent<InputAction.CallbackContext>(
            h => inputAction.started += h,
            h => inputAction.started -= h);
    }
    
    public static IObservable<InputAction.CallbackContext> GetPerformedObservable(this InputAction inputAction)
    {
        return Observable.FromEvent<InputAction.CallbackContext>(
            h => inputAction.performed += h,
            h => inputAction.performed -= h);
    }
    
    public static IObservable<InputAction.CallbackContext> GetCanceldObservable(this InputAction inputAction)
    {
        return Observable.FromEvent<InputAction.CallbackContext>(
            h => inputAction.canceled += h,
            h => inputAction.canceled -= h);
    }
}