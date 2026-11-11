using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
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

    [Header("Inventory")]
    public List<Transform> rows = new List<Transform>();
    public List<HotbarSlot> slots = new List<HotbarSlot>();

    private void Awake()
    {
        foreach(Transform row in rows)
        {
            HotbarSlot[] childSlots = row.GetComponentsInChildren<HotbarSlot>();
            foreach(HotbarSlot hotbarSlot in childSlots)
            {
                slots.Add(hotbarSlot);
            }
        }
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
