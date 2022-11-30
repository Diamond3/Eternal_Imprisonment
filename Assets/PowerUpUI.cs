using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpUI : MonoBehaviour
{
    TextMeshProUGUI _attackText, _speedText, _jumpHeight;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PowerUpsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
