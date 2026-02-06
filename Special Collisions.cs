using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;


public class SpecialCollisions : MonoBehaviour

{
     public GameObject Moving_Object; // The object that triggers the swap on collision
    public GameObject Stationary_Object; // Stationary Object A
    public GameObject Second_Stationary_Object; // Stationary Object B
    public GameObject ConcreteObject; // The concrete to remove
    public GameObject TextPrefab; // A world-space TextMeshPro prefab
    public Vector3 TextOffset = new Vector3(0, 0.5f, 0); // Offset above second object

    private bool hasSwapped = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Trigger only once and only if correct object collides
        if (!hasSwapped && collision.gameObject == Moving_Object)
        {
            Debug.Log("Collision detected with Moving Object!");

            if (Stationary_Object != null && Second_Stationary_Object != null)
            {
                // Swap positions and rotations
                Vector3 posA = Stationary_Object.transform.position;
                Quaternion rotA = Stationary_Object.transform.rotation;

                Vector3 posB = Second_Stationary_Object.transform.position;
                Quaternion rotB = Second_Stationary_Object.transform.rotation;

                Stationary_Object.transform.SetPositionAndRotation(posB, rotB);
                Second_Stationary_Object.transform.SetPositionAndRotation(posA, rotA);

                Debug.Log("Swapped positions between Stationary A and B.");
            }

            // Remove the concrete object
            if (ConcreteObject != null)
            {
                Destroy(ConcreteObject);
                Debug.Log("Concrete object destroyed.");
            }

            // Display text above the new position of Second_Stationary_Object
            if (TextPrefab != null)
            {
                Vector3 textPos = Second_Stationary_Object.transform.position + TextOffset;
                Instantiate(TextPrefab, textPos, Quaternion.identity);
                Debug.Log("Text prefab instantiated above Second_Stationary_Object.");
            }

            hasSwapped = true;
        }
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
