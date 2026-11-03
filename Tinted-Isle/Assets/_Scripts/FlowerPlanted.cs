using System.Collections;
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

    [Header("Grow Effect")]
    public float growEffectTime;
    public AnimationCurve growEffectEasing;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void progressGrowth(int amount)
    {
        int lastGrowth = currentGrowth;
        currentGrowth = Mathf.Clamp(amount + currentGrowth, 1, 4);

        if (currentGrowth > lastGrowth)
        {
            StartCoroutine(GrowEffect());
        }
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

    public IEnumerator GrowEffect()
    {
        Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);
        float elapsed = 0;
        while (elapsed < growEffectTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / growEffectTime;
            t = growEffectEasing.Evaluate(t);
            transform.localScale = new Vector3(t, t, t);
            //transform.localScale = Vector3.Lerp(scale, new Vector3(1, 1, 1), t);
            yield return null;
        }

    }
}
