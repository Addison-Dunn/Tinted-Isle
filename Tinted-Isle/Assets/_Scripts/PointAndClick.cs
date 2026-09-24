using UnityEngine;
using UnityEngine.InputSystem;
public class PointAndClick : MonoBehaviour
{
    public Camera mainCamera;
    private void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);

        Vector3 direction = (mousePos - mainCamera.transform.position);
        direction.z += 10;
        Debug.DrawRay(mainCamera.transform.position, direction, Color.blue);
    }
}
