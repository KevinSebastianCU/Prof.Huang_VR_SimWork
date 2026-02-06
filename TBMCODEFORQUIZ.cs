using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HideTextonClick : MonoBehaviour
{
    public GameObject questionText;
    public Button correctButton;


    void HideText()
    {
        questionText.SetActive(false);
        Debug.Log("Right option, text is now hidden");
    }
    void Start()
    {
        correctButton.onClick.AddListener(HideText);
    }

}