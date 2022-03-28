using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreator : MonoBehaviour
{
    public int chunkSize;
    bool gameStarted;

    void Start()
    {
        new TerrainChunk(chunkSize);

    }

    public class TerrainChunk
    {

        GameObject meshObject;
        
        public TerrainChunk(int size)
        {
            
            

            

        }

    }

    
}
