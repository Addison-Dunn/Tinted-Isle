using System.Collections;
using TMPro;
using UnityEngine;

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
            HotbarSlot hotbarSlot = GameManager.Instance.slots[index];

            if(!hotbarSlot.droppedItemQueued && firstEmptyContainer == null && hotbarSlot.transform.childCount == 0)
            {
                firstEmptyContainer = hotbarSlot.gameObject;
            }

            if(hotbarSlot.transform.childCount != 0)
            {
                Item item = hotbarSlot.GetComponentInChildren<Item>();
                if(item.Identifier == Identifier && item.queuedCount <= item.maxCount - count)
                {
                    item.queuedCount += count;
                    container = hotbarSlot.gameObject;
                    index = GameManager.Instance.slots.Count;
                }
            }
            index++;
        }

        if(container == null)
        {
            HotbarSlot hotbarSlot = firstEmptyContainer.GetComponent<HotbarSlot>();
            hotbarSlot.droppedItemQueued = true;
            return firstEmptyContainer;
        }
        else
        {
            return container;
        }
    }

    public IEnumerator SendToContainerLerped(GameObject targetContainer, float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = targetContainer.transform.position;
        endPos.z = -0.1f;
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
        targetContainer.TryGetComponent<HotbarSlot>(out HotbarSlot hotbarSlot);
        targetContainer.TryGetComponent<GardenPlot>(out GardenPlot gardenPlot);
        if (hotbarSlot != null) hotbarSlot.droppedItemQueued = false;
        droppedItem = false;
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
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator DropItem(float magnitude, float duration)
    {
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
        if (targetContainer != null) StartCoroutine(SendToContainerLerped(targetContainer, 0.5f));
    }
}
