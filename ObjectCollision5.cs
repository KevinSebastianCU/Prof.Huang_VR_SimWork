using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ObjectCollision5 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Moving_Object11;
    public GameObject Moving_Object21;
    public GameObject Stationary_Object1; 


    private static bool hasSwapped1 = false;
    private void OnCollisionEnter(Collision collision)
    {
    // Only continue if we haven't already swapped and collided with a StationaryMarker
    StationaryMarker marker = collision.gameObject.GetComponent<StationaryMarker>();

    if (hasSwapped1 || marker == null)
        return;

    GameObject hitStationary = collision.gameObject;

        // Ensure only Moving_Object111 can collide with Stationary_Object to trigger swap
        if (gameObject == Moving_Object11 && hitStationary == Stationary_Object1)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab1 = Moving_Object11.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab2 = Moving_Object21.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor1 = grab1?.interactorsSelecting.Count > 0 ? grab1.interactorsSelecting[0] : null;
            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor2 = grab2?.interactorsSelecting.Count > 0 ? grab2.interactorsSelecting[0] : null;

            // Force release from hand
            if (interactor1 != null && grab1.interactionManager != null)
                grab1.interactionManager.SelectExit(interactor1, grab1);

            if (interactor2 != null && grab2.interactionManager != null)
                grab2.interactionManager.SelectExit(interactor2, grab2);

            // Swap positions
            Vector3 pos1 = Moving_Object11.transform.position;
            Quaternion rot1 = Moving_Object11.transform.rotation;

            Vector3 pos2 = Moving_Object21.transform.position;
            Quaternion rot2 = Moving_Object21.transform.rotation;

            Moving_Object11.transform.SetPositionAndRotation(pos2, rot2);
            Moving_Object21.transform.SetPositionAndRotation(pos1, rot1);

            // Re-grab
            if (interactor1 != null && grab2.interactionManager != null)
                grab2.interactionManager.SelectEnter(interactor1, grab2);

            if (interactor2 != null && grab1.interactionManager != null)
                grab1.interactionManager.SelectEnter(interactor2, grab1);

                hasSwapped1 = true;
                Invoke(nameof(ResetSwap), 2f);

            
        }
    }

    private void ResetSwap()
    {
        hasSwapped1 = false;
    }

       void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
