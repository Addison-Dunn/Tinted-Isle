using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
public class PointAndClick : MonoBehaviour
{
    public InputActionAsset inputAction;

    private InputAction clickAction;

    public Camera mainCamera;
    private void Update()
    {
        Point();

        if (clickAction.WasPressedThisFrame())
        {
            Click();
        }
    }

    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
    }

    public PointSensor lastHitSensor;
    void Point()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);

        Vector3 direction = (mousePos - mainCamera.transform.position);
        direction.z -= mainCamera.transform.position.z;
        Debug.DrawRay(mainCamera.transform.position, direction, Color.blue);

        Collider2D hit = Physics2D.OverlapPoint(mousePos,LayerMask.GetMask("UI"));
        if (hit != null)
        {
            lastHitSensor = hit.gameObject.GetComponent<PointSensor>();
            lastHitSensor.hovered = true;
        }
        else
        {
            if(lastHitSensor != null) lastHitSensor.hovered = false;
        }

    }
    public void Click()
    {

    }
}
