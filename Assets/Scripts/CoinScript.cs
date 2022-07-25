using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoinType{
    BRONZE,
    SILVER,
    GOLD,
    DIAMOND
}
public class CoinScript : MonoBehaviour
{
    public float HeightOffset;
    public float RotationOffsetY;
    public float TimeToAnimate;
    public float TimeAfterAnimate;
    public AnimationCurve AnimationCurvature;
    public Color BronzeColor;
    public Color SilverColor;
    public Color GoldColor;
    public Color DiamondColor;
    public CoinType TypeOfCoin;

    private float oneOverTimeToAnimate;
    private float timeElapsed;
    private float fromHeight;
    private float toHeight;
    private float fromRotationEulerY;
    private float toRotationEulerY;

    bool cashedOut = false;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.localPosition;
        fromHeight = pos.y;
        toHeight = fromHeight + HeightOffset;

        fromRotationEulerY = transform.rotation.eulerAngles.y;
        toRotationEulerY = fromRotationEulerY + RotationOffsetY;

        oneOverTimeToAnimate = 1 / TimeToAnimate;

        switch(TypeOfCoin)
        {
            case CoinType.BRONZE:
                GetComponent<Renderer>().material.SetColor("_BaseColor", BronzeColor);
                break;
            case CoinType.SILVER:
                GetComponent<Renderer>().material.SetColor("_BaseColor", SilverColor);
                break;
            case CoinType.GOLD:
                GetComponent<Renderer>().material.SetColor("_BaseColor", GoldColor);
                break;
            case CoinType.DIAMOND:
                GetComponent<Renderer>().material.SetColor("_BaseColor", DiamondColor);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        float lerpValue = AnimationCurvature.Evaluate(timeElapsed * oneOverTimeToAnimate);

        Vector3 pos = transform.localPosition;
        pos.y = Mathf.Lerp(fromHeight, toHeight, lerpValue);
        transform.localPosition = pos;

        float yRotEuler = Mathf.Lerp(fromRotationEulerY, toRotationEulerY, lerpValue);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotEuler, transform.rotation.eulerAngles.z);

        if (timeElapsed > TimeToAnimate + TimeAfterAnimate)
        {
            Destroy(gameObject);
        }
        else if (timeElapsed > TimeToAnimate && !cashedOut)
        {
            switch (TypeOfCoin)
            {
                case CoinType.BRONZE:
                    UserInterfaceScript.Instance.AddToCurrentBalance(.05f);
                    break;
                case CoinType.SILVER:
                    UserInterfaceScript.Instance.AddToCurrentBalance(1f);
                    break;
                case CoinType.GOLD:
                    UserInterfaceScript.Instance.AddToCurrentBalance(10f);
                    break;
                case CoinType.DIAMOND:
                    UserInterfaceScript.Instance.AddToCurrentBalance(100f);
                    break;
            }
            cashedOut = true;
        }

    }
}
