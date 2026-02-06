using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using System;
using Unity.VisualScripting;


public class Materialsandshit : MonoBehaviour
{
    public Dropdown CA;



    public Dropdown FA;



    public Dropdown Water;



    public Dropdown cement;



    public Slider WCRatio;

    public float f_c, w, c;
    public float SulphateContentpercentCoarse, SulphateContentpercentCement, SulphateContentpercentWater,
    SulphateContentpercentFine, total_SulphateContent; //needed in dropdowns, alkali% and silica-ratio change when cured.

    public float silica_ratio, MagOx, AlkaliContent, AirContent,
     CoarseAggContent, FineAggContent, SpecficArea, SVRatio,
     GypsumContent, FreeLimeContent, bln;
    /*specificarea needed in dropdown*/

    public float CuringAge = 28f;

    public float WC_ratio;

    public Slider water, Cement, Air_Content, Coarse_Agg_Content, Fine_Agg_Content,
    Coarse_Specific_Surface, Sulphate_Content_Coarse, 
    Sulphate_Content_Fine, Silicate_Ratio, Alkali_Content, Sulphate_Content_Cement,
    Free_Lime_Content, Gypsum_Content, Specific_Surface_Cement, Magnesium_Oxide;

    public GameObject hydraulicPress;

    public GameObject hydraulicPress2;

    public TextMeshProUGUI outputresult;

    public Button Button1;

    public Animator Hy1;

    public Animator Hy2;

    public void UpdateCalculations(int index1, int index2, int index3, int index4)
    {

        switch (index1)
        {
            case 0: //Type 1
                silica_ratio = 3f; //https://www.researchgate.net/publication/374338250_Shear_behavior_of_treated_soil_by_rice_husk_ash_and_cement
                Gypsum_Content.value = 0.04f;
                Free_Lime_Content.value = 0.015f;
                Magnesium_Oxide.value = 0.029f;
                SulphateContentpercentCement = 0.025f;
                Alkali_Content.value = 0.007f;
                bln = Specific_Surface_Cement.value = 350f;
                break;

            case 1: //Type 2 
                silica_ratio = 2f;
                Gypsum_Content.value = 0.04f;
                Free_Lime_Content.value = 0.015f;
                Magnesium_Oxide.value = 0.029f;
                SulphateContentpercentCement = 0.02f;
                Alkali_Content.value = 0.04f;
                bln = Specific_Surface_Cement.value = 350f;
                break;

            case 2: //Type 3 
                silica_ratio = 4f;
                Gypsum_Content.value = 0.04f;
                Free_Lime_Content.value = 0.02f;
                Magnesium_Oxide.value = 0.029f;
                SulphateContentpercentCement = 0.035f;
                Alkali_Content.value = 0.06f;
                bln = Specific_Surface_Cement.value = 450f;
                break;

            case 3: //Type 4 
                silica_ratio = 1f;
                Gypsum_Content.value = 0.04f;
                Free_Lime_Content.value = 0.01f;
                Magnesium_Oxide.value = 0.029f; // has to do with cracking
                SulphateContentpercentCement = 0.03f;
                Alkali_Content.value = 0.005f;
                bln = Specific_Surface_Cement.value = 300f;
                break;


            case 4: //Type 5 
                silica_ratio = 0.5f;
                Gypsum_Content.value = 0.04f; //prevents flash setting and affects setting time
                Free_Lime_Content.value = 0.01f;
                Magnesium_Oxide.value = 0.029f; // has to do with cracking
                SulphateContentpercentCement = 0.025f;
                Alkali_Content.value = 0.004f;
                bln = Specific_Surface_Cement.value = 350f;
                break;
        }

        switch (index2)
        {

            case 0: // Crushed Granite
                SulphateContentpercentCoarse = 0.002f;
                break;

            case 1: // Crushed LimeStone 
                SulphateContentpercentCoarse = 0.035f;
                break;

            case 2: //3B River Gravel
                SulphateContentpercentCoarse = 0.005f;
                break;

        }

        switch (index3)
        {
            case 0: //Medium Sand
                SulphateContentpercentFine = 0.005f;
                break;

            case 1: //Fine Sand
                SulphateContentpercentFine = 0.003f;
                break;

        }

        switch (index4)
        {
            case 0:
            SulphateContentpercentWater = 0.04f;
            break;

        }


        Debug.Log("UpdateCalculations called");
        // Early exit if cement is 0
        if (Cement.value == 0)
        {
            outputresult.text = "You can't have 0 cement in your concrete, please input a cement amount.";
            return;  // Avoid further calculations if cement is zero
        }

        w = water.value;
        c = Cement.value;

        // Avoid unnecessary recalculations
        /*SulphateContentpercentCoarse = Sulphate_Content_Coarse.value;
        SulphateContentpercentFine = Sulphate_Content_Fine.value;
        SulphateContentpercentCement = Sulphate_Content_Cement.value;*/
        AirContent = Air_Content.value;
        CoarseAggContent = Coarse_Agg_Content.value;
        FineAggContent = Fine_Agg_Content.value;

        // Precompute total SulphateContent
        total_SulphateContent = SulphateContentpercentCement * c + SulphateContentpercentCoarse * CoarseAggContent + SulphateContentpercentFine * FineAggContent + SulphateContentpercentWater * w;

       
        GypsumContent = Gypsum_Content.value * c;
        SVRatio = 1f / 3f;
        FreeLimeContent = Free_Lime_Content.value * c;
        MagOx = Magnesium_Oxide.value * c;
        AlkaliContent = Alkali_Content.value * c;
        WC_ratio = WCRatio.value;

        // Avoid zero or negative values for logarithms and pow
        bln = Specific_Surface_Cement.value == 0 ? 1 : Specific_Surface_Cement.value;

        // Precompute logs where needed
        float logBln = Mathf.Log(bln);
        float logCuringAge = Mathf.Log(CuringAge);
        float logC = Mathf.Log(c);

        // Calculate common terms once
        float term1 = Mathf.Pow(silica_ratio, 1.026f);
        float term2 = Mathf.Pow(FineAggContent / c, 1.008f);
        float term3 = Mathf.Pow(logBln, 1.436f);  // Use precomputed logBln
        float term4 = Mathf.Pow(logCuringAge, 1.352f);
        float term5 = Mathf.Pow(logC, 3.790f);
        float term6 = Mathf.Pow(SVRatio, 2.867f);
        float term7 = Mathf.Pow(total_SulphateContent - GypsumContent, 1.040f);
        float term8 = Mathf.Pow(WC_ratio + 0.03f * AirContent, 1.040f);
        float term9 = Mathf.Pow(FreeLimeContent, 1.010f);
        float term10 = Mathf.Pow(SpecficArea, 1.050f);
        float term11 = Mathf.Pow(CoarseAggContent / c, 0.866f);
        float term12 = Mathf.Pow(AlkaliContent, 0.899f);
        float term13 = Mathf.Pow(MagOx, 1.069f);

        // Calculate denominator only once
        float denominator = term7 + term8 + term9 + term10 + term11 + term12 + term13;

        // Check if denominator is too small to avoid division by zero
        if (denominator == 0)
        {
            outputresult.text = "Please configure the Cement Interface.";
            return; // Avoid division by zero
        }

        // Calculate f_c only if the denominator is valid
        f_c = 0.718f * ((term1 + term2 + term3 + term4 + term5 + term6) / denominator);
        outputresult.text = $"Estimated Compressive Strength: {f_c*145f:F4} lb/sq-in";

    }

