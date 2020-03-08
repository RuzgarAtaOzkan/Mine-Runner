using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    Rigidbody rb;
    float rotationSpeed = -3f;
    public bool shouldDeform = false;

    [SerializeField] ParticleSystem rockDestroyFX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ProcessCoroutines();
    }

    void Update()
    {
        MoveRockAndRotate();
    }

    [Obsolete]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            DestroyObstacleAnimation(collision);
            Destroy(collision.gameObject);
        }
    }

    [Obsolete]
    private void DestroyObstacleAnimation(Collision collision)
    {
        ParticleSystem destroyedRockFX = Instantiate(rockDestroyFX, collision.gameObject.transform.position, Quaternion.identity);
        Transform[] destroyedRockParticles = destroyedRockFX.GetComponentsInChildren<Transform>();
        foreach (Transform child in destroyedRockParticles) { child.localScale = new Vector3(1.1f, 1.1f, 1.1f); }
        Destroy(destroyedRockFX, destroyedRockFX.duration);
    }

    private void MoveRockAndRotate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed, Space.Self);
        rb.AddForce(Vector3.right * 400f * Time.deltaTime);
        if (rb.velocity.x > 4.5f) { rb.velocity = new Vector3(4.5f, rb.velocity.y, rb.velocity.z); }
    }

    private IEnumerator AddCrossForce()
    {
        yield return new WaitForSeconds(1f);
        float forceToAdd = -170f;
        while (true)
        {
            rb.AddForce(Vector3.forward * forceToAdd);
            forceToAdd *= -1;
            yield return new WaitForSeconds(4f);
        }
    }

    // check if the velocity is below 0.5 if it is deform terrain on rock by setting shouldDeform true and check agan at every so
    private IEnumerator CheckIfStandingStill()
    {
        while (true)
        {
            shouldDeform = false;
            if (rb.velocity.x < 0.5f) { shouldDeform = true; }
            yield return new WaitForSeconds(2f);
        }
    }

    private void ProcessCoroutines()
    {
        StartCoroutine(AddCrossForce());
        StartCoroutine(CheckIfStandingStill());
    }

}
