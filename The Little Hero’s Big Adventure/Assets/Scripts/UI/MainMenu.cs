using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        PlayBackgroundMusic();
    }
    public void LoadNewScene()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        nextSceneIndex = nextSceneIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings ? 0 : nextSceneIndex;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayBackgroundMusic()
    {
        AudioManager.Instance.PlaySound("Background");
    }
}
