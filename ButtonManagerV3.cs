using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TeleportButtonEntry
{
    public Button button;             // The UI button in the scene
    public Transform targetObject;    // World object to spawn above
    public Transform outPoint;        // Where the button goes after it's done
    public float verticalOffset = 1f; // How far above the target to place the button
}

[System.Serializable]
public class SpriteEntry1
{
    public Image image;
    public UnityEngine.Sprite originalSprite;
    public UnityEngine.Sprite changedSprite;
}

public class ButtonManagerV3 : MonoBehaviour
{
    [Header("Gameplay Setup")]
    public List<TeleportButtonEntry> buttons;
    public List<SpriteEntry1> spriteEntries;
    public float timeLimit = 45f;

    [Header("Player Teleport")]
    public Transform player;
    public Transform teleportTarget;

    [Header("UI Controls")]
    public Button startButton;
    public Button restoreSpriteButton;

    private List<TeleportButtonEntry> randomizedButtons;
    private int spriteIndex = 0;

    void Start()
    {
        startButton.onClick.AddListener(() => {
            Debug.Log("Game starting!");
            randomizedButtons = new List<TeleportButtonEntry>(buttons);
            Shuffle(randomizedButtons);
            StartCoroutine(ButtonRoutine());
        });

        restoreSpriteButton.gameObject.SetActive(false);
    }

    private void Shuffle<T>(List<T> list)
    {
        Debug.Log("Shuffling..");
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    private IEnumerator ButtonRoutine()
    {
        Debug.Log("ButtonRoutine entered, count = " + randomizedButtons.Count);
        for (int i = 0; i < randomizedButtons.Count; i++)
        {
            Debug.Log("Loop step " + i);
            var tb = randomizedButtons[i];
            Canvas canvas = tb.button.GetComponentInParent<Canvas>();

            if (tb == null)
            {
                Debug.LogError("TeleportButtonEntry at index " + i + " is null!");
                yield break;
            }
        if (tb.button == null)
        {
            Debug.LogError("Button at index " + i + " is null!");
            yield break;
        }

        Debug.Log("Placing button " + tb.button.name);


            if (canvas != null)
            {
                // Position canvas in world space
                Vector3 spawnPos = tb.targetObject.position + Vector3.up * tb.verticalOffset;
                canvas.transform.position = spawnPos;

                Debug.Log($"Teleported Canvas '{canvas.name}' (Button '{tb.button.name}') to {spawnPos}");
            }
            else
            {
                Debug.LogError($"Button {tb.button.name} has no parent Canvas!");
            }

            // Make sure it's visible
            tb.button.gameObject.SetActive(true);

            bool pressed = false;
            UnityEngine.Events.UnityAction listener = () => { pressed = true; };
            tb.button.onClick.AddListener(listener);

            float timer = 0f;
            while (timer < timeLimit && !pressed)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            tb.button.onClick.RemoveListener(listener);

            if (canvas != null)
            {
                canvas.transform.position = tb.outPoint.position;
            }
            tb.button.gameObject.SetActive(false);


            if (!pressed)
            {
                Debug.Log("Button failed: timeout");
                yield break; // end game early
            }

            // Every two buttons, flip a sprite
            if ((i + 1) % 2 == 0 && spriteIndex < spriteEntries.Count)
            {
                yield return StartCoroutine(SpriteChallenge(spriteEntries[spriteIndex]));
                spriteIndex++;
            }
        }

        Debug.Log("All buttons complete!");
        if (player && teleportTarget)
        {
            player.position = teleportTarget.position;
            player.rotation = teleportTarget.rotation;
        }
    }

    private IEnumerator SpriteChallenge(SpriteEntry1 entry)
    {
        Debug.Log("Changing Spriteimage");
        entry.image.sprite = entry.changedSprite;
        restoreSpriteButton.gameObject.SetActive(true);

        bool restored = false;
        UnityEngine.Events.UnityAction restoreListener = () =>
        {
            entry.image.sprite = entry.originalSprite;
            restored = true;
            restoreSpriteButton.gameObject.SetActive(false);
        };

        restoreSpriteButton.onClick.AddListener(restoreListener);
        while (!restored)
        {
            yield return null;
        }
        restoreSpriteButton.onClick.RemoveListener(restoreListener);
    }
}



