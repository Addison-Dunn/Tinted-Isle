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

    public float growth;
    public float dryness;
    public float wetness;

    private InputAction clickAction;

    private PointSensor ps;

    private PointAndClick pnc;

    public ButtonInventory bi;

    private GameManager gm;

    public bool hovered => ps.hovered;

    private void Awake()
    {
        gm = Object.FindAnyObjectByType<GameManager>();
        gm.updateGrowth.AddListener(GrowthTickUpdate);

        clickAction = InputSystem.actions.FindAction("Click");
        ps = GetComponent<PointSensor>();

        pnc = ps.mousePosGO.transform.GetComponentInParent<PointAndClick>();

        pnc.onClick.AddListener(OnClick);
        pnc.onClickRelease.AddListener(OnClickRelease);

    }

    private void Update()
    {
        if (!bi.open)
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
    }

    void OnClick()
    {
        if (!bi.open)
        {
            if (hovered && ps.mousePosGO.transform.childCount != 0)
            {
                ItemSeed mouseItem = ps.mousePosGO.GetComponentInChildren<ItemSeed>();


                if (gameObject.transform.childCount == 1)
                {
                    GameObject dropFlower = Instantiate(mouseItem.flowerPlanted);
                    FlowerPlanted fp = dropFlower.GetComponent<FlowerPlanted>();
                    fp.SendToGardenPlot(gameObject);
                    mouseItem.count--;
                    mouseItem.UpdateCountText();

                    if (mouseItem.count == 0)
                    {
                        Destroy(mouseItem.gameObject);
                    }
                }
                else
                {
                    mouseItem.SendToContainer(mouseItem.lastContainer);
                }
            }
        }
    }
    void OnClickRelease()
    {
        
    }

    void GrowthTickUpdate()
    {
        if(transform.childCount > 1)
        {
            FlowerPlanted fp = GetComponentInChildren<FlowerPlanted>();
            int randomNum = Random.Range(0, 2);
            fp.progressGrowth(randomNum);
        }
    }

}
