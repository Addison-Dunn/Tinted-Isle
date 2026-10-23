using Unity.VisualScripting;
using UnityEngine;


public class HotbarSlot : MonoBehaviour
{
    public PointSensor ps;
    bool hovered => ps.hovered;

    private PointAndClick pnc;

    public bool inventorySlot;
    public bool visible;
    private void Awake()
    {
        visible = inventorySlot ? false : true;

        pnc = ps.mousePosGO.transform.GetComponentInParent<PointAndClick>();

        pnc.onClick.AddListener(OnClick);
        pnc.onClickRelease.AddListener(OnClickRelease);
    }

    void OnClick()
    {
        // pickup out of slot
        if (visible)
        {
            if (ps.mousePosGO.transform.childCount == 0)
            {
                if (hovered && gameObject.transform.childCount != 0)
                {
                    ItemSeed containedItem;
                    containedItem = GetComponentInChildren<ItemSeed>();
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
            else // drop into slot
            {
                if (hovered && ps.mousePosGO.transform.childCount != 0)
                {
                    ItemSeed mouseItem = ps.mousePosGO.GetComponentInChildren<ItemSeed>();

                    if (gameObject.transform.childCount == 0)
                    {
                        mouseItem.SendToContainer(this.gameObject);
                    }
                    else
                    {
                        ItemSeed containedItem = GetComponentInChildren<ItemSeed>();

                        if (containedItem.flowerType == mouseItem.flowerType)
                        {
                            if ((containedItem.count + mouseItem.count) > containedItem.maxCount)
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
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inventorySlot && collision.CompareTag("InventorySpriteMask"))
        {
            visible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (inventorySlot && collision.CompareTag("InventorySpriteMask"))
        {
            visible = false;
        }
    }
    void OnClickRelease()
    {
        

    }

}
