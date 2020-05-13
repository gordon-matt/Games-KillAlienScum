using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelMaker : MonoBehaviour
{
    public AudioSource MusicAudioSource;

    public AudioClip[] Music;

    public GameObject Layer_Background;

    public GameObject Layer_BackgroundElements;

    public GameObject Layer_Middleground1;

    public GameObject Layer_Middleground2;

    public GameObject Layer_Middleground3;

    public GameObject Layer_Foreground;

    public GameObject[] Nebulae; // Parent: Layer_BackgroundElements

    public GameObject[] Stars;

    public GameObject[] Planets;

    public GameObject[] Moons;

    public GameObject[] Asteroids;

    public GameObject[] Enemies;

    public GameObject HealthPack;

    public GameObject ZapAttack;

    private List<int> planetsSeenIndexes = new List<int>();
    private string[] asteroidColors = new[] { "Brown", "Gray", "Red" };

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks); // // Initialize seed for Random

        PlayRandomMusic();

        var nebula = Nebulae[Random.Range(0, Nebulae.Length)];
        Instantiate(nebula, Vector3.zero, Quaternion.identity, Layer_BackgroundElements.transform);

        StartCoroutine(SpawnCelestialBodies());
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        if (!MusicAudioSource.isPlaying)
        {
            PlayRandomMusic();
        }

        // The LevelMaker is as good a place as any for this...
        if (Input.GetKey((KeyCode.Escape)))
        {
            Application.Quit();
        }
    }

    private IEnumerator SpawnCelestialBodies()
    {
        // If this is the first thing generated (game just started), then we want to show something special (but not every time.. just more likely)
        bool isFirst = true;

        while (true)
        {
            // Position just to right of camera (location to spawn things)
            float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

            // Chance to spawn a combination of star, planet and moon together
            bool generateCombo = isFirst
                ? Random.Range(1, 4) == 1
                : Random.Range(1, 7) == 1;

            // Change to generate an asteroid field
            bool generateAsteroidField = Random.Range(1, 5) == 1;

            if (generateCombo)
            {
                // We only spawn a star if it's the first thing generated.
                SpawnCombo(rightBorder, isFirst);
                isFirst = false;
            }
            else if (generateAsteroidField)
            {
                SpawnAsteroidField(rightBorder);
            }
            else
            {
                // Chance to spawn a star (we don't want them too often, as that would be weird to have so many stars seen so close together).
                bool generateStar = Random.Range(1, 7) == 1;

                if (isFirst || generateStar)
                {
                    // Chance to spawn twin stars
                    bool twinStars = isFirst
                        ? Random.Range(1, 2) == 1
                        : Random.Range(1, 10) == 1;

                    if (twinStars)
                    {
                        // If we have twin stars, we want to keep the parallax, to better to keep one of them bigger than the other..
                        //  thus we split the stars into large and "other" (medium + small).
                        var largeStars = Stars.Where(x => x.name.Contains("Large"));
                        var otherStars = Stars.Where(x => !x.name.Contains("Large"));

                        // Decide whether the large star should be in front for parallax or behind.
                        var largeStarInFront = Random.Range(1, 3) == 1;

                        SpawnStar(
                            rightBorder,
                            largeStars,
                            layer: largeStarInFront ? Layer.Middleground2 : Layer.Middleground1); // Assign parent layer for parallax

                        SpawnStar(
                            largeStarInFront
                                ? rightBorder - Random.Range(5, 10)
                                : rightBorder + Random.Range(5, 10),
                            otherStars,
                            layer: largeStarInFront ? Layer.Middleground1 : Layer.Middleground2); // Assign parent layer for parallax
                    }
                    else
                    {
                        // Else just spawn a single star
                        SpawnStar(rightBorder, Stars);
                    }

                    isFirst = false;
                }
                else
                {
                    // Else spawn a planet
                    float planetSize = Random.Range(0.2f, 1f);
                    SpawnPlanet(rightBorder, planetSize);
                }
            }

            yield return new WaitForSeconds(Random.Range(30, 60) + Random.value);
        }
    }

    private void SpawnCombo(float rightBorder, bool spawnStar)
    {
        var starLocation = new Vector3(rightBorder, Random.Range(2, 8), 0);

        if (spawnStar)
        {
            var smallStars = Stars.Where(x => x.name.Contains("Small"));
            SpawnStar(rightBorder, smallStars, starLocation);
        }

        var planetLocation = spawnStar
            ? new Vector3(starLocation.x + 11.5f, starLocation.y, 0)
            : starLocation;

        float planetSize = Random.Range(0.75f, 1.5f);
        SpawnPlanet(rightBorder, planetSize, planetLocation);

        bool moveMoonDown = Random.Range(1, 3) == 1;

        float moonY = moveMoonDown && starLocation.y <= 4
            ? planetLocation.y + Random.Range(1f, 4f)
            : planetLocation.y - Random.Range(1f, 4f);

        var moonLocation = new Vector3(planetLocation.x + 6f, moonY, 0);

        SpawnMoon(rightBorder, 0.2f, moonLocation);
    }

    private void SpawnStar(
        float rightBorder,
        IEnumerable<GameObject> stars,
        Vector3? position = null,
        Layer layer = Layer.Middleground2)
    {
        var objectToCreate = stars.ElementAt(Random.Range(0, stars.Count()));

        var renderer = objectToCreate.GetComponent<Renderer>();
        renderer.sortingLayerName = GetSortingLayerName(layer);
        renderer.sortingOrder = 0;

        Instantiate(
            objectToCreate,
            position ?? new Vector3(rightBorder, Random.Range(2, 8), 0),
            Quaternion.identity,
            GetLayerTransform(layer));
    }

    private void SpawnPlanet(float rightBorder, float planetSize, Vector3? position = null)
    {
        // If we have showed all the planets already..
        if (planetsSeenIndexes.Count >= Planets.Length)
        {
            // .. clear the list of seen planets, so we start again.
            planetsSeenIndexes.Clear();
        }

        // Get a random planet from the array
        int index = Random.Range(0, Planets.Length);

        // If we have already instantiated this particular planet recently...
        while (planetsSeenIndexes.Contains(index))
        {
            // Then choose another one..
            index = Random.Range(0, Planets.Length);
        }

        var objectToCreate = Planets[index];
        planetsSeenIndexes.Add(index);

        objectToCreate.transform.localScale = new Vector3(planetSize, planetSize);

        var renderer = objectToCreate.GetComponent<Renderer>();
        renderer.sortingLayerName = "Middleground 2";
        renderer.sortingOrder = 0;

        Instantiate(
            objectToCreate,
            position ?? new Vector3(rightBorder, Random.Range(2, 8), 0),
            // This is to rotate the planet, so we have more diversity.. but unfortunately there is one small issue: most of the planet textures
            //  each have a shaded part to them. So it does look weird if the light from the local star appears to shade each planet differently.
            //  Ideally if I ever released this as an actual game, it would be best to have no shading at all on the textures and then provide
            //  my own shading based on something like where we placed the star.. like making the sun a light source and wrapping the planet
            //  textures around a sphere maybe...
            Quaternion.Euler(0, 0, Random.Range(0f, 300f)),
            Layer_Middleground2.transform);
    }

    private void SpawnMoon(float rightBorder, float moonSize, Vector3? position = null)
    {
        var objectToCreate = Planets[Random.Range(0, Moons.Length)];

        objectToCreate.transform.localScale = new Vector3(moonSize, moonSize);

        var renderer = objectToCreate.GetComponent<Renderer>();
        renderer.sortingLayerName = "Middleground 3";
        renderer.sortingOrder = 0;

        Instantiate(
            objectToCreate,
            position ?? new Vector3(rightBorder, Random.Range(2, 8), 0),
            Quaternion.identity,
            Layer_Middleground3.transform);
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Position just to right of camera (location to spawn enemies)
            float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

            // Random enemy (although at the moment we only have 1 enemy prefab)
            var enemy = Enemies[Random.Range(0, Enemies.Length)];

            // Random size for enemy
            var enemySize = Random.Range(0.1f, 0.2f);
            enemy.transform.localScale = new Vector3(enemySize, enemySize);

            Instantiate(
                enemy,
                new Vector3(rightBorder, Random.Range(2, 8), 0), // Random Y location (near top, middle or bottom of screen)
                Quaternion.identity);

            // TODO: Decrease time based on how long played for to make it harder
            yield return new WaitForSeconds(Random.Range(1, 3) + Random.value);
        }
    }

    private void SpawnAsteroidField(float rightBorder)
    {
        // Random number of asteroids
        int asteroidCount = Random.Range(15, 45);

        // Min and max X position (for how wide the asteroid field will be).
        float xMin = rightBorder;
        float xMax;

        bool denseField = Random.Range(1, 2) == 1; // sparse vs dense asteroid field..
        if (denseField)
        {
            xMax = xMin + 30;
        }
        else
        {
            xMax = xMin + 70;
        }

        // It looks a little bit weird having all the different colored asteroids.
        //  Much better to choose a single color and make 90% of them that color and then other 10% as the other 2 colors.
        string primaryRoidColor = asteroidColors[Random.Range(0, asteroidColors.Length)];

        for (int i = 0; i < asteroidCount; i++)
        {
            // Random position within the range of xMin and xMax and within defined bounds for y position as well.
            var position = new Vector3(
                Random.Range(xMin, xMax),
                Random.Range(1.5f, 8.5f),
                0);

            GameObject prefab = null;

            bool choosePrimaryRoid = Random.value >= 0.1f; // 90% chance for primary colored asteroids
            if (choosePrimaryRoid)
            {
                var asteroids = Asteroids.Where(x => x.name.Contains(primaryRoidColor));
                prefab = asteroids.ElementAt(Random.Range(0, asteroids.Count()));
            }
            else
            {
                var asteroids = Asteroids.Where(x => !x.name.Contains(primaryRoidColor));
                prefab = asteroids.ElementAt(Random.Range(0, asteroids.Count()));
            }

            var asteroid = Instantiate(prefab, position, Quaternion.identity, Layer_Middleground3.transform);

            // Randomize Size
            float scale = Random.Range(0.5f, 2f);
            asteroid.transform.localScale = new Vector3(scale, scale, 0);

            // Randomize Initial Rotation

            GameObject gameObjectToRotate = null;

            var spriteRenderer = asteroid.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Middleground 3";
                spriteRenderer.sortingOrder = 0;

                gameObjectToRotate = spriteRenderer.gameObject;
            }

            if (gameObjectToRotate != null)
            {
                int rotation = Random.Range(0, 359);
                gameObjectToRotate.transform.Rotate(0, 0, rotation);
            }
        }
    }

    private void PlayRandomMusic()
    {
        MusicAudioSource.clip = Music[Random.Range(0, Music.Length)];
        MusicAudioSource.Play();
    }

    private enum Layer : byte
    {
        Background,
        BackgroundElements,
        Middleground1,
        Middleground2,
        Middleground3,
        Foreground
    }

    private static string GetSortingLayerName(Layer layer)
    {
        switch (layer)
        {
            case Layer.Background: return "Background";
            case Layer.BackgroundElements: return "Background Particles";
            case Layer.Middleground1: return "Middleground 1";
            case Layer.Middleground2: return "Middleground 2";
            case Layer.Middleground3: return "Middleground 3";
            case Layer.Foreground: return "Foreground";
            default: return "Middleground 2";
        }
    }

    private Transform GetLayerTransform(Layer layer)
    {
        switch (layer)
        {
            case Layer.Background: return Layer_Background.transform;
            case Layer.BackgroundElements: return Layer_BackgroundElements.transform;
            case Layer.Middleground1: return Layer_Middleground1.transform;
            case Layer.Middleground2: return Layer_Middleground2.transform;
            case Layer.Middleground3: return Layer_Middleground3.transform;
            case Layer.Foreground: return Layer_Foreground.transform;
            default: return Layer_Middleground2.transform;
        }
    }
}