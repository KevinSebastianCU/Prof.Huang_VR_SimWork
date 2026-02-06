using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Lumin;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

[System.Serializable]
public class Sprite1
{
    public Image image;
    public Sprite originalSprite;
    public Sprite changedSprite;
}

[System.Serializable]
public class GrainSprite
{
    public Image image1;
    public Sprite originalSprite1;
    public Sprite changedSprite1;
}

[System.Serializable]
public class teleportButton
{
    public Button button1;
    public float verticalOffset;
    public Transform TargetObject;
    public Transform outPoint;
}

public class RandomButtonsV2 : MonoBehaviour
{
    public List<teleportButton> buttons;
    public float timeLimit = 45f;
    public Button button;
    public Transform player;
    public Transform teleportTarget;
    private List<teleportButton> randomizedButtons;
    private int SpriteIndex = 0;
    private bool interrupted = false;

    private bool restore = false;

    public List<Sprite1> spriteEntries;

    public List<GrainSprite> spriteEntries2;
    public Sprite completedLoc;
    public Button SpriteRestoredButton;

    public void StrtRoutine()
    {
        Debug.Log("Button Clicked! Game starting");
        randomizedButtons = new List<teleportButton>(buttons);
        Shuffle(randomizedButtons);
        StartCoroutine(StartRoutine());
    }

    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public IEnumerator TwoButtonAnswered()
    {
        if (SpriteIndex >= spriteEntries.Count)
        {
            yield break;
        }

        var entry = spriteEntries[SpriteIndex];
        entry.image.sprite = entry.changedSprite;
        SpriteRestoredButton.gameObject.SetActive(true);

        UnityEngine.Events.UnityAction restoreListener = () =>
        {
            entry.image.sprite = entry.originalSprite;
            restore = true;
            interrupted = true;
            SpriteRestoredButton.gameObject.SetActive(false);
        };

        SpriteRestoredButton.onClick.AddListener(restoreListener);
        SpriteIndex++;
    }

    /*public IEnumerator TwoButtonAnswered1()
    {
        if (SpriteIndex >= spriteEntries.Count)
        {
            yield break;
        }

        var entry = spriteEntries2[SpriteIndex];
        entry.image1.sprite = entry.changedSprite1;
        SpriteRestoredButton.gameObject.SetActive(true);

        UnityEngine.Events.UnityAction restoreListener = () =>
        {
            entry.image1.sprite = entry.originalSprite1;
            restore = true;
            interrupted = true;
            SpriteRestoredButton.gameObject.SetActive(false);
        };

        SpriteRestoredButton.onClick.AddListener(restoreListener);
        SpriteIndex++;
    }*/

    public void OnAllButtonSuccess()
    {
        Debug.Log("All Buttons Completed!!!");

        if (player != null && teleportTarget != null)
        {
            player.position = teleportTarget.position;
            player.rotation = teleportTarget.rotation;
        }
    }

    public IEnumerator StartRoutine()
    {

        for (int i = 0; i < randomizedButtons.Count; i++)
        {
            Debug.Log("StartRoutine entered, randomizedButtons count = " + randomizedButtons.Count);
            teleportButton tb = randomizedButtons[i];
            
            Debug.Log("Loop step " + i);

            Debug.Log($"Index {i}: tb={tb}, button1={(tb != null ? tb.button1 : null)}");
            if (tb != null && tb.button1 != null)
                {
                    Debug.Log("Button1 name: " + tb.button1.name + " (scene object)");
                }

            
            
            if (tb == null || tb.button1 == null)
            {
                Debug.LogError("Button or teleportButton is null at index " + i);
                yield break;
            }
            //Vector3 spawnPosition = tb.TargetObject.position + Vector3.up * tb.verticalOffset;
            //tb.button1.transform.position = spawnPosition;
            /*tb.button1.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            tb.button1.gameObject.SetActive(true);
            Debug.Log("Activated button " + i + " at position " + tb.button1.transform.position);*/

            bool success = false;
            UnityEngine.Events.UnityAction Listener = () => { success = true; };
            tb.button1.onClick.AddListener(Listener);

            float timeElapsed = 0f;
            while (timeElapsed < timeLimit && !success)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            tb.button1.onClick.RemoveListener(Listener);
            tb.button1.transform.position = tb.outPoint.position;
            tb.button1.gameObject.SetActive(false);

            if (!success)
            {
                Debug.Log("Failed to Press in Time");
                yield break;
            }

            if ((i + 1) % 2 == 0)
            {
                Debug.Log("EPB/SPB Question Starting");
                yield return StartCoroutine(TwoButtonAnswered());
            }

        }

        Debug.Log("All Buttons Completed!! XD");
        OnAllButtonSuccess();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(StrtRoutine);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
