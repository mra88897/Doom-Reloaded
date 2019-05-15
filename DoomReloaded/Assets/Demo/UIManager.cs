using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public void Back()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Play()
    {
        SceneManager.LoadScene("Workspace");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Workspace");
    }

    public void Exit()
    {
        Application.Quit();
    }



}