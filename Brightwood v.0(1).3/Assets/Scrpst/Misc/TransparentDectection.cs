using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = .4f;

    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;

    private void Awake() {
           spriteRenderer = GetComponent<SpriteRenderer>();
           tilemap = GetComponent<Tilemap>();
           Debug.Log($"Awake: spriteRenderer is {(spriteRenderer ? "found" : "null")}, tilemap is {(tilemap ? "found" : "null")}");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!gameObject.activeInHierarchy || !enabled) return;
        Debug.Log($"OnTriggerEnter2D: Triggered by {other.gameObject.name}");
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player) {
            Debug.Log("PlayerController detected");
            if (spriteRenderer) {
                Debug.Log("Fading SpriteRenderer");
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
            } else if (tilemap) {
                Debug.Log("Fading Tilemap");
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            } else {
                Debug.LogWarning("No SpriteRenderer or Tilemap found!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!gameObject.activeInHierarchy || !enabled) return;
        Debug.Log($"OnTriggerExit2D: Triggered by {other.gameObject.name}");
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            Debug.Log("PlayerController detected");
            if (spriteRenderer) {
                Debug.Log("Restoring SpriteRenderer alpha");
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));
            } else if (tilemap) {
                Debug.Log("Restoring Tilemap alpha");
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            } else {
                Debug.LogWarning("No SpriteRenderer or Tilemap found!");
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency) {
        Debug.Log($"FadeRoutine(SpriteRenderer): from {startValue} to {targetTransparency} over {fadeTime}s");
        float elapsedTime = 0;     
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
        Debug.Log($"FadeRoutine(SpriteRenderer) finished. Final alpha: {spriteRenderer.color.a}");
    }

    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        Debug.Log($"FadeRoutine(Tilemap): from {startValue} to {targetTransparency} over {fadeTime}s");
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
        Debug.Log($"FadeRoutine(Tilemap) finished. Final alpha: {tilemap.color.a}");
    }
}
