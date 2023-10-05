using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorLerpTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public Color color1;
    public Color color2;
    public float duration = 2.0f;

    private void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned!");
            return;
        }

        StartCoroutine(LerpColor());
    }

    private IEnumerator LerpColor()
    {
        float elapsed;
        while (true) // Infinite loop to continuously lerp between colors
        {
            elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                tilemap.color = Color.Lerp(color1, color2, t);
                yield return null;
            }

            // Swap the colors after reaching one of them
            Color temp = color1;
            color1 = color2;
            color2 = temp;
        }
    }
}
