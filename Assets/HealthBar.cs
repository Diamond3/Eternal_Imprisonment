using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
	
    public void SetHealth(float newHealthValue)
    {
	    slider.value = newHealthValue;
    }

    public void SetMaxHealth(float maxHealthValue)
    {
	    slider.maxValue = maxHealthValue;
        slider.value = maxHealthValue;
    }
}
