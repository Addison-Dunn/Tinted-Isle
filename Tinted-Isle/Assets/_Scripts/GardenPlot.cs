using UnityEngine;
using UnityEngine.InputSystem;

public class GardenPlot : MonoBehaviour
{
    public SpriteRenderer highlightedSr;
    public SpriteRenderer sr;

    public Sprite covered;
    public Sprite normal;
    public Sprite dry1;
    public Sprite dry2;
    public Sprite wet1;
    public Sprite wet2;

    private InputAction clickAction;

    private PointSensor ps;

    private PointAndClick pnc;

    public bool hovered => ps.hovered;

    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        ps = GetComponent<PointSensor>();

        pnc = ps.mousePosGO.transform.GetComponentInParent<PointAndClick>();

        pnc.onClick.AddListener(OnClick);
        pnc.onClickRelease.AddListener(OnClickRelease);

    }

    private void Update()
    {
        if (hovered)
        {
            highlightedSr.enabled = true;
        }
        else
        {
            highlightedSr.enabled = false;
        }
    }

    void OnClick()
    {

    }
    void OnClickRelease()
    {
        if (hovered && ps.mousePosGO.transform.childCount != 0)
        {
            ItemSeed mouseItem = ps.mousePosGO.GetComponentInChildren<ItemSeed>();

            if (gameObject.transform.childCount == 1)
            {
                mouseItem.SendToContainer(this.gameObject);
                mouseItem.GetPlanted();
            }
            else
            {
                mouseItem.SendToContainer(mouseItem.lastContainer);
            }
        }
    }

}
