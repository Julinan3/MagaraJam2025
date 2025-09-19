using UnityEngine;
using UnityEngine.Tilemaps;

public class DarkDay : MonoBehaviour
{
    public Tilemap[] tilemaps;
    public bool isDarkTheme = false;
    public float transitionSpeed = 2f;

    private Color targetColor;
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f);
    private Color whiteColor = Color.white;
    private bool lastThemeState;
    private bool isTransitioning = false;

    public GameObject rain;

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
}
