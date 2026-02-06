using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CuringScript : MonoBehaviour
{
    public GameObject Table;
    public GameObject ConcreteCylinder;

    public GameObject ConcreteCylinder2;

    public Button myButton;

    private bool isColliding = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject== Table)
        {
            isColliding = true;
            Debug.Log("Collision detected");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Table)
        {
            isColliding = false;
            Debug.Log("Collision has ended");
        }     
    }

    public void OnButtonClick()
    {
        if (isColliding == true)
        {
            Vector3 CC1pos = ConcreteCylinder.transform.position;
            Quaternion CC1rot = ConcreteCylinder.transform.rotation;

            Vector3 CC2pos = ConcreteCylinder2.transform.position;
            Quaternion CC2rot = ConcreteCylinder2.transform.rotation;

            ConcreteCylinder.transform.SetPositionAndRotation(CC2pos, CC2rot);
            ConcreteCylinder2.transform.SetPositionAndRotation(CC1pos, CC1rot);

            Debug.Log("Button Pressed While Colliding with Target");
        }
        else
        {
            Debug.Log("Button Pressed But not Colliding with Target");
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
