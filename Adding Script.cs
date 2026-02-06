using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using NUnit.Framework.Internal;



public class AddingScript : MonoBehaviour
{
    [SerializeField] public Slider slider_Coarse;
    [SerializeField] public Slider slider_Fine;

    [SerializeField] public Slider MC_C;

    [SerializeField] public Slider MC_F;

    [SerializeField] public Slider slider_Water;
    [SerializeField] public Slider slider_Cement;

    [SerializeField] public Slider slider_AC;

    [SerializeField] public Dropdown slider_Airentrained;

    public int AirEntrained;

    [SerializeField] public Dropdown CA;

    public int coarse;

    [SerializeField] public Dropdown FA;

    [SerializeField] public Dropdown Water;

    public int fine;

    [SerializeField] public Dropdown Cement;

    public int cement;

    [SerializeField] public Dropdown Slump;

    public int slump1;

    public float slump;

    [SerializeField] public Slider WCRatio; // set initial to 0.3 and max to 0.4.


    public TextMeshProUGUI result_sign;

    public TextMeshProUGUI Warning_Sign;

    public Button Button2;

    public GameObject Unmixed_Drum;

    public Animator SpinningDrum_ANIM;


    public GameObject Mixed_Drum;


    // Start is called before the first frame update
    public float MoistureContentF;

    public float MoistureContentC;//Formula from website: https://www.engineeringtoolbox.com/moisture-content-d_1821.html#:~:text=Calculate%20the%20moisture%20content%20in,h2o%20=%20mw%20MCw

    public float Total_Agg_Amount;

    public float FAgg_SG;

    public float CAgg_SG;

    public float Water_SG;

    public float Cement_SG;

    public float Volume_FA;

    public float Volume_CA;

    public float Volume_W;

    public float Volume_Ce;

    public float Volume_Total_NoFA;

    public float Volume_Total = 27f;

    public float FM_FA;

    public float MSA_CA;

    bool Air_Entrained;

    public float WC_Ratio;

    public float Bulk_Volume;

    public float density = 500f;

    public float Final_Fine;

    public float Final_Coarse;

    public float Absorption_Capacity_CA;

    public float Absorption_Capacity_FA;

    public float sum;

    /// <summary>
    ///for volume checks: used the ratio table in this slide presentation: https://www.fhwa.dot.gov/pavement/materials/hmec/pubs/module_g/lesson04_wbt.pdf. and proceeded
    /// to calculate the PCF by summing the weights and dividing by 27ft^3
    /// </summary>


