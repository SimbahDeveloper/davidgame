using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class TarikTambangUI : MonoBehaviour
{
    public Slider _slider;
    bool _SliderFull = false;
    bool _SliderEmpty = false;

    public string GetStatusGame()
    {
        if (!_SliderFull && !_SliderEmpty)
        {
            return Constants.OnGame;
        }else if (_SliderFull)
        {
            return Constants.Win;
        }
        else if(_SliderEmpty)
        {
            return Constants.Lose;
        }
        else
        {
            return Constants.Draw;
        }
    }

    public void UpdateSlider(float value)
    {
        _slider.value += value;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_slider.value == _slider.maxValue)
        {
            _SliderFull = true;
        }
    }
}
