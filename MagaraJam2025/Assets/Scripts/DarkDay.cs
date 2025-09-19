using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class DarkDay : MonoBehaviour
{
    public Tilemap[] tilemaps;
    public SpriteRenderer[] spriteRenderers;
    public bool isDarkTheme = false;
    public float transitionSpeed = 2f;

    private Color targetColor;
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f);
    private Color whiteColor = Color.white;
    private bool lastThemeState;
    private bool isTransitioning = false;

    public GameObject rain;
    public GameObject[] Thunders;

    private Coroutine thunderCoroutine;

    void Start()
    {
        lastThemeState = isDarkTheme;
        targetColor = isDarkTheme ? darkColor : whiteColor;
        SetTilemapsColor(targetColor); // Baþlangýçta doðru renk
    }

    void Update()
    {
        if (lastThemeState != isDarkTheme)
        {
            // Tema deðiþti, geçiþ baþlat
            targetColor = isDarkTheme ? darkColor : whiteColor;
            isTransitioning = true;
            lastThemeState = isDarkTheme;
            rain.SetActive(isDarkTheme);

            if (isDarkTheme)
            {
                if (thunderCoroutine != null)
                    StopCoroutine(thunderCoroutine);
                thunderCoroutine = StartCoroutine(ActivateThundersRandomly());
            }
            else
            {
                if (thunderCoroutine != null)
                    StopCoroutine(thunderCoroutine);
                foreach (var thunder in Thunders)
                {
                    thunder.SetActive(false);
                }
            }
        }

        if (isTransitioning)
        {
            bool finished = true;
            foreach (var tilemap in tilemaps)
            {
                tilemap.color = Color.Lerp(tilemap.color, targetColor, Time.deltaTime * transitionSpeed);
                if (Vector4.Distance(tilemap.color, targetColor) > 0.01f)
                    finished = false;
            }
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * transitionSpeed);
            }
            if (finished)
            {
                SetTilemapsColor(targetColor);
                isTransitioning = false;
            }
        }
    }

    private void SetTilemapsColor(Color color)
    {
        foreach (var tilemap in tilemaps)
        {
            tilemap.color = color;
        }
    }

    private IEnumerator ActivateThundersRandomly()
    {
        // Tüm thunders'ý kapat
        foreach (var thunder in Thunders)
        {
            thunder.SetActive(false);
        }

        // Rastgele sýrayla açmak için index listesi oluþtur
        var indices = new System.Collections.Generic.List<int>();
        for (int i = 0; i < Thunders.Length; i++)
            indices.Add(i);

        var rand = new System.Random();

        while (indices.Count > 0)
        {
            int idx = rand.Next(indices.Count);
            Thunders[indices[idx]].SetActive(true);
            indices.RemoveAt(idx);

            float waitTime = Random.Range(0.75f, 2.5f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
