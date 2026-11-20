using UnityEngine;

public class ItemFertilizer : Item
{
    [Header("fertilizer attributes")]
    public int boostLevel;

    ButtonInventory bi;
    private void Awake()
    {
        PointAndClick pnc = Object.FindAnyObjectByType<PointAndClick>();
        pnc.onClickPush.AddListener(OnClick);
        bi = Object.FindAnyObjectByType<ButtonInventory>();
    }
    void OnClick()
    {
        if (!bi.open && held && MousePosBehavior.Instance.hoveredGardenPlot != null)
        {
            GameObject hoveredContainerGO = MousePosBehavior.Instance.hoveredGardenPlot;
            hoveredContainerGO.TryGetComponent<GardenPlot>(out GardenPlot hoveredGP);
            if(hoveredGP != null)
            {
                float growthChange = 0;

                #region determine growth change based on wetness/dryness of the plot
                switch (hoveredGP.gplevel)
                {
                    case (GardenPlotLevel.marsh):
                        break;
                    case (GardenPlotLevel.wet):
                        growthChange = 0.125f;
                        break;
                    case (GardenPlotLevel.dry):
                        growthChange = 0.25f;

                        break;
                    case (GardenPlotLevel.desert):
                        growthChange = 0.75f;
                        break;
                    default:
                        growthChange = 0.25f;
                        break;
                }
                #endregion

                hoveredGP.growth += growthChange;
                count--;

                if(count < 1)
                {
                    Destroy(gameObject);
                }
                else
                {
                    UpdateCountText();

                }
            }
        }
    }
}
