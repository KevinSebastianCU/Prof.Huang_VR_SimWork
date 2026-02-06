using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class ObjectCollision2butFullDrum : MonoBehaviour
{
        public GameObject Moving_Object1;
    public GameObject Moving_Object2;
    public GameObject Stationary_Object;
    public GameObject New_Stationary_Object; // assign in inspector
    public TextMeshPro TextPrefab; // assign in inspector
    public Vector3 TextOffset = new Vector3(0, 0.3f, 0); // how far above to place text

    private static bool hasSwapped = false;

    private void OnCollisionEnter(Collision collision)
    {
        StationaryMarker marker = collision.gameObject.GetComponent<StationaryMarker>();
        if (hasSwapped || marker == null)
            return;

        GameObject hitStationary = collision.gameObject;

        if (gameObject == Moving_Object1 && hitStationary == Stationary_Object)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab1 = Moving_Object1.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab2 = Moving_Object2.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor1 = grab1?.interactorsSelecting.Count > 0 ? grab1.interactorsSelecting[0] : null;
            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor2 = grab2?.interactorsSelecting.Count > 0 ? grab2.interactorsSelecting[0] : null;

            // Force release from hand
            if (interactor1 != null && grab1.interactionManager != null)
                grab1.interactionManager.SelectExit(interactor1, grab1);

            if (interactor2 != null && grab2.interactionManager != null)
                grab2.interactionManager.SelectExit(interactor2, grab2);

            // Swap positions of the moving objects
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

            // Swap positions of the stationary objects
            Vector3 posA = Stationary_Object.transform.position;
            Quaternion rotA = Stationary_Object.transform.rotation;

            Vector3 posB = New_Stationary_Object.transform.position;
            Quaternion rotB = New_Stationary_Object.transform.rotation;

            Stationary_Object.transform.SetPositionAndRotation(posB, rotB);
            New_Stationary_Object.transform.SetPositionAndRotation(posA, rotA);

            // Move TextMeshPro object above the new position of New_Stationary_Object
            
            if (TextPrefab != null)
            {
                Vector3 textPos = New_Stationary_Object.transform.position + TextOffset;
                Instantiate(TextPrefab, textPos, Quaternion.identity);
                Debug.Log("Text prefab instantiated above Second_Stationary_Object.");
            }

            hasSwapped = true;

            hasSwapped = true;
            Invoke(nameof(ResetSwap), 2f);
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
