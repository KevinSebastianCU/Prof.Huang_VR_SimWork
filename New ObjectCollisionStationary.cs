using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewObjectCollisionStationary : MonoBehaviour
{
    
    public GameObject MovingObject; // The player-held object
    public List<GameObject> StationaryObjects; // Ordered list of swappable objects

    private int currentIndex = 0;
    private bool hasSwapped = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the one in the current order
        if (!hasSwapped && collision.gameObject == StationaryObjects[currentIndex])
        {
            GameObject stationary = StationaryObjects[currentIndex];

            // Handle grabbing (if applicable)
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = MovingObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor = grab?.interactorsSelecting.Count > 0 ? grab.interactorsSelecting[0] : null;

            if (interactor != null && grab.interactionManager != null)
                grab.interactionManager.SelectExit(interactor, grab);

            // Swap positions
            Vector3 tempPos = MovingObject.transform.position;
            Quaternion tempRot = MovingObject.transform.rotation;

            MovingObject.transform.SetPositionAndRotation(stationary.transform.position, stationary.transform.rotation);
            stationary.transform.SetPositionAndRotation(tempPos, tempRot);

            // Re-grab
            if (interactor != null && grab.interactionManager != null)
                grab.interactionManager.SelectEnter(interactor, grab);

            // Move to the next object in the order
            currentIndex++;
            hasSwapped = true;
            Invoke(nameof(ResetSwap), 1.5f);
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
