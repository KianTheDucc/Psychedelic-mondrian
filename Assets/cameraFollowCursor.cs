using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowCursor : MonoBehaviour
{
    public Camera playercamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playercamera.ScreenToWorldPoint(transform.position);
    }
}
