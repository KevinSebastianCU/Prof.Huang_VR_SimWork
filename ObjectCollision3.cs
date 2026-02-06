using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ObjectCollision3 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Moving_Object11;
    public GameObject Moving_Object21;
    public GameObject Stationary_Object1; 
    public GameObject New_Stationary_Object1; // <-- assign this in the inspector


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
            if (grab1 != null && interactor1 != null && grab1.interactionManager != null)
{
    if (grab1.interactorsSelecting.Contains(interactor1))
    {
        grab1.interactionManager.SelectExit(interactor1, grab1);
    }
}

if (grab2 != null && interactor2 != null && grab2.interactionManager != null)
{
    if (grab2.interactorsSelecting.Contains(interactor2))
    {
        grab2.interactionManager.SelectExit(interactor2, grab2);
    }
}
            // Replace stationary object
            if (New_Stationary_Object1 != null)
            {
                Vector3 pos = hitStationary.transform.position;
                Quaternion rot = hitStationary.transform.rotation;

                Vector3 newPos = New_Stationary_Object1.transform.position;
                Quaternion newRot = New_Stationary_Object1.transform.rotation;

                // Swap the positions of the current and new stationary objects
                hitStationary.transform.SetPositionAndRotation(newPos, newRot);
                New_Stationary_Object1.transform.SetPositionAndRotation(pos, rot);

                hasSwapped1 = true;
                Invoke(nameof(ResetSwap), 2f);

            }
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
