using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    AudioSource audioSource;
    TerrainDeformer terrainDeformer;

    [SerializeField] AudioClip levelPassSFX;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        terrainDeformer = FindObjectOfType<TerrainDeformer>();
    }

    public void LoadNextLevel()
    {
        AudioSource.PlayClipAtPoint(levelPassSFX, Camera.main.transform.position);
        StartCoroutine(LoadNextLevelSMooth(1f));
    }

    IEnumerator LoadNextLevelSMooth(float loadTime)
    {
        yield return new WaitForSeconds(loadTime);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentSceneIndex + 1;
        SceneManager.LoadScene(nextScene);
        terrainDeformer.SetTerrainHeightsBackToNormal();
    }

    public void ReLoadCurrentLevel()
    {
        StartCoroutine(LoadCurrentLevel(1.5f));
    }

    IEnumerator LoadCurrentLevel(float loadTime)
    {
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        terrainDeformer.SetTerrainHeightsBackToNormal();
    }
}
