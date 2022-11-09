using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
	
    public void setHealth(int newHealthValue)
    {
	slider.value = newHealthValue;
    }

    public void setMaxHealth(int maxHealthValue)
    {
	slider.maxValue = maxHealthValue;
        slider.value = maxHealthValue;
    }
}
