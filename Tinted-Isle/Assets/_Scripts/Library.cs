using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Library : MonoBehaviour
{
    public static Library Instance;

    public AnimationCurve flowerPlaceShake;
    public AnimationCurve flowerGrow;
    public AnimationCurve flowerRegress;
    public AnimationCurve itemBounce;
    public AnimationCurve itemShrink;

    public TMP_FontAsset unMaskedFont;
    public TMP_FontAsset maskedFont;
    private void Awake()
    {
        Instance = this;
    }
}
