using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class load_game : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
    }

    public void StartGame()
    {
        Debug.LogError("!@#");
        SceneManager.LoadScene("Main");
    }
}