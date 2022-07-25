using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ditzelgames;


public class WaterFloating : MonoBehaviour
{
    // public properties
    public float AirDrag = 1;
    public float WaterDrag = 10;
    public Transform[] FloatPoints;
    public Transform WaterSurface;

    // used components
    protected Rigidbody rigidbody;

    // water line
    protected float waterLine;
    protected Vector3[] waterLinePoints;

    //help vectors
    protected Vector3 centerOffset;
    protected Vector3 smoothVectorRotation;
    protected Vector3 targetUp;

    

    public Vector3 Center { get { return transform.position + centerOffset; } }
    void Awake()
    {
        if (!WaterSurface)
            WaterSurface = GameObject.Find("WaterSurface").transform;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;

        waterLinePoints = new Vector3[FloatPoints.Length];
        for (int i = 0; i < FloatPoints.Length; i++)
            waterLinePoints[i] = FloatPoints[i].position;
        centerOffset = PhysicsHelper.GetCenter(waterLinePoints) - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // water surface
        if(WaterSurface)
        {
            float newWaterLine = 0f;
            bool pointUnderWater = false;

            for (int i = 0; i < FloatPoints.Length; i++)
            {
                waterLinePoints[i] = FloatPoints[i].position;
                waterLinePoints[i].y = WaterSurface.position.y;
                newWaterLine += waterLinePoints[i].y / FloatPoints.Length;
                if (waterLinePoints[i].y > FloatPoints[i].position.y)
                    pointUnderWater = true;
            }

            float waterLineDelta = newWaterLine - waterLine;
            waterLine = newWaterLine;

            // gravity
            Vector3 gravity = Physics.gravity;
            rigidbody.drag = AirDrag;
            if (waterLine > Center.y)
            {
                rigidbody.drag = WaterDrag;
                // go up
                gravity = -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * .9f);
            }
            rigidbody.AddForce(gravity * Mathf.Clamp(Mathf.Abs(waterLine - Center.y), 0, 1));

            // rotation stuff 
            targetUp = WaterSurface.up;
            if (pointUnderWater)
            {
                targetUp = Vector3.SmoothDamp(transform.up, targetUp, ref smoothVectorRotation, .2f);
                rigidbody.rotation = Quaternion.FromToRotation(transform.up, targetUp) * rigidbody.rotation;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (FloatPoints == null)
            return;

        for (int i = 0; i < FloatPoints.Length; i++)
        {
            if (FloatPoints[i] == null)
                continue;

            if (WaterSurface != null && Application.isPlaying)
            {

                //draw cube
                Gizmos.color = Color.red;
                Gizmos.DrawCube(waterLinePoints[i], Vector3.one * 0.3f);
            }

            //draw sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(FloatPoints[i].position, 0.1f);

        }

        //draw center
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(Center.x, waterLine, Center.z), Vector3.one * .5f);
        }
    }

}
