using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;

public class GardenPlot : MonoBehaviour
{
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
        gm.updateGrowth.AddListener(UpdateContainedFlowerGrowth);

        ps = GetComponent<PointSensor>();
        pnc = ps.mousePosGO.transform.GetComponentInParent<PointAndClick>();

        pnc.onClickPush.AddListener(OnClick);
        pnc.onClickRelease.AddListener(OnClickRelease);
    }

    private void Update()
    {
        if (bi.open)
        {
            ps.hoverable = false;
        }
        else
        {
            ps.hoverable = true;
        }
    }

    void OnClick()
    {
        if (hovered)
        {
            if (ps.mousePosGO.transform.childCount != 0)
            {
                // if mouse is holding a seed item, plant a flower on this plot
                ItemSeed mouseItemSeed = ps.mousePosGO.GetComponentInChildren<ItemSeed>();
                if (mouseItemSeed != null)
                {
                    if (gameObject.transform.childCount == 1)
                    {
                        GameObject dropFlower = Instantiate(mouseItemSeed.flowerPlanted);
                        FlowerPlanted fp = dropFlower.GetComponent<FlowerPlanted>();
                        fp.SendToGardenPlot(gameObject);
                        mouseItemSeed.count--;
                        mouseItemSeed.UpdateCountText();

                        if (mouseItemSeed.count == 0)
                        {
                            Destroy(mouseItemSeed.gameObject);
                        }
                    }
                    else
                    {
                        mouseItemSeed.SendToContainer(mouseItemSeed.lastContainer);
                    }
                }
  
            }


            //if mouse is holding shovel, pickup the flower at its current stage



            //if mouse is holding snips, harvest the flower
            
        }
        
    }
    void OnClickRelease()
    {
        
    }

    void UpdateContainedFlowerGrowth()
    {
        if(transform.childCount > 1)
        {
            FlowerPlanted fp = GetComponentInChildren<FlowerPlanted>();
            int plantGrowthDelta = 0;

            #region determine how to clamp growth based on the contained plant's attributes
            float clampedGrowth = 0;
            clampedGrowth = Mathf.Clamp(growth, fp.minGrowthChance, fp.maxGrowthChance);

            #endregion

            #region determine how much the containd plant will grow
            if (clampedGrowth < 0)
            {
                plantGrowthDelta = Random.value <= Mathf.Abs(clampedGrowth) ? -1 : 0;
            }
            else if (clampedGrowth < 0.5f)
            {
                plantGrowthDelta = 0;
            }
            else if (clampedGrowth < 1)
            {
                plantGrowthDelta = Random.value <= clampedGrowth ? 1 : 0;
            }
            else if (clampedGrowth < 2)
            {
                plantGrowthDelta = Random.value <= Utils.Map(clampedGrowth,-1,2,0,1,true) ? 2 : 1;
            }
            else if (clampedGrowth == 2)
            {
                plantGrowthDelta = 2;
            }
            fp.InitializeGrowth(plantGrowthDelta);
            #endregion
        }
    }

}
