using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinalDrumCollision : MonoBehaviour
{

    public GameObject Moving_Object;               // The object currently held or moving
    public GameObject New_Stationary_Object;       // What to replace the stationary object with
    public GameObject Given_Object;                // What to give the player after swap

    private static bool hasSwapped = false;

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (hasSwapped) return;

        // Check if a hand touched this object
        if (other.CompareTag("Hand"))
        {
            hasSwapped = true;

            // Swap positions
            Vector3 stationaryPos = transform.position;
            Quaternion stationaryRot = transform.rotation;

            Vector3 movingPos = Moving_Object.transform.position;
            Quaternion movingRot = Moving_Object.transform.rotation;

            // Swap the transforms
            Moving_Object.transform.SetPositionAndRotation(stationaryPos, stationaryRot);
            transform.SetPositionAndRotation(movingPos, movingRot);

            // Replace the stationary object
            if (New_Stationary_Object != null)
            {
                Destroy(gameObject);
                Instantiate(New_Stationary_Object, movingPos, movingRot);
            }

            // Give the player an object
            if (Given_Object != null)
            {
                GameObject given = Instantiate(Given_Object, other.transform.position, Quaternion.identity);

                UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = given.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
                UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor>();

                if (grabInteractable != null && interactor != null && interactor.interactionManager != null)
                {
                    interactor.interactionManager.SelectEnter(interactor, grabInteractable);
                }
            }

            Invoke(nameof(ResetSwap), 2f); // Prevent repeated swaps for 2 seconds
        }
    }

    private void ResetSwap()
    {
        hasSwapped = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
