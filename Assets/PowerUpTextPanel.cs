using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpTextPanel : MonoBehaviour
{
    [SerializeField] GameObject _powerUpPanel;
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;

    Transform _player = null;
    PowerUp _obj = null;
    bool _inUse = false;
    float _pickRadius = 3f;

    void Start()
    {
        _powerUpPanel.SetActive(false);
    }

    public void SetPanel(string text, Transform player, PowerUp obj, float radius)
    {
        if (_inUse) return;
        _pickRadius = radius;
        _player = player;
        _obj = obj;
        _inUse = true;
        _powerUpPanel.SetActive(true);
        _textMeshProUGUI.text = text + "\n(E - to pick up)";
    } 

    public void HidePanel()
    {
        _inUse = false;
        _player = null;
        _obj = null;
        _powerUpPanel.SetActive(false);
    }

    void Update()
    {
        if (_inUse)
        {
            if (_player == null || _obj?.Active == false || (_player.position - _obj.transform.position).sqrMagnitude > _pickRadius * _pickRadius)
            {
                HidePanel();
            }
        }
    }
}
