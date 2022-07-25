using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public Transform ArcEnd;
    public Transform ArcTop;
    public Animator ChestAnimator;
    public GameObject CoinSpawner;

    float oneOverParabolaDuration = 0;
    Quaternion fromRot = Quaternion.identity;
    ParabolaController parabola;
    float timeElapsed = 0;

    public float chestAmount = 0;
    bool hasMovedOnParabola = false;
    bool hasDoneEndOfParabola = false;

    WaterFloating _floatscript;
    Rigidbody _rigidbody;

    
    void Start()
    {
        parabola = GetComponent<ParabolaController>();

        ArcTop = GameObject.Find("ChestArcTop").transform;
        ArcEnd = GameObject.Find("ChestArcEnd").transform;
        _floatscript = GetComponent<WaterFloating>();
        _rigidbody = GetComponent<Rigidbody>();

        
    }

    
    void Update()
    {
        if (parabola.Animation == true && parabola.enabled == true)
        {
            hasMovedOnParabola = true;
            transform.rotation = Quaternion.Slerp(fromRot, ArcEnd.rotation, timeElapsed * oneOverParabolaDuration );
            timeElapsed += Time.deltaTime;
        }
        else if(hasMovedOnParabola && !hasDoneEndOfParabola)
        {
            hasDoneEndOfParabola = true;

            _floatscript.enabled = false;
            _rigidbody.isKinematic = true;
            ChestAnimator.enabled = true;
            CoinSpawner.SetActive(true);
        }
        else
            timeElapsed = 0;

        
    }

    bool _hasBeenClicked;
    private void OnMouseDown()
    {
        if (GameManager.Instance.State != GameState.PLAY && _hasBeenClicked == false)
            return;

        _hasBeenClicked = true;
        parabola.enabled = true;
        Transform[] parabolaPoints = parabola.ParabolaRoot.GetComponentsInChildren<Transform>();
        parabolaPoints[1].position = transform.position;
        parabolaPoints[2].position = ArcTop.position;
        parabolaPoints[3].position = ArcEnd.position;
        oneOverParabolaDuration = 1 / parabola.GetDuration();

        if (chestAmount != 0)
            GameManager.Instance.IncrementNumChestGotRight();
    }

    public void SetChestAmount(float amount)
    {
        chestAmount = amount;
        CoinSpawner.GetComponent<CoinSpawner>().MoneyValue = chestAmount;
    }
}
