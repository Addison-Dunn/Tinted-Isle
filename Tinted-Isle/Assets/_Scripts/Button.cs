using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PointSensor))]
public class Button : MonoBehaviour
{
    [SerializeField] private UnityEvent onButtonPress;

    public PointSensor ps;
    bool hovered => ps.hovered;

    private PointAndClick pnc;
    private void Awake()
    {
        pnc = ps.mousePosGO.transform.GetComponentInParent<PointAndClick>();
        pnc.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (hovered)
        {
            onButtonPress?.Invoke();
        }
    }
}
