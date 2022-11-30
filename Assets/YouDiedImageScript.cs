using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YouDiedImageScript : MonoBehaviour
{
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = gameObject.GetComponent<Image>();
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeToBlackInternal());
    }

    IEnumerator FadeToBlackInternal()
    {
        while (_image.color.a < 1)
        {
            var fadeAmount = _image.color.a + (2f * Time.deltaTime);

            var newColor = new Color(_image.color.r, _image.color.g, _image.color.b, fadeAmount);
            _image.color = newColor;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
