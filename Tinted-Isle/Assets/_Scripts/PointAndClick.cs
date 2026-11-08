using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PointAndClick : MonoBehaviour
{
    public GameObject mousePosGO;
    public InputActionAsset inputAction;
    private InputAction clickAction;
    private InputAction rightClickAction;

    public Camera mainCamera;

    public UnityEvent onClickPush;
    public UnityEvent onClickRelease;
    public UnityEvent onRightClickPush;
    public UnityEvent onRightClickRelease;
    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        rightClickAction = InputSystem.actions.FindAction("RightClick");

    }
    private void Update()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosGO.transform.position = mouseWorldPos;
        if (clickAction.WasPressedThisFrame())
        {
            onClickPush.Invoke();
        }
        if (clickAction.WasReleasedThisFrame())
        {
            onClickRelease.Invoke();
        }

        if (rightClickAction.WasPressedThisFrame())
        {
            onRightClickPush.Invoke();
        }
        if (rightClickAction.WasReleasedThisFrame())
        {
            onRightClickRelease.Invoke();
        }
    }



}
