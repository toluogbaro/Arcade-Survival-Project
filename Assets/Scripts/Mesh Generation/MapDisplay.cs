using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    GameObject meshObject;
    public Material material;
    MeshCollider meshCollider;

    public float chunkSize;
    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Material material)
    {
        Vector3 positionV3 = new Vector3(0, 0, 0);
        chunkSize = MapGenerator.mapChunkSize;
        meshObject = new GameObject("Terrain Chunk");
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        meshCollider = meshObject.AddComponent<MeshCollider>();
        
        meshFilter.mesh = meshData.CreateMesh();
        meshCollider.sharedMesh = meshFilter.mesh;
         
        Debug.Log(meshFilter.mesh);
        meshObject.transform.position = positionV3;
        meshObject.transform.localScale = Vector3.one * chunkSize / 10;

    }
}
