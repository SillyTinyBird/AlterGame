using System;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-2)]
public class InputManager : Singleton<InputManager>
{
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    private Controls _controls;
    private void Awake()
    {
        _controls = new();
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();
    void Start()
    {
        _controls.Touch.IsInContact.started += context => StartTouch(context);
        _controls.Touch.IsInContact.canceled += context => EndTouch(context);
    }
    private void StartTouch(InputAction.CallbackContext context)
    {
        OnStartTouch?.Invoke(CameraToWorldPosition.ScreenToWorld(_controls.Touch.Position.ReadValue<Vector2>()), (float)context.startTime);
    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        OnEndTouch?.Invoke(CameraToWorldPosition.ScreenToWorld(_controls.Touch.Position.ReadValue<Vector2>()), (float)context.time);
    }
}
