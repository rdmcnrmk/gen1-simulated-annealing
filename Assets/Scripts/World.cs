using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// holds all the locations.
/// Also, it calculates the total loop distance between all locations.
/// </summary>
public class World
{
    /// <summary>
    /// location list
    /// </summary>
    public List<Vector2> Locations { get; set; }

    /// <summary>
    /// constructs a blank world
    /// </summary>
    public World()
    {
        Locations = new List<Vector2>();
    }

    /// <summary>
    /// constructs a world with defined locations
    /// </summary>
    /// <param name="locations"></param>
    public World(List<Vector2> locations)
    {
        Locations = locations;
    }

    /// <summary>
    /// gets the total distance of the world
    /// </summary>
    /// <returns></returns>
    public float GetDistance()
    {
        float totalDistance = 0;

        for (int locIndex = 0; locIndex < Locations.Count; locIndex++)
        {
            Vector2 fromLoc = Locations[locIndex];
            Vector2 toLoc;

            if (locIndex + 1 < Locations.Count)
            {
                toLoc = Locations[locIndex + 1];
            }
            else
            {
                toLoc = Locations[0];
            }

            totalDistance += Vector2.Distance(fromLoc, toLoc);
        }
        return totalDistance;
    }
}
