using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FlowerPlanted : MonoBehaviour
{
    [Header("Growth Stages")]
    public Sprite growth1;
    public Sprite growth2;
    public Sprite growth3;
    public Sprite growth4;

    private SpriteRenderer sr;


    [Header("Grow Effect")]
    public float growEffectTime;
    public AnimationCurve growEffectEasing;
    public AnimationCurve growShakeEasing;
    public AnimationCurve regressEffectEasing;


    public float growShakeMag;
    public float growShakeDur;

    [Header("Growth Functionality")]
    public float timeUntilNextGrowth;
    public int remainingGrowth;
    public int currentGrowth = 1;
    public int targetGrowth;

    [Header("Growth attributes")]
    public float maxGrowthChance = 2;
    public float minGrowthChance = -1;
    public float dryMult = 1;
    public float wetMult = 1;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void InitializeGrowth(int amount)
    {
        targetGrowth = currentGrowth + amount;
        remainingGrowth = targetGrowth - currentGrowth;
        GameManager gm = GameManager.Instance;
        timeUntilNextGrowth = Random.Range(gm.elapsedTime,gm.elapsedTime + (gm.dayLength - gm.elapsedTime)/ Mathf.Abs(remainingGrowth));
    }

    public void Grow(int amount)
    {
        currentGrowth = Mathf.Clamp(currentGrowth + amount, 1, 4);
        if(amount > 0) StartCoroutine(GrowEffect());
        if(amount<0) StartCoroutine(RegressEffect());
        UpdateGrowth();
    }

    private void Update()
    {
        if(remainingGrowth != 0)
        {
            timeUntilNextGrowth -= Time.deltaTime;
            int growDir = remainingGrowth > 0 ? 1: -1;
            if(timeUntilNextGrowth <= 0)
            {
                remainingGrowth += growDir;
                Grow(growDir);
                GameManager gm = GameManager.Instance;
                timeUntilNextGrowth = Random.Range(gm.elapsedTime, gm.elapsedTime + (gm.dayLength - gm.elapsedTime) / Mathf.Abs(remainingGrowth));
            }
        }
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
        StartCoroutine(Shake(growShakeDur, growShakeMag));
        StartCoroutine(RegressEffect());
    }

    public void PickUp()
    {

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
    public IEnumerator RegressEffect()
    {
        Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);
        float elapsed = 0;
        while (elapsed < growEffectTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / growEffectTime;
            t = regressEffectEasing.Evaluate(t);
            transform.localScale = new Vector3(t, t, t);
            //transform.localScale = Vector3.Lerp(scale, new Vector3(1, 1, 1), t);
            yield return null;
        }

    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0;
        Vector3 originalLocalPos = transform.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector2 randomOffset = Random.insideUnitCircle * (magnitude * growShakeEasing.Evaluate(t));

            Vector3 randomPos = new Vector3(randomOffset.x + originalLocalPos.x, randomOffset.y + originalLocalPos.y, originalLocalPos.z);
            transform.localPosition = randomPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPos;
    }
}
