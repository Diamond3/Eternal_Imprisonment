using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Action
{
    Jump
}

public class InputManager : MonoBehaviour
{
    private KeyCode[] InputMap;
    public Vector2 Movement { get; private set; } = Vector2.zero;

    public void Awake()
    {
        InputMap = new KeyCode[] {
             KeyCode.Space,
         };
    }
    public void Update()
    {
        Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        #region Restart

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("AidasScene", LoadSceneMode.Single);
        }

        #endregion
    }

    public bool GetKey(Action inputCode)
    {
        return Input.GetKey(InputMap[(int)inputCode]);
    }
    public bool GetKeyDown(Action inputCode)
    {
        return Input.GetKeyDown(InputMap[(int)inputCode]);
    }

    public bool GetKeyUp(Action inputCode)
    {
        return Input.GetKeyUp(InputMap[(int)inputCode]);
    }

}