using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float turnSpeed = 0.5f;

    void Update()
    {
        MoveLogic();

        //Get the normal of the terrain so we can set the rotation of the object with it.
        Vector3 terrainNormal = GetTerrainNormal(Terrain.activeTerrain, transform.position);

        //LookRotation prioritizes the forward vector, so we have to make our forward vector perpendicular
        //to the normal, thus properly aligning the object with the normal
        transform.forward = transform.forward - terrainNormal * Vector3.Dot(transform.forward, terrainNormal);
        transform.rotation = Quaternion.LookRotation(transform.forward, terrainNormal);

        //Set the height to be the height of the terrain (I found the long decimal myself to make the bottom mostly align)
        transform.position = new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position), transform.position.z);
    }

    private void MoveLogic()
    {
        //Move cactus forward or back moveSpeed units
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0, moveSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, 0, -moveSpeed);
        }

        //Turn cactus by turnSpeed degrees
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.AngleAxis(-turnSpeed, transform.up);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.AngleAxis(turnSpeed, transform.up);
        }
    }

    /// <summary>
    /// Takes a terrain and a position, and gets the normal of the terrain at that position.
    /// Huge thanks to http://answers.unity.com/answers/1295326/view.html for this method; it's been
    /// slightly edited to fit this script, but the idea of using InverseLerp and whatnot came from there.
    /// </summary>
    /// <param name="terrain">The terrain to get the normal from.</param>
    /// <param name="position">The position on the terrain to get the normal from.</param>
    /// <returns>The normal at the given position on the given terrain.</returns>
    private Vector3 GetTerrainNormal(Terrain terrain, Vector3 position)
    {
        //Subtracts the terrain's position from the given position, to get the position relative to the terrain
        Vector3 terrainLocalPos = position - terrain.transform.position;

        //with that relative position, use InverseLerp (get percentage from value) to get how far along the terrain
        //the relative position is, from 0 to 1
        Vector2 normalizedPos = new Vector2(
            Mathf.InverseLerp(0f, terrain.terrainData.size.x, terrainLocalPos.x),
            Mathf.InverseLerp(0f, terrain.terrainData.size.z, terrainLocalPos.z)
        );

        //Finally, now that we have the normalized, local position, we can get the interpolated normal at that position
        var terrainNormal = terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
        return terrainNormal;
    }
}
