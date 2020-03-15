using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
