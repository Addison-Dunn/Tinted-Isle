using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]public UnityEvent updateGrowth;
    [Header("Day Attributes")]
    public bool startDayOnAwake;
    public float dayLength;
    public GameObject Garden;
    bool dayInProgress;

    [Header("Clock")]
    public GameObject clockPointer;
    public Vector2 pointerStartPos;
    public Vector2 pointerEndPos;

    public float growthTickRate;
    public float elapsedTime;

    private void Awake()
    {
        Instance = this;
        if (startDayOnAwake)
        {
            DayStart();
        }
    }
    private void Update()
    {

        if (dayInProgress)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / dayLength;
            clockPointer.transform.localPosition = Vector2.Lerp(pointerStartPos, pointerEndPos, t);
            if (elapsedTime >= dayLength)
            {
                elapsedTime = 0;

                DayStart();
            }
        }

        //if(elapsedTime >= growthTickRate)
        //{
        //    elapsedTime = 0;
        //    updateGrowth?.Invoke();
        //}
    }
    void DayStart()
    {
        updateGrowth?.Invoke();
        dayInProgress = true;
    }
}
