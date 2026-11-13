using UnityEngine;

public class Library : MonoBehaviour
{
    public static Library Instance;

    public AnimationCurve flowerPlaceShake;
    public AnimationCurve flowerGrow;
    public AnimationCurve flowerRegress;
    public AnimationCurve itemBounce;
    public AnimationCurve itemShrink;
    
    private void Awake()
    {
        Instance = this;
    }
}
