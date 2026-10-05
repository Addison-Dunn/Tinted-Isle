using UnityEngine;
using UnityEngine.InputSystem;

public class PointSensor : MonoBehaviour
{
    public GameObject mousePosGO;
    public BoxCollider2D bc;
    public bool hovered;

    private void Update()
    {

        if (bc.OverlapPoint(mousePosGO.transform.position))
        {
            hovered = true;
        }
        else
        {
            hovered = false;
        }
    }
}
