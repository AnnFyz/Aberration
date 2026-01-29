using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Destroy(AudioManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
