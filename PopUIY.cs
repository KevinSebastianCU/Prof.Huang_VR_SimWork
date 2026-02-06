using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

public class PopUIY : MonoBehaviour
{
     public GameObject xMenuPanel;   // Panel for X button
    public GameObject yMenuPanel;   // Panel for Y button
    public Transform playerHead;    // XR Rig Main Camera
    public float distanceFromPlayer = 2f;

    private InputDevice leftController;

    private bool xMenuOpen = false;
    private bool yMenuOpen = false;

    private Vector3 xHomePos, yHomePos;
    private Quaternion xHomeRot, yHomeRot;

    // track button states to prevent jitter
    private bool prevXPressed = false;
    private bool prevYPressed = false;

    void Start()
    {
        TryInitializeControllers();

        if (xMenuPanel != null)
        {
            xHomePos = xMenuPanel.transform.position;
            xHomeRot = xMenuPanel.transform.rotation;
            xMenuPanel.SetActive(false);
        }

        if (yMenuPanel != null)
        {
            yHomePos = yMenuPanel.transform.position;
            yHomeRot = yMenuPanel.transform.rotation;
            yMenuPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!leftController.isValid)
        {
            TryInitializeControllers();
            return;
        }

        // --- X button check ---
        if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool xPressedNow))
        {
            if (xPressedNow && !prevXPressed) // fire only on "button down"
            {
                if (!xMenuOpen) ShowUI(xMenuPanel, ref xMenuOpen);
                else HideUI(xMenuPanel, xHomePos, xHomeRot, ref xMenuOpen);
            }
            prevXPressed = xPressedNow; // update state
        }

        // --- Y button check ---
        if (leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool yPressedNow))
        {
            if (yPressedNow && !prevYPressed) // fire only on "button down"
            {
                if (!yMenuOpen) ShowUI(yMenuPanel, ref yMenuOpen);
                else HideUI(yMenuPanel, yHomePos, yHomeRot, ref yMenuOpen);
            }
            prevYPressed = yPressedNow; // update state
        }
    }

    void TryInitializeControllers()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
            leftController = devices[0];
    }

    void ShowUI(GameObject panel, ref bool isOpen)
    {
        if (panel == null || playerHead == null) return;

        Vector3 forward = playerHead.forward;
        forward.y = 0;
        forward.Normalize();

        panel.transform.position = playerHead.position + forward * distanceFromPlayer;
        panel.transform.rotation = Quaternion.LookRotation(forward);

        panel.SetActive(true);
        isOpen = true;

        Debug.Log(panel.name + " opened");
    }

    void HideUI(GameObject panel, Vector3 homePos, Quaternion homeRot, ref bool isOpen)
    {
        if (panel == null) return;

        panel.transform.position = homePos;
        panel.transform.rotation = homeRot;

        panel.SetActive(false);
        isOpen = false;

        Debug.Log(panel.name + " closed");
    }
}

