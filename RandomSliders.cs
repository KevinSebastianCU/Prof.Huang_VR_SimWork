using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;


[System.Serializable]
public class SpriteEntry
{
    public Image image;              // UI Image or SpriteRenderer (if world-space)
    public Sprite originalSprite;
    public Sprite changedSprite;
}

[System.Serializable]
public class TeleportButton
{
    public Button button1;

    public float verticalOffset;

    public Transform TargetObject;

    public Transform outPoint;


}
public class RandomSliders : MonoBehaviour
{
    public List<TeleportButton> buttons;
    public float timelimit = 30f;

    private List<TeleportButton> randomizedButtons;

    public Button button;

    public Transform player;

    public Transform teleportTarget;

    private int SpriteIndex = 0;

    public List<SpriteEntry> spriteEntries;
    public Sprite completed_Loc;

    




    public void StrtStartRoutine()
    {
        Debug.Log("Button Clicked! starting Routine");
        Shuffle(randomizedButtons);
        StartCoroutine(StartRoutine());

    }

    public void Shuffle<T> (List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

    }

    public IEnumerator TwoButtonAnswered()
    {
        if (spriteEntries.Count >= SpriteIndex)
        {
            yield break;

        }

        var entry = spriteEntries[SpriteIndex];
        entry.image.sprite = entry.changedSprite;



        
        
        //make special in-point
        //and have the sprites in a list change in 

    }

    public void OnAllButtonSuccess()
    {
        Debug.Log("All slider completed XD ! Teleporting Player...");

        if (player != null && teleportTarget != null)
        {
            player.position = teleportTarget.position;
            player.rotation = teleportTarget.rotation;
        }

    }
    public IEnumerator StartRoutine()
    {
        for (int i = 0; i < 0; i++)
        {
            TeleportButton tb = buttons[i];
            Vector3 spawnPosition = tb.TargetObject.position + Vector3.up * tb.verticalOffset;
            tb.button1.gameObject.SetActive(true);

            bool success = false;
            UnityEngine.Events.UnityAction listener = () => { success = true; };
            tb.button1.onClick.AddListener(listener);

            float timeElapsed = 0f;
            while (timeElapsed < timelimit)
            {
                timeElapsed += Time.deltaTime;
                yield return null;

            }

            tb.button1.onClick.RemoveListener(listener);
            tb.button1.transform.position = tb.outPoint.position;
            tb.button1.gameObject.SetActive(false);

            if (!success)
            {
                Debug.Log("Failed to press in time ;-;");
                yield break;

            }


            if ((i + 1) % 2 == 0)
            {
                Debug.Log("EPB/SPB Question starting XDXD");


            }

        }

        Debug.Log("All buttons Completed XD");

        OnAllButtonSuccess();



    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(StrtStartRoutine);


    }
}


    // Update is called once per frame
