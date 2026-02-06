using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;


public class DispenserCode : MonoBehaviour
{

    public Animator animatedObject;       // Animator on the independent object
                                          //public string animationTrigger = "PlaySwap"; // trigger in Animator

    public GameObject Table;

    public Button myButton;
    
  
    public GameObject ConcreteCylinder;

    public GameObject ConcreteCylinder2;


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

    public void AfterAnimation()
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

     private IEnumerator WaitAndSwap()
    {
        // get current animation state info
        AnimatorStateInfo state = animatedObject.GetCurrentAnimatorStateInfo(0);

        // how long the current clip lasts
        float clipLength = state.length;

        Debug.Log("Waiting " + clipLength + " seconds until swap...");

        // wait for animation to finish
        yield return new WaitForSeconds(clipLength + 1);

        // now run the swap function
        AfterAnimation();
    }





    public void OnButtonClick()
    {
        Debug.Log("PlayAnim() was called!");

        animatedObject.SetTrigger("PlaySwap");

        StartCoroutine(WaitAndSwap());



    }   



    // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        Debug.Log("Script has started!");
        myButton.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
