using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreator : MonoBehaviour
{
    int chunkSize;
    bool gameStarted;

    void Start()
    {
        chunkSize = MapGenerator.mapChunkSize - 1;

        if (gameStarted == false)
        {
            
        } else
        {
            new TerrainChunk(chunkSize);
            gameStarted = true;
        }
            
    }

    public class TerrainChunk
    {

        GameObject meshObject;
        
        public TerrainChunk(int size)
        {
            
            Vector3 positionV3 = new Vector3(0, 0, 0);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size / 10f;

        }

    }

    
}
