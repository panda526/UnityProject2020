using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour 
{
    public void SceneChangeByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void BackToMenu()
    {
        GameManager.ctrl.BackToMenu();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
