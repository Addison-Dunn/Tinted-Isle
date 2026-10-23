using TMPro;
using UnityEngine;

public enum eFlower { red, green, yellow };


public class ItemSeed : MonoBehaviour
{
    [Header("Count text")]
    public TMP_FontAsset unMaskedFont;
    public TMP_FontAsset maskedFont;

    public TextMeshPro countText;
    public int maxCount;
    public int count;

    [HideInInspector]public GameObject lastContainer;

    [Header("Sprites")]

    public SpriteRenderer sr;

    [Header("FlowerType")]
    public eFlower flowerType;

    [Header("Seperate prefab for the planted flower")]
    public GameObject flowerPlanted;

    private void Start()
    {
        lastContainer = transform.parent.gameObject;
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
        if(hs != null)
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

    public void GetPlanted()
    {

    }

    public void UpdateCountText()
    {
        if(count > 1)
        {
            countText.text = "" + count;
        }
        else
        {
            countText.text = "";
        }
    }

    public void Grow(int amount)
    {
        if (amount > 0)
        {

        }
    }

}
