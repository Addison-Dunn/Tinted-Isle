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

    public Sprite growth1;
    public Sprite growth2;
    public Sprite growth3;
    public Sprite growth4;

    bool planted;

    [Header("FlowerType")]
    public eFlower flowerType;

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
        sr.sprite = growth1;
        planted = true;
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

}
