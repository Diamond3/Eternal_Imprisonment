using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpUI : MonoBehaviour
{
    public TextMeshProUGUI _attackText, _speedText, _jumpHeight;
    PowerUpsManager _manager;

    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<PowerUpsManager>();
        UpdateText();
    }
    public void UpdateText()
    {
        float ats, ms, jh;
        (ats, ms, jh) = _manager.GetPowerUpsCount();

        _attackText.text = "Attack Speed " + (ats) * 100 + "%";
        _speedText.text = "Speed " + (ms - 1) * 100 + "%";
        _jumpHeight.text = "Jump " + (jh - 1) * 100 + "%";
    }
    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
}
