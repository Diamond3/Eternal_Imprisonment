using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    const string LEVEL1_END_TEXT = "January 5th, 2005:\nI need money but this gravedigging job is killing me… I can’t stop though, I need to finish my invention, it’s going to change how we perceive the world.I can’t wait to see what heaven looks like and talk to my family again.It’s been so long since I’ve seen them.Every time I dig a grave I see the pain in the people’s eyes, the same pain that I’ve been feeling for so long… I hope I can help them meet their loved ones again.\nPress Enter to continue.";
    const string LEVEL2_END_TEXT = "January 6th, 2005:\nMet a strange man today, he seemed to know about my work...Offered me a lot of money to help me finish my invention but had weird conditions.I mean, who wants to see hell? Why would he want me to open the door to it as well? I don‘t know about this but I do need to reach heaven, I guess I‘ll take his money and worry about the consequences later.\nPress Enter to continue.";
    const string LEVEL3_END_TEXT = "March 28th, 2005:\nI finally did it, I finally managed to open the door to the afterlife! I peeked through to hell, oh man, that place looks terrible, I need to tell my financer that it is not a good idea to go there. I’m sure that when he sees the contrast between the door to heaven and the door to hell, he’ll make the right decision. Off I go…\nPress Enter to continue.";

    [SerializeField]
    GameObject _endLevelPanel;
    [SerializeField]
    TextMeshProUGUI _textMeshProUGUI;

    bool _crRunning = false;
    Coroutine _textCoroutine;

    void Start()
    {
        _endLevelPanel.SetActive(false);
        _textMeshProUGUI.text = string.Empty;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_crRunning)
        {
            _endLevelPanel.SetActive(true);
            _textCoroutine = StartCoroutine(TypeText());
            _crRunning = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _endLevelPanel.SetActive(false);
            _crRunning = false;
            StopCoroutine(_textCoroutine);
            _textMeshProUGUI.text = string.Empty;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Return))
            {
                var currentScene = SceneManager.GetActiveScene().name;
                switch (currentScene)
                {
                    case "AidasScene":
                    case "Level1":
                        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
                        break;
                    case "Level2":
                        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
                        break;
                    case "Level3":
                        SceneManager.LoadScene("Level4-Boss", LoadSceneMode.Single);
                        break;
                    default:
                        Application.Quit();
                        break;
                }
            }
        }
    }

    private IEnumerator TypeText()
    {
        string levelEndText = "";
        switch (SceneManager.GetActiveScene().name)
        {
            case "AidasScene":
            case "Level1":
                levelEndText = LEVEL1_END_TEXT;
                break;
            case "Level2":
                levelEndText = LEVEL2_END_TEXT;
                break;
            case "Level3":
                levelEndText = LEVEL3_END_TEXT;
                break;
            default:
                Application.Quit();
                break;
        }
        foreach (char letter in levelEndText)
        {
            _textMeshProUGUI.text += letter;
            // TODO: jei noresim ideti typinimo soundus:
            //if (typeSound1 && typeSound2)
            //    SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);
            yield return 0;
            yield return new WaitForSeconds(0.00003f);
        }
    }
}