    void UpdateSum(int index1, int index2, int index3, int index4, int index5)
    {
        //Coarse Aggregate
        switch (index1)
        {
            case 0: //Crushed Granite - MSA: 1.5in - https://www.braenstone.com/materials/aggregates/crushed-granite/
                MSA_CA = 1.5f;
                CAgg_SG = 2.65f; //https://www.tctc.edu/media/3594/cal1-section-3-rev4-uw-added.pdf 
                Absorption_Capacity_CA = 0.5f;
                break;

            case 1: //#57 Crushed Limestone - MSA: 1in- https://www.hilltopcompanies.com/crushed-stone/ 
                MSA_CA = 1f;
                CAgg_SG = 2.66f; //https://www.tctc.edu/media/3594/cal1-section-3-rev4-uw-added.pdf 
                Absorption_Capacity_CA = 3f;
                break;

            case 2: //3B River Gravel - MSA: 3in - https://aplsinc.com/product/3b-river-gravel/#:~:text=Description,of%201.5%E2%80%B3%20%E2%80%93%203%E2%80%B3.
                MSA_CA = 3f;
                CAgg_SG = 2.55f; //https://pdfs.semanticscholar.org/1ff5/8e7885d474f4a58ba5f8343282006c7fe435.pdf#:~:text=3.2.1%20Specific%20Gravity%20of%20River%20Stone%20From,%5B11%5D.%202.612%20are%20obviously%20within%20this%20limit.
                Absorption_Capacity_CA = 1.5f; //https://www.ce.memphis.edu/1101/notes/concrete/everything_about_concrete/06_aggregate.html#:~:text=Absorption%20capacity%20(AC)%20%2D%2D%20maximum,aggregates%20is%201%20%2D%202%25.
                break;
        }
        
        //Fine Aggregate
        switch (index2)
        {
            case 0: // Medium Sand - FM: 2.7 - https://testbook.com/question-answer/the-fineness-modulus-of-medium-sand-ranges-between--6242c6d3178b6ea9d458d717#:~:text=Table_title:%20Detailed%20Solution%20Table_content:%20header:%20%7C%20Type,Fineness%20Modulus%20Range:%202.9%20%2D%203.2%20%7C
                FM_FA = 2.7f;
                FAgg_SG = 2.66f; //https://civiltoday.com/civil-engineering-materials/sand/359-specific-gravity-of-sand 
                Absorption_Capacity_FA = 2.3f;
                break;

            case 1: // Fine Sand - FM: 2.2 - https://www.scribd.com/document/402744503/Fineness-Modulus-of-Sand-Fine-Aggregate-and-Calculation#:~:text=Aggregate)%20and%20Calculation-,The%20fineness%20modulus%20of%20sand%20is%20an%20index%20that%20represents,determine%20the%20cumulative%20percentages%20retained. 
                FM_FA = 2.2f; //
                Absorption_Capacity_FA = 2.3f; //https://www.sciencedirect.com/science/article/pii/S0950061817320536
                FAgg_SG = 2.75f; // https://www.ijscer.com/uploadfile/2015/0429/20150429082139577.pdf#:~:text=The%20bulk%20density%20of%20Manufactured%20sand%20was,and%20it%20was%20found%20to%20be%20similar.  
                break;

        }

        //air entrained
        switch (index3)
        {
            case 0: // Air Entrained
                Air_Entrained = true;
                break;

            case 1: // Non-Air Entrained
                Air_Entrained = false;
                break;
        }

        //slump 
        switch (index4) // Table 7.8
        {
            case 0: // 1 to 2
                slump = 1.5f;
                break;

            case 1: // 3 to 4
                slump = 3.5f;
                break;

            case 2: // 6 to 7 
                slump = 6.5f;
                break;

        }

        //cement 
        switch (index5)
        {
            case 0://Type 1
                Cement_SG = 3.15f;
                break;
            case 1: //Type 2:https://www.researchgate.net/publication/367110297_Experimental_Study_on_Mechanical_Properties_and_Durability_of_Polymer_Silica_Fume_Concrete_with_Vinyl_Ester_Resin
                Cement_SG = 3.18f;
                break;
            case 2: //Type 3:https://www.pci.org/PCI_Docs/Papers/2021/Paper_Cardelino.pdf#:~:text=A%20Type%20III%20cement%20and,have%20specific%20gravities%20of%202.7.
                Cement_SG = 3.10f;
                break;
            case 3: //Type 4: Close Enough to 3.15
                Cement_SG = 3.15f;
                break;
            case 4: //Type 5: Close Enough to 3.15
                Cement_SG = 3.15f;
                break; 
            

        }
        // non - air entrained 
        //for slump = 1.5
        if (slump == 1.5 && MSA_CA == 1.5 && Air_Entrained == false)
        {
            slider_Water.value = 275;   
        }

        if (slump == 1.5 && MSA_CA == 1 && Air_Entrained == false)
        {
            slider_Water.value = 300;
        }

        if (slump == 1.5 && MSA_CA == 3 && Air_Entrained == false)
        {
            slider_Water.value = 220;
        }
        
        //slump = 3.5
         if (slump == 3.5 && MSA_CA == 1.5 && Air_Entrained == false)
        {
            slider_Water.value = 300;   
        }

        if (slump == 3.5 && MSA_CA == 1 && Air_Entrained == false)
        {
            slider_Water.value = 325;
        }

        if (slump == 3.5 && MSA_CA == 3 && Air_Entrained == false)
        {
            slider_Water.value = 245;
        }
        
        //slump 6.5
        if (slump == 6.5 && MSA_CA == 1.5 && Air_Entrained == false)
        {
            slider_Water.value = 315;   
        }

        if (slump == 6.5 && MSA_CA == 1 && Air_Entrained == false)
        {
            slider_Water.value = 340;
        }

        if (slump == 6.5 && MSA_CA == 3 && Air_Entrained == false)
        {
            slider_Water.value = 270;
        }

         // Yes air entrained 
        //for slump = 1.5
        if (slump == 1.5 && MSA_CA == 1.5 && Air_Entrained == true)
        {
            slider_Water.value = 250;   
        }

        if (slump == 1.5 && MSA_CA == 1 && Air_Entrained == true)
        {
            slider_Water.value = 270;
        }

        if (slump == 1.5 && MSA_CA == 3 && Air_Entrained == true)
        {
            slider_Water.value = 205;
        }
        
        //slump = 3.5
         if (slump == 3.5 && MSA_CA == 1.5 && Air_Entrained == true)
        {
            slider_Water.value = 275;   
        }

        if (slump == 3.5 && MSA_CA == 1 && Air_Entrained == true)
        {
            slider_Water.value = 295;
        }

        if (slump == 3.5 && MSA_CA == 3 && Air_Entrained == true)
        {
            slider_Water.value = 225;
        }
        
        //slump 6.5
        if (slump == 6.5 && MSA_CA == 1.5 && Air_Entrained == true)
        {
            slider_Water.value = 290;   
        }

        if (slump == 6.5 && MSA_CA == 1 && Air_Entrained == true)
        {
            slider_Water.value = 310;
        }

        if (slump == 6.5 && MSA_CA == 3 && Air_Entrained == true)
        {
            slider_Water.value = 260;
        }


        //Check for if Water-Cement Ratio is too high
        if (slider_Water.value / slider_Cement.value > 0.7)
        {
            Warning_Sign.text = "Excessive Slump, Change your w/c";
        }

        if (slider_Water.value / slider_Cement.value < 0.4)
        {
            Warning_Sign.text = "Low Slump, Change your w/c";
        }

        //Bulk Volume of Coarse Aggreate Calculations - gotten from table 7.5
        if (MSA_CA == 1.5 && FM_FA == 2.7)
        {
            Bulk_Volume = 0.72f;
        }

        if (MSA_CA == 1.5 && FM_FA == 2.4)
        {
            Bulk_Volume = 0.75f;
        }

        if (MSA_CA == 1 && FM_FA == 2.7)
        {
            Bulk_Volume = 0.68f;
        }

        if (MSA_CA == 1 && FM_FA == 2.4)
        {
            Bulk_Volume = 0.71f;
        }

        if (MSA_CA == 3 && FM_FA == 2.7)
        {
            Bulk_Volume = 0.79f;
        }

        if (MSA_CA == 3 && FM_FA == 2.4)
        {
            Bulk_Volume = 0.82f;
        }

        //Cement Calculation

        slider_Cement.value = slider_Water.value * WCRatio.value;

        //Weight of Coarse Aggregate Calculation

        slider_Coarse.value = density * Bulk_Volume * 27;

        //Volume Calculation for Fine Aggregate
        Volume_CA = slider_Coarse.value / (CAgg_SG * 62.4f);
        Volume_W = slider_Water.value / (1* 62.4f);
        Volume_Ce = slider_Cement.value / (Cement_SG * 62.4f);

        Volume_Total_NoFA = Volume_CA + Volume_Ce + Volume_W + slider_AC.value*27f;

        Volume_FA = Volume_Total - Volume_Total_NoFA;

        slider_Fine.value = Volume_FA * (FAgg_SG * 62.4f);

        //field Moisture Adjustments.

        if (slider_AC.value < MC_F.value)
        {
            Final_Fine = slider_Fine.value * (1 - MC_F.value);
        }

        if (slider_AC.value > MC_F.value)
        {
            Final_Fine = slider_Fine.value * (1 + Absorption_Capacity_FA);
        }

        if (slider_AC.value < MC_C.value)
        {
            Final_Coarse = slider_Coarse.value * (1 - MC_C.value);
        }

        if (slider_AC.value > MC_C.value)
        {
            Final_Coarse = slider_Coarse.value * (1 + Absorption_Capacity_CA);
        }
        
        float Final_Water = slider_Water.value - (Final_Coarse - slider_Coarse.value) - (Final_Fine - slider_Fine.value);

        sum = (slider_Cement.value + Final_Coarse + Final_Fine + Final_Water) / 27f;


    }
    

