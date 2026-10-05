using UnityEngine;

public class HotbarSlot : MonoBehaviour
{
    public PointSensor ps;
    bool hovered => ps.hovered;

    private PointAndClick pnc;

    private void Awake()
    {

        pnc = ps.mousePosGO.transform.GetComponentInParent<PointAndClick>();

        pnc.onClick.AddListener(OnClick);
        pnc.onClickRelease.AddListener(OnClickRelease);
    }

    void OnClick()
    {
        if(hovered && gameObject.transform.childCount != 0)
        {
            ItemSeed containedItem;
            containedItem = GetComponentInChildren<ItemSeed>();
            containedItem.gameObject.transform.parent = ps.mousePosGO.transform;
            containedItem.gameObject.transform.localPosition = Vector2.zero;
            Vector3 itemPos = containedItem.gameObject.transform.position;
            itemPos.z = -3;
            containedItem.gameObject.transform.position = itemPos;
            containedItem.lastContainer = this.gameObject;
            containedItem = null;
        }
    }
    void OnClickRelease()
    {
        if(hovered && ps.mousePosGO.transform.childCount != 0)
        {
            ItemSeed mouseItem = ps.mousePosGO.GetComponentInChildren<ItemSeed>();

            if (gameObject.transform.childCount == 0)
            {
                mouseItem.SendToContainer(this.gameObject);
            }
            else
            {
                mouseItem.SendToContainer(mouseItem.lastContainer);
            }

        }

    }

}
