using UnityEngine;

public class FlowerPlanted : MonoBehaviour
{
    [Header("Growth Stages")]
    public Sprite growth1;
    public Sprite growth2;
    public Sprite growth3;
    public Sprite growth4;

    private SpriteRenderer sr;
    public int currentGrowth;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void progressGrowth(int amount)
    {
        currentGrowth = Mathf.Clamp(amount + currentGrowth, 1, 4);
        UpdateGrowth();
        
    }

    void UpdateGrowth()
    {
        switch (currentGrowth)
        {
            case 2:
                sr.sprite = growth2;
                break;
            case 3:
                sr.sprite = growth3;
                break;
            case 4: sr.sprite = growth4;
                break;
            default:
                sr.sprite = growth1;
                break;
        }
    }

    public void SendToGardenPlot(GameObject targetContainer)
    {
        transform.parent = targetContainer.transform;
        transform.localPosition = Vector2.zero;
        Vector3 itemPos = transform.localPosition;
        itemPos.z = -0.1f;
        transform.localPosition = itemPos;
    }

}
