using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public Terrain terrainToRandomize;
    
    public Vector3 terrainSize = new Vector3(100, 25, 100);
    public float minOffset = 100;
    public float maxOffset = 1000;
    public float perlinCoordSpacing = 0.01f;

    void Start()
    {
        //Set the terrain to the size of the size we set in the editor
        terrainToRandomize.terrainData.size = terrainSize;
        terrainToRandomize.transform.position = new Vector3(-(terrainSize.x / 2), 0, -(terrainSize.z / 2));

        //Set where the perlin noise starts
        float xOffset = Random.Range(minOffset, maxOffset);
        float yOffset = Random.Range(minOffset, maxOffset);

        PerlinizeTerrain(terrainToRandomize, xOffset, yOffset, perlinCoordSpacing);
    }

    /// <summary>
    /// Set a terrain's heightmap to a chunk of perlin noise.
    /// </summary>
    /// <param name="terrainToPerlinize">Terrain to give a perlin height map.</param>
    /// <param name="xOffset">The starting X coord of the perlin space we're getting.</param>
    /// <param name="yOffset">The starting Y coord of the perlin space we're getting.</param>
    /// <param name="spacing">How close the entries of the height map are on the perlin space. Smaller = smoother</param>
    private void PerlinizeTerrain(Terrain terrainToPerlinize, float xOffset, float yOffset, float spacing)
    {
        //Make a height array
        float[,] heightArray = new float[terrainToPerlinize.terrainData.heightmapResolution, terrainToPerlinize.terrainData.heightmapResolution];

        //Fill the height array
        float xVal = xOffset;
        float zVal = yOffset;

        //For each row,
        for (int i = 0; i < heightArray.GetLength(0); i++)
        {
            //reset x val, because we're at a new row
            xVal = xOffset;

            //for each column,
            for (int j = 0; j < heightArray.GetLength(0); j++)
            {
                //set the height array at our current coordinate to a perlin noise value dictated by x and z vals
                heightArray[i, j] = Mathf.PerlinNoise(xVal, zVal);

                //move over to the next x coordinate
                xVal += spacing;
            }

            //move onto the next z coordinate
            zVal += spacing;
        }

        //finally, the terrain heightmap is done; apply it
        terrainToPerlinize.terrainData.SetHeights(0, 0, heightArray);
    }
}
