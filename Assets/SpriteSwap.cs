using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwap : MonoBehaviour
{
    [SerializeField] GameObject _obj1, _obj2;
    void Start()
    {
        _obj1.SetActive(true);
        _obj2.SetActive(false);
    }

    public void Swap()
    {
        _obj1.SetActive(false);
        _obj2.SetActive(true);
    }
}
