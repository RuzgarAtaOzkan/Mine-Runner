using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    RockMovement rockMovement;
    float speed = 4.8f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rockMovement = FindObjectOfType<RockMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FreezeRotations();
        MovePosition();
    }

    [Obsolete]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            animator.SetBool("isDigging", true);
            rockMovement.DestroyObstacleAnimation(collision, 0.3f, 0.3f, 0.3f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            animator.SetBool("isDigging", false);
        }
    }

    private void MovePosition()
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
