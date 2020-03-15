using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    TerrainDeformer terrainDeformer;

    private void Start()
    {
        terrainDeformer = FindObjectOfType<TerrainDeformer>();
    }

    public void LoadNextLevel()
    {
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