    public void OnButtonClick()
    {
        UpdateSum(CA.value, FA.value, slider_Airentrained.value, Slump.value, Cement.value);

        Vector3 UnDrumpos = Unmixed_Drum.transform.position;
        Quaternion UnDrumRot = Unmixed_Drum.transform.rotation;

        Vector3 MixedDrumpos = Mixed_Drum.transform.position;
        Quaternion MixedDrumRot = Mixed_Drum.transform.rotation;

        Unmixed_Drum.transform.SetPositionAndRotation(MixedDrumpos, MixedDrumRot);
        Mixed_Drum.transform.SetPositionAndRotation(UnDrumpos, UnDrumRot);

        SpinningDrum_ANIM.SetTrigger("Mix");

        result_sign.text = sum.ToString("F2") + " (lb)/(ft^3)";

    }

    void Start()
    {
        slider_Coarse.onValueChanged.AddListener( delegate { UpdateSum(CA.value, FA.value, slider_Airentrained.value, Slump.value, Cement.value);  } );
        slider_Fine.onValueChanged.AddListener(delegate { UpdateSum(CA.value, FA.value, slider_Airentrained.value, Slump.value, Cement.value); });
        slider_Water.onValueChanged.AddListener(delegate { UpdateSum(CA.value, FA.value, slider_Airentrained.value, Slump.value, Cement.value); });
        slider_Cement.onValueChanged.AddListener(delegate { UpdateSum(CA.value, FA.value, slider_Airentrained.value, Slump.value, Cement.value); });
        Button2.onClick.AddListener(delegate { OnButtonClick(); });
        slider_AC.onValueChanged.AddListener(delegate { OnButtonClick(); });
        MC_C.onValueChanged.AddListener(delegate { OnButtonClick(); });
        MC_F.onValueChanged.AddListener(delegate { OnButtonClick();});


           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
