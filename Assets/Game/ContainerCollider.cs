using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ContainerCollider : MonoBehaviour
{
    public int maxHearts = 3;
    private int currentHearts;

    public Image[] heartImages; // Assign your heart UI images here in the Inspector
    public Sprite fullHeart;    // Sprite for full heart
    public Sprite emptyHeart;   // Sprite for empty heart

    public float hitCooldown = 1f; // seconds between heart losses
    private float lastHitTime = -999f;

    public GameObject redOverlay; // UI Image with red overlay (set alpha low like 0.3–0.5)
    public GameObject warningIcon; // Some icon (exclamation mark etc.)
    public float overlayFlashTime = 3f; // How long the red flash lasts

    void Start()
    {
        currentHearts = maxHearts;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Container")) // or "Ground"
        {
            if (Time.time - lastHitTime > hitCooldown && currentHearts > 0)
            {
                StartCoroutine(FlashDamageOverlay());

                currentHearts--;
                lastHitTime = Time.time;
                UpdateHeartsUI();
                Debug.Log("Crashed! Hearts left: " + currentHearts);

                if (currentHearts <= 0)
                {
                    RestartGame();
                }
            }
        }

        void UpdateHeartsUI()
        {
            for (int i = 0; i < heartImages.Length; i++)
            {
                if (i < currentHearts)
                {
                    heartImages[i].sprite = emptyHeart;
                }
                else
                {
                    heartImages[i].sprite = fullHeart;
                }
            }
        }

        IEnumerator FlashDamageOverlay()
        {
            if (redOverlay != null) redOverlay.SetActive(true);
            if (warningIcon != null) warningIcon.SetActive(true);

            yield return new WaitForSeconds(overlayFlashTime);

            if (redOverlay != null) redOverlay.SetActive(false);
            if (warningIcon != null) warningIcon.SetActive(false);
        }

        void RestartGame()
        {
            Debug.Log("No hearts left. Restarting game...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
