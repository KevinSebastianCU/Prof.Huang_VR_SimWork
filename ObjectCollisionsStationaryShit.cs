using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class ObjectCollisionsStationaryShit : MonoBehaviour
{
    public GameObject Moving_Object11;
    public GameObject Moving_Object22;
    public GameObject Stationary_Object11;
    public GameObject Stationary_Object22;

    private bool hasSwapped1 = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only react if touching the stationary object
        if (hasSwapped1 || other.gameObject != Stationary_Object11)
            return;

        if (Moving_Object11 == null || Moving_Object22 == null)
            return;

        XRGrabInteractable grab1 = Moving_Object11.GetComponent<XRGrabInteractable>();
        XRGrabInteractable grab2 = Moving_Object22.GetComponent<XRGrabInteractable>();

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
        Vector3 pos1 = Moving_Object11.transform.position;
        Quaternion rot1 = Moving_Object11.transform.rotation;

        Vector3 sta_pos = Stationary_Object11.transform.position;
        Quaternion sta_rot = Stationary_Object11.transform.rotation;


        Moving_Object11.transform.SetPositionAndRotation(Moving_Object22.transform.position, Moving_Object22.transform.rotation);
        Moving_Object22.transform.SetPositionAndRotation(pos1, rot1);

        Stationary_Object11.transform.SetPositionAndRotation(Stationary_Object22.transform.position, Stationary_Object22.transform.rotation);
        Stationary_Object22.transform.SetPositionAndRotation(sta_pos, sta_rot);

        hasSwapped1 = true;

        // Re-enable and re-grab
        StartCoroutine(ReenableAndRegrab1(grab1, grab2, interactor1, interactor2));
    }

    private IEnumerator ReenableAndRegrab1(
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
        hasSwapped1 = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
