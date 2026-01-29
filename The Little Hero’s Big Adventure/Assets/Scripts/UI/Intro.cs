using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] int clickCount = 0;
    [SerializeField] GameObject[] introTexts;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] string[] buttonTexts;
    int amountOfClickEvents = 2;

    public void SetIntro()
    {
        switch (clickCount)
        {
            case 0:
                introTexts[0].SetActive(false);
                introTexts[1].SetActive(true);
                buttonText.text = buttonTexts[1];
                clickCount++;
                break;
            case 1:
                LoadNewScene();
                break;
            default:
                break;
        }
    }

    public void LoadNewScene()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        nextSceneIndex = nextSceneIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings ? 0 : nextSceneIndex;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
