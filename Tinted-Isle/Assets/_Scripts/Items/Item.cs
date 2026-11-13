using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Item : MonoBehaviour
{
    [Header("Inherited")]
    public string Identifier;
    [Header("Count text")]
    public TMP_FontAsset unMaskedFont;
    public TMP_FontAsset maskedFont;

    public TextMeshPro countText;
    public int maxCount;
    public int count;
    [HideInInspector] public int queuedCount;

    [HideInInspector] public GameObject lastContainer;

    [Header("Sprites")]

    public SpriteRenderer sr;

    [Header("Dropped Item")]
    public AnimationCurve easing;
    public bool droppedItem;
    bool dropped;

    private void Start()
    {
        if(transform.parent != null) lastContainer = transform.parent.gameObject;
        UpdateCountText();
    }

    public void SendToContainer(GameObject targetContainer)
    {
        transform.parent = targetContainer.transform;
        transform.localPosition = Vector2.zero;
        Vector3 itemPos = transform.localPosition;
        itemPos.z = -0.1f;
        transform.localPosition = itemPos;

        HotbarSlot hs = targetContainer.GetComponent<HotbarSlot>();
        if (hs != null)
        {
            if (hs.inventorySlot)
            {
                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                countText.font = maskedFont;
            }
            else
            {
                sr.maskInteraction = SpriteMaskInteraction.None;
                countText.font = unMaskedFont;
            }
        }
    }

    public void UpdateCountText()
    {
        if (count > 1)
        {
            countText.text = "" + count;
        }
        else
        {
            countText.text = "";
        }
        queuedCount = count;
    }

    public GameObject FindAvailableContainer()
    {
        GameObject container = null;
        GameObject firstEmptyContainer = null;
        int index = 0;
        while (index < GameManager.Instance.slots.Count)
        {
            HotbarSlot thisHotbarSlot = GameManager.Instance.slots[index];

            if(firstEmptyContainer == null && thisHotbarSlot.transform.childCount == 0)
            {
                if((thisHotbarSlot.queuedItem != null && thisHotbarSlot.queuedItem.Identifier == Identifier) ||
                   thisHotbarSlot.queuedItem == null)
                {
                    firstEmptyContainer = thisHotbarSlot.gameObject;
                }
            }

            if(thisHotbarSlot.transform.childCount != 0)
            {
                Item item = thisHotbarSlot.GetComponentInChildren<Item>();
                if(item.Identifier == Identifier && item.queuedCount < item.maxCount - 1)
                {
                    item.queuedCount += count;
                    container = thisHotbarSlot.gameObject;
                    index = GameManager.Instance.slots.Count;
                }
            }
            index++;
        }

        if(container == null)
        {
            HotbarSlot hotbarSlot = firstEmptyContainer.GetComponent<HotbarSlot>();
            if(hotbarSlot.queuedItem == null) hotbarSlot.queuedItem = this;
            return firstEmptyContainer;
        }
        else
        {
            return container;
        }
    }

    public IEnumerator SendToContainerLerped(GameObject targetContainer, float duration, bool sendingToHotbarSlot)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -3);
        Vector3 startPos = transform.position;
        Vector3 endPos = targetContainer.transform.position;
        if (!ButtonInventory.Instance.open && !sendingToHotbarSlot)
        {
            endPos = ButtonInventory.Instance.transform.position;
        }
        startPos.z = -2.25f;
        endPos.z = -2.25f;

        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = easing.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;

        targetContainer.TryGetComponent<HotbarSlot>(out HotbarSlot hotbarSlot);
        targetContainer.TryGetComponent<GardenPlot>(out GardenPlot gardenPlot);

        droppedItem = false;
        bool destroyMe = false;
        if (hotbarSlot != null || gardenPlot != null)
        {
            int childCountCheck;
            childCountCheck = hotbarSlot != null ? 0 : 1;

            //if the target container is empty
            if (targetContainer.transform.childCount == childCountCheck)
            {
                SendToContainer(targetContainer);
            }
            else
            {
                Item containedItem = targetContainer.GetComponentInChildren<Item>();
                containedItem.count += count;
                containedItem.UpdateCountText();
                destroyMe = true;
            }
        }

        yield return StartCoroutine(SendItemToInventoryEffect(gameObject, 0.3f, sendingToHotbarSlot, destroyMe));

        if (hotbarSlot != null || gardenPlot != null)
        {
            int childCountCheck;
            childCountCheck = hotbarSlot != null ? 0 : 1;

            //if the target container is empty
            if (targetContainer.transform.childCount == childCountCheck)
            {
            }
            else
            {
                //Item containedItem = targetContainer.GetComponentInChildren<Item>();
                ////containedItem.count += count;
                //containedItem.UpdateCountText();
                //Destroy(gameObject);
            }
        }
    }

    public IEnumerator DropItem(float magnitude, float duration)
    {
        sr.maskInteraction = SpriteMaskInteraction.None;
        countText.font = unMaskedFont;

        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + Random.insideUnitCircle * magnitude;

        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = easing.Evaluate(t);

            transform.position = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;
        yield return new WaitForSeconds(0.2f);

        GameObject targetContainer = FindAvailableContainer();
        targetContainer.TryGetComponent<HotbarSlot>(out HotbarSlot hotbarSlot);

        if (targetContainer != null) StartCoroutine(SendToContainerLerped(targetContainer, 0.5f, hotbarSlot != null && !hotbarSlot.inventorySlot));
    }

    public IEnumerator SendItemToInventoryEffect(GameObject targetItem, float duration, bool targetIsHotbar, bool destroyAtEnd)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scale = 1;
            if (ButtonInventory.Instance.open)
            {
                scale = Library.Instance.itemBounce.Evaluate(t);
            }
            else
            {
                if (targetIsHotbar)
                {
                    scale = Library.Instance.itemBounce.Evaluate(t);
                }
                else
                {
                    scale = Library.Instance.itemShrink.Evaluate(t);
                }
            }
            targetItem.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        targetItem.transform.localScale = new Vector3(1, 1, 1);

        if (destroyAtEnd) Destroy(gameObject);
    }

}
