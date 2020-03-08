using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    Rigidbody rb;

    float rotationSpeed = -3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed, Space.Self);
        rb.AddForce(Vector3.right * 400f * Time.deltaTime);
        if (rb.velocity.x > 4f)
        {
            rb.velocity = new Vector3(4f, rb.velocity.y, rb.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(collision.gameObject);
        }
    }
}
