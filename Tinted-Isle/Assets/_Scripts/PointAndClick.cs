using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
public class PointAndClick : MonoBehaviour
{
    public InputActionAsset inputAction;
    private InputAction clickAction;

    private void Update()
    {

        if (clickAction.WasPressedThisFrame())
        {
            Click();
        }
    }

    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
    }
    public void Click()
    {

    }
}
