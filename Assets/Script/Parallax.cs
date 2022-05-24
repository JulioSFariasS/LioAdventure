using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPosX, startPosY;

    private Transform cam;

    public float parallaxFactor, parallaxFactorY;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main.transform;
    }

    void Update()
    {
        float restartPos = cam.transform.position.x * (1 - parallaxFactor);
        float distanceX = cam.transform.position.x * parallaxFactor;
        float distanceY = cam.transform.position.y * parallaxFactorY;

        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);
        
        if(restartPos > startPosX + length)
        {
            startPosX += length;
        }
        else if(restartPos < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
