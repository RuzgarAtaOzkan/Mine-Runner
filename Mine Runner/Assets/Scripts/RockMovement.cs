using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    Rigidbody rb;
    float rotationSpeed = -3f;
    public bool shouldDeform = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ProcessCoroutines();
    }

    void Update()
    {
        MoveRockAndRotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(collision.gameObject);
        }
    }

    private void MoveRockAndRotate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed, Space.Self);
        rb.AddForce(Vector3.right * 500f * Time.deltaTime);
        if (rb.velocity.x > 4.5f)
        {
            rb.velocity = new Vector3(4.5f, rb.velocity.y, rb.velocity.z);
        }
    }

    private IEnumerator AddCrossForce()
    {
        float forceToAdd = 170f;
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
            if (rb.velocity.x < 0.5f)
            {
                shouldDeform = true;
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private void ProcessCoroutines()
    {
        StartCoroutine(AddCrossForce());
        StartCoroutine(CheckIfStandingStill());
    }

}
