using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    const string LEVEL_END_TEXT = "PRESS ENTER";

    TextMeshPro _endLevelText;
    bool _crRunning = false;
    Coroutine _textCoroutine;

    void Start()
    {
        _endLevelText = gameObject.transform.Find("LevelEndText").gameObject.GetComponent<TextMeshPro>();
        _endLevelText.text = string.Empty;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_crRunning)
        {
            _textCoroutine = StartCoroutine(TypeText());
            _crRunning = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _crRunning = false;
            StopCoroutine(_textCoroutine);
            _endLevelText.text = string.Empty;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Return))
            {
                //TODO: parasyti kito levelio pavadinima cia
                //SceneManager.LoadScene("TODO:NEXT_LEVEL_NAME", LoadSceneMode.Single);
            }
        }
    }

    private IEnumerator TypeText()
    {
        foreach (char letter in LEVEL_END_TEXT)
        {
            _endLevelText.text += letter;
            // TODO: jei noresim ideti typinimo soundus:
            //if (typeSound1 && typeSound2)
            //    SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);
            yield return 0;
            yield return new WaitForSeconds(0.07f);
        }
    }
}
