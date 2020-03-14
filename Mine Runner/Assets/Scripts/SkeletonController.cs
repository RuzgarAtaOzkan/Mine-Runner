using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    RockMovement rockMovement;
    int obstaclesLength;
    float speed = 9f;

    [SerializeField] Material flashMat;
    [SerializeField] GameObject minerQuantityFX;
    [SerializeField] Transform minerQuantitiyLerpTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rockMovement = FindObjectOfType<RockMovement>();
        animator = GetComponent<Animator>();
        obstaclesLength = GameObject.FindGameObjectsWithTag("Obstacle").Length;
    }

    void Update()
    {
        FreezeRotations();
        MovePosition(speed);
        KeepTrackOfObjectsWithTag("Obstacle");
        LerpMinerQuantities(minerQuantitiyLerpTarget.position);
    }

    [Obsolete]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && collision.gameObject.tag != "Terrain")
        {
            animator.SetBool("isDigging", true);
            StartCoroutine(FlashObstacle(collision, flashMat, 0.05f));
            DestroyObstacleAfterFlashObstacle(collision, 1.08f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            animator.SetBool("isDigging", false);
        }
    }

    [Obsolete]
    public IEnumerator FlashObstacle(Collision collision, Material flashMat, float flashRepeatTime)
    {
        if (collision.gameObject.GetComponent<MeshRenderer>() != null && collision.gameObject.tag == "Obstacle" && collision.gameObject.tag != "Terrain")
        {
            int counter = 0;
            int switcher = 1;
            bool isFlashing = true;
            Material[] savedMaterials = collision.gameObject.GetComponent<MeshRenderer>().materials;
            while (isFlashing)
            {
                Material[] flashMaterialsToPlace = new Material[savedMaterials.Length];
                for (int i = 0; i < flashMaterialsToPlace.Length; i++)
                {
                    flashMaterialsToPlace[i] = flashMat;
                }
                switch (switcher)
                {
                    case 1:
                        try
                        {
                            if (collision.gameObject.GetComponent<MeshRenderer>() != null && collision.gameObject.tag == "Obstacle")
                            {
                                collision.gameObject.GetComponent<MeshRenderer>().materials = savedMaterials;
                            }
                        }
                        catch
                        {
                            Debug.Log("object has been destroyed");
                        }
                        break;
                    case -1:
                        try
                        {
                            if (collision.gameObject.GetComponent<MeshRenderer>() != null && collision.gameObject.tag == "Obstacle")
                            {
                                collision.gameObject.GetComponent<MeshRenderer>().materials = flashMaterialsToPlace;
                            }
                        }
                        catch
                        {
                            Debug.Log("object has been destroyed");
                        }
                        break;
                }
                counter++;
                switcher *= -1;
                if (counter > 7) 
                {
                    isFlashing = false;
                } 
                yield return new WaitForSeconds(flashRepeatTime);
            }
            StopCoroutine(FlashObstacle(collision, flashMat, 0.08f));
        }
    }

    [Obsolete]
    private void DestroyObstacleAfterFlashObstacle(Collision collision, float destroyTime)
    {
        float animDuration = rockMovement.DestroyObstacleAnimation(collision, 0.5f, 0.5f, 0.5f);
        float destroyDuration = animDuration - (animDuration / destroyTime);
        KeepTrackOfMinerQuantitiesLength(collision, 2); // second parameter determines to destroy all the mine quantites in scene
        Destroy(collision.gameObject, destroyDuration);
    }

    private static void LerpMinerQuantities(Vector3 target) // lerp all the quantity FXs to specific position
    {
        GameObject[] minerQuantities = GameObject.FindGameObjectsWithTag("MinerQuantityFX");
        foreach (GameObject minerQuantity in minerQuantities)
        {
            if (minerQuantity != null)
            {
                minerQuantity.transform.position = Vector3.Lerp(minerQuantity.transform.position, target, Time.deltaTime * 2f);
                Destroy(minerQuantity, 2f);
            }
        }
    }

    private void KeepTrackOfMinerQuantitiesLength(Collision collision, float numberOfMinerQuantity) // destroy mine quantities if they passed the edge value
    {
        Instantiate(minerQuantityFX, collision.gameObject.transform.position, Quaternion.identity);
        GameObject[] minerQuantities = GameObject.FindGameObjectsWithTag("MinerQuantityFX");
        if (minerQuantities.Length > numberOfMinerQuantity)
        {
            for (int i = 0; i < minerQuantities.Length; i++)
            {
                Destroy(minerQuantities[i++]);
            }
        }
    }

    private void KeepTrackOfObjectsWithTag(string tagName)
    {
        int childrens = GameObject.FindGameObjectsWithTag(tagName).Length;
        if (childrens < obstaclesLength)
        {
            obstaclesLength = childrens;
            animator.SetBool("isDigging", false);
        }
    } 

    private void MovePosition(float speed)
    {
        rb.MovePosition(rb.position + Vector3.right * speed * Time.deltaTime);
        rb.AddForce(Vector3.forward * -270f);
    }

    private void FreezeRotations()
    {
        float xRotation = transform.rotation.x;
        xRotation = Mathf.Clamp(xRotation, xRotation - 1, xRotation + 2);
        transform.rotation = Quaternion.Euler(xRotation, 90f, 90f);
    }
}
