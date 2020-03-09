using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform targetSkeleton;
    RockMovement rockMovement;
    [SerializeField] Vector3 offset = new Vector3(0f, 15f, 5f);
    
    void Start()
    {
        targetSkeleton = GameObject.Find("skeleton").transform;
        rockMovement = FindObjectOfType<RockMovement>();
    }

    void Update()
    {
        transform.position = targetSkeleton.transform.position + offset;
        if (rockMovement.isCrushed)
        {
            StartCoroutine(CameraShake());
            rockMovement.isCrushed = false;
        }
    }

    private IEnumerator CameraShake()
    {
        bool isShaking = true;
        int shakeCount = 0;
        while (isShaking)
        {
            const float cameraXRotation = 90f; 
            float currentXRotation = Camera.main.transform.rotation.x;
            float currentYRotation = Camera.main.transform.rotation.y;
            float xShakeMagnitude = Random.Range(-2f, 2f);
            float yShakeMagnitude = Random.Range(-2, 2f);
            Quaternion cameraRotation = Camera.main.transform.rotation;
            transform.rotation = Quaternion.Euler(currentXRotation + xShakeMagnitude + cameraXRotation, currentYRotation + yShakeMagnitude, cameraRotation.z);
            shakeCount++;
            if (shakeCount > 40) { isShaking = false; }
            yield return null;
        }
    }
}
