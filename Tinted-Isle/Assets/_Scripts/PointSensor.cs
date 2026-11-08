using UnityEngine;
using UnityEngine.InputSystem;

public class PointSensor : MonoBehaviour
{
    public GameObject mousePosGO;
    public BoxCollider2D bc;
    public bool hovered;
    private SpriteRenderer sr;
    public bool hoverable;

    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out sr);
    }

    private void Update()
    {
        if (hoverable)
        {
            if (bc.OverlapPoint(mousePosGO.transform.position))
            {
                hovered = true;
                if (sr != null) sr.enabled = true;
            }
            else
            {
                hovered = false;
                if (sr != null) sr.enabled = false;

            }
        }
        else
        {
            hovered = false;
            if (sr != null) sr.enabled = false;

        }
    }
}
