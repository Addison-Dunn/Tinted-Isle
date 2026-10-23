using UnityEngine;

public class ButtonInventory : MonoBehaviour
{
    public SpriteMask spriteMask;

    public GameObject hotbarGO;

    public AnimationCurve easing;
    public float openDuration;
    public float totalDistance;
    public Vector3 endPos;
    public Vector3 startPos;
    public Vector3 targetPos;
    Vector3 startLerpPos;
    float startLerpRot;

    public bool open;
    public float lerpTime;

    private void Awake()
    {
        startPos = hotbarGO.transform.localPosition;
    }
    public void OnClick()
    {
        open = open ? false : true;
        lerpTime = 0;
        startLerpPos = hotbarGO.transform.localPosition;
        startLerpRot = transform.localEulerAngles.z;
    }

    private void Update()
    {
        targetPos = open ? endPos : startPos;
        
        if(hotbarGO.transform.localPosition != targetPos)
        {
            float distance = Vector3.Distance(startLerpPos, targetPos);
            float distProgress = Mathf.Clamp01(distance / totalDistance);
            float duration = openDuration * distProgress;

            lerpTime += Time.deltaTime;

            float progress = Mathf.Clamp01(lerpTime / openDuration);

            progress = easing.Evaluate(progress);

            Vector3 pos = hotbarGO.transform.localPosition;
            pos.y = Mathf.Lerp(startLerpPos.y, targetPos.y, progress);
            hotbarGO.transform.localPosition = pos;

            float targetRotation = open ? 180 : 0;
            Vector3 rotation = transform.localEulerAngles;
            rotation.z = Mathf.LerpAngle(startLerpRot, targetRotation, progress);
            transform.localEulerAngles = rotation;
        }
    }
}
