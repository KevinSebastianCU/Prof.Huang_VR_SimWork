using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinessModuleCalculation : MonoBehaviour
{
    [SerializeField] public Slider CA_amount;
    [SerializeField] public Slider FA_amount;


    public TMP_Dropdown CA_material;
    public TMP_Dropdown FA_material;

    public Slider FM_Result;

    public float FM_FA;

    public float FM_CA;

    public float quantity_FA;

    public float quantity_CA;

    public float FA_percent;

    public float CA_percent;

    public float FM_mix;

    private void FMCalculation(int index1, int index2)
    {
        switch (index1)
        {
            case 0: //Crushed Granite
                FM_CA = 7.5f;
                break;

            case 1: // Crushed Limestone 
                FM_CA = 6.5f;
                break;

            case 2: //River Gravel
                FM_CA = 6.0f;
                break;

            case 3: //Lightweight Shale
                FM_CA = 7.0f;
                break;

            case 4: //Recycled Coarse Aggregate
                FM_CA = 5.75f;
                break;

        }

        switch (index2)
        {
            case 0: // Natural River Sand
                FM_FA = 2.7f;
                break;

            case 1: // Manufactured Sand
                FM_FA = 2.95f;
                break;

            case 2: // Crushed Stone Sand
                FM_FA = 3.6f;
                break;

            case 3: //Pit Sand
                FM_FA = 2.8f;
                break;

            case 4: //Recycled Fine Aggregate
                FM_FA = 2.5f;
                break;
        }

        quantity_CA = CA_amount.value;
        quantity_FA = FA_amount.value;

        FA_percent = quantity_FA / (quantity_FA + quantity_CA);
        CA_percent = quantity_CA / (quantity_FA + quantity_CA);

        FM_mix = (FA_percent * FM_FA) + (CA_percent * FM_CA);

        // Update the slider to reflect FM_mix (make sure the slider range fits FM values)
        FM_Result.value = FM_mix;

    }

    void Recalculate()
        {
            FMCalculation(CA_material.value, FA_material.value);
        }

    // Start is called before the first frame update
    void Start()
    {
        CA_amount.onValueChanged.AddListener(delegate { Recalculate(); });
        FA_amount.onValueChanged.AddListener(delegate { Recalculate(); });

        CA_material.onValueChanged.AddListener(delegate { Recalculate(); });
        FA_material.onValueChanged.AddListener(delegate { Recalculate(); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
