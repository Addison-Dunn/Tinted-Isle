using UnityEngine;

public class MousePosBehavior : MonoBehaviour
{

    public PointAndClick pnc;

    private void Awake()
    {
        pnc.onClickRelease.AddListener(OnClickRelease);
    }

    void OnClickRelease()
    {
        if(transform.childCount != 0)
        {
            ItemSeed heldItem = GetComponentInChildren<ItemSeed>();
            Collider2D hoveredContainer = Physics2D.OverlapPoint(transform.position);
            if (hoveredContainer == null)
            {
                heldItem.SendToContainer(heldItem.lastContainer);
            }
        }

    }
}
