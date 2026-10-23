using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    [HideInInspector]public UnityEvent updateGrowth;

    public float growthTickRate;
    public float elapsedTime;
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= growthTickRate)
        {
            elapsedTime = 0;
            updateGrowth?.Invoke();
        }
    }
}
