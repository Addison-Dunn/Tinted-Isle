using UnityEngine;

public class Container : MonoBehaviour
{
    public Item queuedItem;
    public Item containedItem;

    private PointSensor ps;
    private PointAndClick pnc;
    private GameObject mouse;
    public bool hovered => ps.hovered;


    public virtual void Awake()
    {
        pnc = ps.mousePosGO.transform.GetComponentInParent<PointAndClick>();

        pnc.onClickPush.AddListener(OnClick);
        pnc.onClickRelease.AddListener(OnClickRelease);
        mouse = ps.mousePosGO;
    }

    public virtual void OnClick()
    {
        if (hovered)
        {
            #region drop item into container
            if (ps.mousePosGO.transform.childCount > 0)
            {
                Item heldItem = mouse.GetComponentInChildren<Item>();

                if (containedItem == null)
                {
                    // if no contained item, just send the item to me
                }
                // if I have an item
                else if (containedItem != null)
                {
                    // if the item is the same as the item being dropped
                    if (containedItem.identifier == heldItem.identifier)
                    {
                        // if the item I have has room for more
                        if (containedItem.count < containedItem.maxCount)
                        {

                        }
                    }
                    else
                    // otherwise, swap the item im holding with the item the mouse is holding
                    {

                    }
                }
            }
            #endregion

            #region pickup item from container
            if (ps.mousePosGO.transform.childCount != 0)
            {

            }
            #endregion
        }

    }

    void OnClickRelease()
    {

    }
}
