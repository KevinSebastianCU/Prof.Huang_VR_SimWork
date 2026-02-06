using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;


public class SliderValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text Slider_Value;

    // Start is called before the first frame update
    void Start()
    {
        if (slider != null && Slider_Value != null)
        {
            UpdateValueText(slider.value);
            slider.onValueChanged.AddListener(UpdateValueText);

        }
    }

    void UpdateValueText(float value)
    {
        Slider_Value.text = value.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
