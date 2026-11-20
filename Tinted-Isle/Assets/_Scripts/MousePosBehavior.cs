using UnityEngine;

public class MousePosBehavior : MonoBehaviour
{
    public static MousePosBehavior Instance;
    public PointAndClick pnc;
    public GameObject hoveredGardenPlot;
    public Collider2D hoveredCol;
    private void Awake()
    {
        Instance = this;
        pnc.onClickRelease.AddListener(OnClickRelease);
    }

    private void Update()
    {
        hoveredCol = Physics2D.OverlapPoint(transform.position);
        if (hoveredCol != null && hoveredCol.TryGetComponent<GardenPlot>(out GardenPlot hoveredGP))
        {
            if(hoveredGP != null)hoveredGardenPlot = hoveredCol == null ? null : hoveredCol.gameObject;
        }
        else if(hoveredCol == null)
        {
            hoveredGardenPlot = null;
        }
    }
    void OnClickRelease()
    {
        if(transform.childCount != 0)
        {
            Item heldItem = GetComponentInChildren<Item>();
            Collider2D hoveredContainer = Physics2D.OverlapPoint(transform.position);
            if (hoveredContainer == null)
            {
                heldItem.SendToContainer(heldItem.lastContainer);
                heldItem.held = false;
            }
        }

    }
}
