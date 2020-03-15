using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform targetSkeleton;
    RockMovement rockMovement;
    [SerializeField] Vector3 offset = new Vector3(2f, 15f, 5f);
    
    void Start()
    {
        targetSkeleton = GameObject.Find("skeleton").transform;
        rockMovement = FindObjectOfType<RockMovement>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetSkeleton.transform.position + offset, Time.deltaTime);
        if (rockMovement.isCrushed) { ProcessCoroutines(); }
    }

    private IEnumerator CameraShake(int xMagnitude, int yMagnitude)
    {
        const float cameraXRotation = 90f;
        const float cameraYRotation = 0f;
        bool isShaking = true;
        int shakeCount = 0;
        int shakeTime = 20;
        while (isShaking)
        {
            Quaternion cameraRotation = Camera.main.transform.rotation;

            float xShakeMagnitude = Random.Range(-xMagnitude, xMagnitude);
            float yShakeMagnitude = Random.Range(-yMagnitude, yMagnitude);

            float shakedXRoation = cameraXRotation + xShakeMagnitude;
            float shakedYRotation = cameraYRotation + yShakeMagnitude;

            float lerpedXRotation = Mathf.Lerp(cameraXRotation, shakedXRoation, Time.deltaTime * 2f);
            float lerpedYRotation = Mathf.Lerp(cameraYRotation, shakedYRotation, Time.deltaTime * 2f);
            
            Quaternion shakedRotations = Quaternion.Euler(lerpedXRotation, lerpedYRotation, cameraRotation.z);

            transform.rotation = shakedRotations;
            shakeCount++;
            if (shakeCount > shakeTime) { isShaking = false; }
            yield return null;
        }
        rockMovement.isCrushed = false;
        StopCoroutine(CameraShake(16, 4));
    }

    private void ProcessCoroutines()
    {
        StartCoroutine(CameraShake(60, 60));
    }
}
