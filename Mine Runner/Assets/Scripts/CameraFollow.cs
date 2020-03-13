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
        if (rockMovement.isCrushed) { ProcessCoroutines(); }
    }

    private IEnumerator CameraShake(int xMagnitude, int yMagnitude)
    {
        const float cameraXRotation = 90f;
        const float cameraYRotation = 0f;
        bool isShaking = true;
        int shakeCount = 0;
        int shakeTinme = 10;
        while (isShaking)
        {
            float xShakeMagnitude = Random.Range(-xMagnitude, xMagnitude);
            float yShakeMagnitude = Random.Range(-yMagnitude, yMagnitude);
            float shakedXRoation = cameraXRotation + xShakeMagnitude;
            float shakedYRotation = cameraYRotation + yShakeMagnitude;
            Quaternion cameraRotation = Camera.main.transform.rotation;
            Quaternion shakedRotations = Quaternion.Euler(shakedXRoation, shakedYRotation, cameraRotation.z);
            transform.rotation = shakedRotations;
            shakeCount++;
            if (shakeCount > shakeTinme) { isShaking = false; }
            yield return null;
        }
        rockMovement.isCrushed = false;
        StopCoroutine(CameraShake(16, 4));
    }

    private void ProcessCoroutines()
    {
        StartCoroutine(CameraShake(5, 5));
    }
}
