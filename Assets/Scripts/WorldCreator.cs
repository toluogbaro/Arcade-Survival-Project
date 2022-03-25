using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreator : MonoBehaviour
{
    public int chunkSize;
    bool gameStarted;
    static MapGenerator mapGenerator;
    //public MeshData meshData;

    void Start()
    {

        
        new TerrainChunk(chunkSize);
        mapGenerator = FindObjectOfType<MapGenerator>();

    }

    public class TerrainChunk
    {
       
        GameObject meshObject;
        Vector2 position;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshData meshData;

        
        public TerrainChunk(int size)
        {

            GenerateMeshData(meshData);
            Vector3 positionV3 = new Vector3(0, 0, 0);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshObject.transform.localScale = Vector3.one * size;
            meshObject.transform.position = positionV3;
            

        }

        void GenerateMeshData(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }

    }

    
}
