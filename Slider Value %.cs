using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class SliderValuepercent : MonoBehaviour
{
    [SerializeField] private Slider slider1;
    [SerializeField] private TMP_Text Slider_Value1;

    // Start is called before the first frame update
    void Start()
    {
        if (slider1 != null && Slider_Value1 != null)
        {
            UpdateValueText(slider1.value);
            slider1.onValueChanged.AddListener(UpdateValueText);

        }
    }

    void UpdateValueText(float value)
    {
        Slider_Value1.text = value.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
