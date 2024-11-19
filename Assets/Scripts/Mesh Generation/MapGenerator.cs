using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap, FalloffMap};
    public DrawMode drawMode;
    [Range(0,241)]
    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;

    public int ocataves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;
    public float meshHeightMultipler;
    public AnimationCurve meshHeightCurve;

    public Material material;

    public bool useFalloff;

    public bool autoUpdate;

    public TerrainType[] regions;

    float[,] fallofMap;

    public GameObject input; 

    private void Awake()
    {
        fallofMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }

    public void Start()
    {
        GenerateMapInEditor();
    }

    public void GenerateSeed()
    {
        int.TryParse(input.GetComponent<InputField>().text, out int n);

        seed = n;

        GenerateMapInEditor();
    }

    public void GenerateMapInEditor()
    {
        if(seed == 0)
        {
            seed = Random.Range(-100000, 100000);
        }

        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, ocataves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - fallofMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }

            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {

            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        } else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
        }

    }

    public void GenerateMeshOnPlay()
    {
        if (seed == 0)
        {
            seed = Random.Range(-100000, 100000);
        }

        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, ocataves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - fallofMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }

            }
        }

        if (GameObject.Find("Terrain Chunk"))
        {
            var terrain = GameObject.Find("Terrain Chunk");
            Destroy(terrain);
            
            MapDisplay display = FindObjectOfType<MapDisplay>();
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultipler, meshHeightCurve, levelOfDetail), material);
        }else
        {
            MapDisplay display = FindObjectOfType<MapDisplay>();
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultipler, meshHeightCurve, levelOfDetail), material);
        }

        
        
    }
    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (ocataves < 0)
        {
            ocataves = 0;
        }
        fallofMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }
}


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

}
