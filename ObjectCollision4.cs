using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ObjectCollision4 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Moving_Objects31;
    public GameObject Moving_Objects32;
    public GameObject Stationary_Object31; 
    public GameObject New_Stationary_Object31; // <-- assign this in the inspector

    public TextMeshProUGUI Starter_PC_Display31;

    public TextMeshProUGUI Real_PCF_Display31;

    private static bool hasSwapped2 = false;
    private void OnCollisionEnter(Collision collision)
{
    // Only continue if we haven't already swapped and collided with a StationaryMarker
    StationaryMarker marker = collision.gameObject.GetComponent<StationaryMarker>();

    if (hasSwapped2 || marker == null)
        return;

    GameObject hitStationary = collision.gameObject;

        // Ensure only Moving_Objects31 can collide with Stationary_Object31 to trigger swap
        if (gameObject == Moving_Objects31 && hitStationary == Stationary_Object31)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab1 = Moving_Objects31.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab2 = Moving_Objects32.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor1 = grab1?.interactorsSelecting.Count > 0 ? grab1.interactorsSelecting[0] : null;
            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor2 = grab2?.interactorsSelecting.Count > 0 ? grab2.interactorsSelecting[0] : null;

            // Force release from hand
            if (interactor1 != null && grab1.interactionManager != null)
                grab1.interactionManager.SelectExit(interactor1, grab1);

            if (interactor2 != null && grab2.interactionManager != null)
                grab2.interactionManager.SelectExit(interactor2, grab2);

            // Swap positions
            Vector3 pos1 = Moving_Objects31.transform.position;
            Quaternion rot1 = Moving_Objects31.transform.rotation;

            Vector3 pos2 = Moving_Objects32.transform.position;
            Quaternion rot2 = Moving_Objects32.transform.rotation;

            Moving_Objects31.transform.SetPositionAndRotation(pos2, rot2);

            // Replace stationary object
            if (New_Stationary_Object31 != null)
            {
                Vector3 pos = hitStationary.transform.position;
                Quaternion rot = hitStationary.transform.rotation;

                Vector3 newPos = New_Stationary_Object31.transform.position;
                Quaternion newRot = New_Stationary_Object31.transform.rotation;

                // Swap the positions of the current and new stationary objects
                hitStationary.transform.SetPositionAndRotation(newPos, newRot);
                New_Stationary_Object31.transform.SetPositionAndRotation(pos, rot);
            }
                Vector3 posStarter = Starter_PC_Display31.transform.position;
                Quaternion rotStarter = Starter_PC_Display31.transform.rotation;

                Vector3 posReal = Real_PCF_Display31.transform.position;
                Quaternion rotReal = Real_PCF_Display31.transform.rotation;

                Starter_PC_Display31.transform.SetPositionAndRotation(posReal, rotReal);
                Real_PCF_Display31.transform.SetPositionAndRotation(posStarter, rotStarter);

            hasSwapped2 = true;
            Invoke(nameof(ResetSwap), 2f);

    }
}

    private void ResetSwap()
    {
        hasSwapped2 = false;
    }

       void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
