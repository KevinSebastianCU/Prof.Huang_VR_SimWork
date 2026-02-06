using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

using System.Collections;
using System.Collections.Generic;


public class Object_Collisions : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;

    public GameObject st_obj;

    
    void OnCollisionEnter(Collision collision)
    {
        GameObject hitStationary = collision.gameObject;

        if (gameObject == obj1 && hitStationary == st_obj)
        {
            Vector3 obj1_pos = obj1.transform.position;
            Vector3 obj2_pos = obj2.transform.position;

            Quaternion obj1_rot = obj1.transform.rotation;
            Quaternion obj2_rot = obj2.transform.rotation;

            obj1.transform.SetPositionAndRotation(obj2_pos, obj2_rot);
            obj2.transform.SetPositionAndRotation(obj1_pos, obj2_rot);
        }
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
