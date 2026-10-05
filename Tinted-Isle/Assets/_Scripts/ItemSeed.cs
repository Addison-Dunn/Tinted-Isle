using UnityEngine;

public class ItemSeed : MonoBehaviour
{
    public GameObject lastContainer;

    public SpriteRenderer sr;

    public Sprite growth1;
    public Sprite growth2;
    public Sprite growth3;
    public Sprite growth4;

    bool planted;

    private void Awake()
    {
        lastContainer = transform.parent.gameObject;
    }

    public void SendToContainer(GameObject targetContainer)
    {
        transform.parent = targetContainer.transform;
        transform.localPosition = Vector2.zero;
        Vector3 itemPos = transform.position;
        itemPos.z = -3;
        transform.position = itemPos;
    }

    public void GetPlanted()
    {
        sr.sprite = growth1;
        planted = true;
    }

}
