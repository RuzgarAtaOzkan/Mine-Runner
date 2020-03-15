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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentSceneIndex + 1;
        SceneManager.LoadScene(nextScene);
    } 

    public void ReLoadCurrentLevel()
    {
        StartCoroutine(LoadCurrentLevel(2f));
    }

    IEnumerator LoadCurrentLevel(float loadTime)
    {
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
