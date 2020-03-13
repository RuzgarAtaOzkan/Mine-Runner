using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockMovement : MonoBehaviour
{
    Rigidbody rb;
    NavMeshAgent agent;
    public bool shouldDeform = false;
    public bool isCrushed = false;

    [SerializeField] Transform target;
    [SerializeField] ParticleSystem rockDestroyFX;

    [Obsolete]
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        ProcessCoroutines();
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }

    [Obsolete]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            DestroyObstacleAnimation(collision, 0.8f, 0.8f, 0.8f);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            isCrushed = true;
        }
    }

    [Obsolete]
    public float DestroyObstacleAnimation(Collision collision, float xScale, float yScale, float zScale)
    {
        ParticleSystem destroyedRockFX = Instantiate(rockDestroyFX, collision.gameObject.transform.position, Quaternion.identity);
        Transform[] destroyedRockParticles = destroyedRockFX.GetComponentsInChildren<Transform>();
        foreach (Transform destroyedRockParticle in destroyedRockParticles) 
        {
            Vector3 scaledPosition = new Vector3(xScale, yScale, zScale);
            destroyedRockParticle.localScale = scaledPosition; 
        }
        Destroy(destroyedRockFX, destroyedRockFX.duration);
        return destroyedRockFX.duration;
    }


    private void RotateMinecart()
    {
        Quaternion defaultRotations = Quaternion.Euler(-90f, 0f, 0f);
        transform.rotation = defaultRotations;
    }

    // check if the velocity is below 0.5 if it is deform terrain on rock by setting shouldDeform true and check agan at every so
    private IEnumerator DecideToDeform()
    {
        while (true)
        {
            shouldDeform = false;
            if (rb.velocity.x < 0.5f) { shouldDeform = true; }
            yield return null;
        }
    }

    [Obsolete]
    private void ProcessCoroutines()
    {
        StartCoroutine(DecideToDeform());
    }

}
