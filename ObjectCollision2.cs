using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;


public class ObjectCollision2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Moving_Object1;
    public GameObject Moving_Object2;
    public GameObject Stationary_Object; 
    public GameObject New_Stationary_Object; // <-- assign this in the inspector

    public TextMeshProUGUI Starter_PCF_Display;

    public TextMeshProUGUI Real_PCF_Display;

    private static bool hasSwapped = false;
    private void OnCollisionEnter(Collision collision)
{
    // Only continue if we haven't already swapped and collided with a StationaryMarker
    StationaryMarker marker = collision.gameObject.GetComponent<StationaryMarker>();

    if (hasSwapped || marker == null)
        return;

    GameObject hitStationary = collision.gameObject;

        // Ensure only Moving_Object1 can collide with Stationary_Object to trigger swap
        if (gameObject == Moving_Object1 && hitStationary == Stationary_Object)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab1 = Moving_Object1.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab2 = Moving_Object2.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor1 = grab1?.interactorsSelecting.Count > 0 ? grab1.interactorsSelecting[0] : null;
            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor2 = grab2?.interactorsSelecting.Count > 0 ? grab2.interactorsSelecting[0] : null;

            // Force release from hand
            if (grab1 != null && grab1.interactorsSelecting.Count > 0)
                grab1.interactionManager.SelectExit(interactor1, grab1);

            if (grab2 != null && grab2.interactorsSelecting.Count > 0)
                grab2.interactionManager.SelectExit(interactor2, grab2);

            // Swap positions
            Vector3 pos1 = Moving_Object1.transform.position;
            Quaternion rot1 = Moving_Object1.transform.rotation;

            Vector3 pos2 = Moving_Object2.transform.position;
            Quaternion rot2 = Moving_Object2.transform.rotation;

            Moving_Object1.transform.SetPositionAndRotation(pos2, rot2);
            Moving_Object2.transform.SetPositionAndRotation(pos1, rot1);

            // Re-grab
            if (interactor1 != null && grab2.interactionManager != null)
                grab2.interactionManager.SelectEnter(interactor1, grab2);

            if (interactor2 != null && grab1.interactionManager != null)
                grab1.interactionManager.SelectEnter(interactor2, grab1);

            // Replace stationary object
            if (New_Stationary_Object != null)
            {
                Vector3 pos = hitStationary.transform.position;
                Quaternion rot = hitStationary.transform.rotation;

                Vector3 newPos = New_Stationary_Object.transform.position;
                Quaternion newRot = New_Stationary_Object.transform.rotation;

                // Swap the positions of the current and new stationary objects
                hitStationary.transform.SetPositionAndRotation(newPos, newRot);
                New_Stationary_Object.transform.SetPositionAndRotation(pos, rot);
            }
                Vector3 posStarter = Starter_PCF_Display.transform.position;
                Quaternion rotStarter = Starter_PCF_Display.transform.rotation;

                Vector3 posReal = Real_PCF_Display.transform.position;
                Quaternion rotReal = Real_PCF_Display.transform.rotation;

                Starter_PCF_Display.transform.SetPositionAndRotation(posReal, rotReal);
                Real_PCF_Display.transform.SetPositionAndRotation(posStarter, rotStarter);

            hasSwapped = true;
            Invoke(nameof(ResetSwap), 2f);

    }
}

    private void ResetSwap()
    {
        hasSwapped = false;
    }

       void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