    public void SwappingPlaces()
    {
        
        Vector3 positionHy1 = hydraulicPress.transform.position;
        Quaternion rotationHy1 = hydraulicPress.transform.rotation;

        Vector3 positionHy2 = hydraulicPress2.transform.position;
        Quaternion rotationHy2 = hydraulicPress2.transform.rotation;

        hydraulicPress.transform.SetPositionAndRotation(positionHy2, rotationHy2);
        hydraulicPress2.transform.SetPositionAndRotation(positionHy1, rotationHy1);

    }

    public bool isSwapping = false;

    public IEnumerator DelayedSwapCalc()
    {
        

         AnimatorStateInfo state = Hy1.GetCurrentAnimatorStateInfo(0);

        // how long the current clip lasts
        float clipLength = state.length;

        Debug.Log("Waiting " + clipLength + " seconds until swap...");

        // wait for animation to finish
        yield return new WaitForSeconds(clipLength + 1);

        SwappingPlaces();

        UpdateCalculations(cement.value, CA.value, FA.value, Water.value);



    }



    public void OnButtonClick()
    {
        isSwapping = true;

        Hy1.SetTrigger("Down");

        StartCoroutine(DelayedSwapCalc());


    }
    

    // Start is called before the first frame update
    void Start()
    {
        
        /*CA_materials.onValueChanged.AddListener(OnCoarseDropdownChanged);
         FA_materials.onValueChanged.AddListener(OnFineDropdownChanged);*/
        Button1.onClick.AddListener(OnButtonClick);
    }
        
    

    // Update is called once per frame
    void Update()
    {

    }
}
