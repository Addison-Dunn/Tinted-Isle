using Unity.VisualScripting;
using UnityEngine;


public class HotbarSlot : MonoBehaviour
{
    public PointSensor ps;
    bool hovered => ps.hovered;
    private PointAndClick pnc;

    public bool inventorySlot;
    public Item queuedItem;
    private void Awake()
    {
        pnc = ps.mousePosGO.transform.GetComponentInParent<PointAndClick>();

        pnc.onClickPush.AddListener(OnClick);
        pnc.onClickRelease.AddListener(OnClickRelease);
    }

    void OnClick()
    {
        // pickup out of slot
        if (ps.mousePosGO.transform.childCount == 0)
        {
            if (hovered && gameObject.transform.childCount != 0)
            {

                Item containedItem = GetComponentInChildren<Item>();

                queuedItem = containedItem;

                containedItem.held = true;
                containedItem.gameObject.transform.parent = ps.mousePosGO.transform;
                containedItem.gameObject.transform.localPosition = Vector2.zero;
                Vector3 itemPos = containedItem.gameObject.transform.position;
                itemPos.z = -3;
                containedItem.gameObject.transform.position = itemPos;
                containedItem.lastContainer = this.gameObject;
                containedItem.sr.maskInteraction = SpriteMaskInteraction.None;
                containedItem.countText.font = containedItem.unMaskedFont;
                containedItem = null;
            }
        }
        else 
        {
            // drop into slot
            if (hovered && ps.mousePosGO.transform.childCount != 0)
            {
                Item mouseItem = ps.mousePosGO.GetComponentInChildren<Item>();
                mouseItem.held = false;
                // if this container is empty
                if (gameObject.transform.childCount == 0)
                {
                    mouseItem.SendToContainer(this.gameObject);
                }
                else // if this container is not empty
                {
                    Item containedItem = GetComponentInChildren<Item>();

                    if (containedItem.identifier == mouseItem.identifier) // if this container's item is the same as the held item
                    {
                        if ((containedItem.count + mouseItem.count) > containedItem.maxCount) // if this container's item's count is less than its max count
                        {
                            mouseItem.count = (containedItem.count + mouseItem.count) - containedItem.maxCount;

                            containedItem.count = containedItem.maxCount;

                            mouseItem.SendToContainer(mouseItem.lastContainer);
                            containedItem.UpdateCountText();
                            mouseItem.UpdateCountText();
                        }
                        else
                        {
                            containedItem.count += mouseItem.count;
                            containedItem.UpdateCountText();
                            Destroy(mouseItem.gameObject);
                        }
                    }
                    else
                    {
                        mouseItem.SendToContainer(mouseItem.lastContainer);
                    }
                }
                mouseItem.lastContainer.TryGetComponent<HotbarSlot>(out HotbarSlot hotbarSlot);
                if (hotbarSlot != null) hotbarSlot.queuedItem = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inventorySlot && collision.CompareTag("InventorySpriteMask"))
        {
            ps.hoverable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (inventorySlot && collision.CompareTag("InventorySpriteMask"))
        {
            ps.hoverable = false;
        }
    }
    void OnClickRelease()
    {
        

    }

}
