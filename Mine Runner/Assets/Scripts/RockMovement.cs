using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform target;
    NavMeshAgent agent;
    float rotationSpeed = -3f;
    public bool shouldDeform = false;
    public bool isCrushed = false;

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
        MoveRockAndRotate();
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
        foreach (Transform child in destroyedRockParticles) { child.localScale = new Vector3(xScale, yScale, zScale); }
        Destroy(destroyedRockFX, destroyedRockFX.duration);
        return destroyedRockFX.duration;
    }

    private void MoveRockAndRotate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed, Space.Self);
        rb.AddForce(Vector3.right * 400f * Time.deltaTime);
        if (rb.velocity.x > 4.5f) { rb.velocity = new Vector3(4.5f, rb.velocity.y, rb.velocity.z); }
    }

    private IEnumerator AddCrossForce()
    {
        yield return new WaitForSeconds(3f);
        float forceToAdd = -170f;
        while (true)
        {
            rb.AddForce(Vector3.forward * forceToAdd);
            forceToAdd *= -1f;
            yield return new WaitForSeconds(4f);
        }
    }

    // check if the velocity is below 0.5 if it is deform terrain on rock by setting shouldDeform true and check agan at every so
    private IEnumerator DecideToDeform()
    {
        while (true)
        {
            shouldDeform = false;
            if (rb.velocity.x < 0.5f) 
            { 
                shouldDeform = true;
            }
            yield return null;
        }
    }

    [Obsolete]
    private void ProcessCoroutines()
    {
        StartCoroutine(DecideToDeform());
    }

}
