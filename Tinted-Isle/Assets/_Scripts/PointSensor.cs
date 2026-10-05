using UnityEngine;
using UnityEngine.InputSystem;

public class PointSensor : MonoBehaviour
{
    public Camera mainCamera;
    public BoxCollider2D bc;
    public bool hovered;

    private void Update()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (bc.OverlapPoint(mouseWorldPos))
        {
            hovered = true;
        }
        else
        {
            hovered = false;
        }
    }
}
