using UnityEngine;
using UnityEngine.InputSystem;

public class GardenPlot : MonoBehaviour
{
    public Sprite covered;
    public Sprite normal;
    public Sprite dry1;
    public Sprite dry2;
    public Sprite wet1;
    public Sprite wet2;

    private InputAction clickAction;

    private PointSensor ps;
    public bool hovered => ps.hovered;

    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        ps = GetComponent<PointSensor>();
    }

    private void Update()
    {
        if (hovered && clickAction.WasPressedThisFrame())
        {
            Debug.Log(name + " was clicked");
        }
    }

}
