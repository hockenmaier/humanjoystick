using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class HumanJoystick : MonoBehaviour
{
    public SteamVR_Camera SteamVRCamera;
    public Text DebugText;
    public float deadzoneRadius;
    public float livezoneRadius;
    public float speedFactor;
    Transform headTransform;
    Vector3 center;
    public GameObject headSpot;
    public GameObject deadZone;
    public GameObject liveZone;
    // Start is called before the first frame update
    void Start()
    {
        headTransform = SteamVRCamera.gameObject.transform;
        updateDeadZone();
        updateLiveZone();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Camera position is " + SteamVRCamera.gameObject.transform.position.x + " , " + SteamVRCamera.gameObject.transform.position.y + " , " + SteamVRCamera.gameObject.transform.position.z);

        //float GetDistanceFromCenter();
        updateCenter();
        float distance = GetDistanceFromCenter();
        Vector3 diff = GetVectorFromCenter();
        Debug.Log(distance);
        DebugText.text = "Distance from Center = " + distance;
        DebugText.text += System.Environment.NewLine;
        DebugText.text += "Vector from Center = " + diff;
        DebugText.text += System.Environment.NewLine;

        if (distance > deadzoneRadius)
        {
            MovePlaySpace(diff, distance);
        }else
        {
            DebugText.text += "Speed = 0";
        }
        updateHeadSpot();
    }

    void updateCenter()
    {
        center = new Vector3(transform.position.x, headTransform.position.y, transform.position.z);
    } 

    void updateHeadSpot()
    {
        headSpot.transform.position = new Vector3(headTransform.position.x, .05f, headTransform.position.z);
    }
    void updateDeadZone()
    {
        float scale = deadzoneRadius * 2f;
        //deadZone.transform.localScale -= deadZone.transform.localScale;
        deadZone.transform.localScale = new Vector3(scale, .02f, scale);
    }
    void updateLiveZone()
    {
        float scale = livezoneRadius * 2f;
        //liveZone.transform.localScale -= liveZone.transform.localScale;
        liveZone.transform.localScale = new Vector3(scale, .01f, scale);
    }


    float GetDistanceFromCenter()
    {
        //Vector3 center = new Vector3(0, headTransform.position.y, 0);
        float distance = Vector3.Distance(headTransform.position, center);
        return distance;
    }
    
    Vector3 GetVectorFromCenter()
    {
        Vector3 diff = headTransform.position - center;
        return diff;
    }

    void MovePlaySpace(Vector3 diff, float dist)
    {
        float speed = 0;
        if (livezoneRadius > dist)
        {
            speed = (dist - deadzoneRadius) * speedFactor;
        }
        else
        {
            speed = (livezoneRadius - deadzoneRadius) * speedFactor;
        }
        transform.position += (diff * speed) * Time.deltaTime;
        DebugText.text += "Speed = " + speed;
    }
}
