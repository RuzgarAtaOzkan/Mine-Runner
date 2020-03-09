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
        bool isShaking = true;
        int shakeCount = 0;
        while (isShaking)
        {
            float currentXRotation = Camera.main.transform.rotation.x;
            float currentYRotation = Camera.main.transform.rotation.y;
            float xShakeMagnitude = Random.Range(-xMagnitude, xMagnitude);
            float yShakeMagnitude = Random.Range(-yMagnitude, yMagnitude);
            Quaternion cameraRotation = Camera.main.transform.rotation;
            transform.rotation = Quaternion.Euler(currentXRotation + xShakeMagnitude + cameraXRotation, currentYRotation + yShakeMagnitude, cameraRotation.z);
            shakeCount++;
            if (shakeCount > 40) { isShaking = false; }
            yield return null;
        }
        rockMovement.isCrushed = false;
    }

    private void ProcessCoroutines()
    {
        StartCoroutine(CameraShake(2, 2));
    }
}
