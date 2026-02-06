using UnityEngine;
using UnityEngine.UI;

public class TeleportationScript : MonoBehaviour
{
    public Transform player;
    public Transform teleportation;

    public Button TP_Button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void TeleportPlayer()
    {
        player.position = teleportation.position;
        player.rotation = teleportation.rotation;

    }
    void Start()
    {
        TP_Button.onClick.AddListener(TeleportPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
