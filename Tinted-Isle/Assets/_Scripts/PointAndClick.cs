using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class PointAndClick : MonoBehaviour
{
    public GameObject mousePosGO;
    public InputActionAsset inputAction;
    private InputAction clickAction;
    public Camera mainCamera;

    public UnityEvent onClick;
    public UnityEvent onClickRelease;


    private void Update()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosGO.transform.position = mouseWorldPos;
        if (clickAction.WasPressedThisFrame())
        {
            onClick.Invoke();
        }
        if (clickAction.WasReleasedThisFrame())
        {
            onClickRelease.Invoke();
        }
    }

    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
    }

}
