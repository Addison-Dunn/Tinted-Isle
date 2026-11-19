using UnityEngine;

public class ItemShovel : Item
{
    public override void SendToContainer(GameObject targetContainer)
    {
        if (transform.childCount > 1) return;
        base.SendToContainer(targetContainer);
    }
}
