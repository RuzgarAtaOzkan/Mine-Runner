using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform targetSkeleton;
    [SerializeField] Vector3 offset = new Vector3(0f, 15f, 5f);
    
    void Start()
    {
        targetSkeleton = GameObject.Find("skeleton").transform;
    }

    void Update()
    {
        transform.position = targetSkeleton.transform.position + offset;
    }
}
