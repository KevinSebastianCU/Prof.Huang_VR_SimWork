using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectCollisions : MonoBehaviour
{
    public GameObject Moving_Object1;
    public GameObject Moving_Object2;
    public GameObject Stationary_Object;

    private bool hasSwapped = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only react if touching the stationary object
        if (hasSwapped || other.gameObject != Stationary_Object)
            return;

        if (Moving_Object1 == null || Moving_Object2 == null)
            return;

        XRGrabInteractable grab1 = Moving_Object1.GetComponent<XRGrabInteractable>();
        XRGrabInteractable grab2 = Moving_Object2.GetComponent<XRGrabInteractable>();

        if (grab1 == null || grab2 == null)
            return;

        // Save interactors holding them (direct grab or far grab)
        IXRSelectInteractor interactor1 = grab1.interactorsSelecting.Count > 0 ? grab1.interactorsSelecting[0] : null;
        IXRSelectInteractor interactor2 = grab2.interactorsSelecting.Count > 0 ? grab2.interactorsSelecting[0] : null;

        // Force release
        foreach (var interactor in new List<IXRSelectInteractor>(grab1.interactorsSelecting))
        {
            grab1.interactionManager.SelectExit(interactor, grab1);
        }
        foreach (var interactor in new List<IXRSelectInteractor>(grab2.interactorsSelecting))
        {
            grab2.interactionManager.SelectExit(interactor, grab2);
        }

        // Disable grabs temporarily
        grab1.enabled = false;
        grab2.enabled = false;

        // Swap positions and rotations
        Vector3 pos1 = Moving_Object1.transform.position;
        Quaternion rot1 = Moving_Object1.transform.rotation;

        Moving_Object1.transform.SetPositionAndRotation(Moving_Object2.transform.position, Moving_Object2.transform.rotation);
        Moving_Object2.transform.SetPositionAndRotation(pos1, rot1);

        hasSwapped = true;

        // Re-enable and re-grab
        StartCoroutine(ReenableAndRegrab(grab1, grab2, interactor1, interactor2));
    }

    private IEnumerator ReenableAndRegrab(
        XRGrabInteractable grab1,
        XRGrabInteractable grab2,
        IXRSelectInteractor interactor1,
        IXRSelectInteractor interactor2)
    {
        // Wait one frame so Unity processes release
        yield return null;

        grab1.enabled = true;
        grab2.enabled = true;

        // Re-grab with original interactors (works with near grab and far grab)
        if (interactor1 != null && grab2.interactionManager != null)
        {
            grab2.interactionManager.SelectEnter(interactor1, grab2);
        }

        if (interactor2 != null && grab1.interactionManager != null)
        {
            grab1.interactionManager.SelectEnter(interactor2, grab1);
        }

        // Optional cooldown before allowing another swap
        yield return new WaitForSeconds(1f);
        hasSwapped = false;
    }
}
