using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Advertisements;

public class RockMovement : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody rb;
    NavMeshAgent agent;
    SkeletonController skeletonController;
    LevelManager levelManager;
    public bool shouldDeform = false;
    public bool isCrushed = false;
    bool isFlashing = true;

    [SerializeField] AudioClip crashSoundSFX;
    [SerializeField] Image flashImage;
    [SerializeField] Material flashMat;
    [SerializeField] Transform target;
    [SerializeField] ParticleSystem rockDestroyFX;

    [Obsolete]
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        skeletonController = FindObjectOfType<SkeletonController>();
        levelManager = FindObjectOfType<LevelManager>();
        ProcessCoroutines();
        Time.timeScale = 1f;
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
        if (collision.gameObject.tag == "Player" && collision.gameObject.tag != "Terrain")
        {
            AudioSource.PlayClipAtPoint(crashSoundSFX, transform.position);
            isCrushed = true;
            StartCoroutine(skeletonController.FlashObject(collision, "Player", flashMat, 0.2f));
            StartCoroutine(FlashEffect(0.5f));
            Handheld.Vibrate();
            
        }
    }

    private IEnumerator FlashEffect(float flashTime)
    {
        while (isFlashing)
        {
            flashImage.CrossFadeAlpha(1f, 0.5f, true);
            isFlashing = false;
            yield return new WaitForSeconds(flashTime);
            flashImage.CrossFadeAlpha(0f, 0.5f, true);
        }
        StopCoroutine(FlashEffect(1f));
        levelManager.ReLoadCurrentLevel();
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0.4f;
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
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


    private void RotateMinecart() // not in use
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
