using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public Slider _slider;
    public Gradient _gradient;
    public Image _fill;
    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        
        //_fill.color = _gradient.Evaluate(1f);
    }

    public void Sethealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        //_fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }


}
