using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// http://www.theprojectspot.com/tutorial-post/simulated-annealing-algorithm-for-beginners/6
/// </summary>

public class SimulatedAnnealing
{
    /// <summary>
    /// all solution steps in an annealing process
    /// </summary>
    public List<List<Vector2>> solutions = new List<List<Vector2>>();

    /// <summary>
    /// initial problem 
    /// </summary>
    private World initialWorld;

    /// <summary>
    /// initial temp
    /// </summary>
    private float temp = 100000f;

    /// <summary>
    /// cooling rate
    /// </summary>
    float coolingRate = 0.003f;

    /// <summary>
    /// creates a random world
    /// </summary>
    public SimulatedAnnealing()
    {
        initialWorld = new World();
        for (int i = 0; i < 30; i++)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(5, 50);
            initialWorld.Locations.Add(new Vector2(r * Mathf.Sin(angle), r * Mathf.Cos(angle)));
        }
    }

    public SimulatedAnnealing(float temp, float coolingRate, World world)
    {
        this.temp = temp;
        this.coolingRate = coolingRate;
        this.initialWorld = world;
    }

    /// <summary>
    /// calculate the acceptance probability.
    /// if current energy is better than previous one accept that solution.
    /// else, calculate a new acceptance probability
    /// </summary>
    /// <param name="energy"> previous energy parameter </param>
    /// <param name="newEnergy"> current energy parameter </param>
    /// <param name="temperature"> used for probability calculation </param>
    /// <returns></returns>
    public static float AcceptanceProbability(float energy, float newEnergy, float temperature)
    {
        if (newEnergy < energy)
        {
            return 1.0f;
        }

        return Mathf.Exp((energy - newEnergy) / temperature);
    }

    public void Findsolution()
    {
        // Create and add our cities
        World currentSolution = new World(new List<Vector2>(initialWorld.Locations));

        // Randomly reorder the tour
        currentSolution.Locations.Shuffle();

        // Set as current best
        World best = new World(new List<Vector2>(currentSolution.Locations));

        // Loop until system has cooled
        while (temp > 1)
        {
            // Create new neighbour tour
            World newSolution = new World(new List<Vector2>(currentSolution.Locations));

            // Get a random positions in the tour
            int tourPos1 = (int)(Random.Range(0, newSolution.Locations.Count));
            int tourPos2 = (int)(Random.Range(0, newSolution.Locations.Count));

            // Get the cities at selected positions in the tour
            Vector2 citySwap1 = newSolution.Locations[tourPos1];
            Vector2 citySwap2 = newSolution.Locations[tourPos2];

            // Swap them
            newSolution.Locations[tourPos2] = citySwap1;
            newSolution.Locations[tourPos1] = citySwap2;

            // Get energy of solutions
            float currentEnergy = currentSolution.GetDistance();
            float neighbourEnergy = newSolution.GetDistance();

            // Decide if we should accept the neighbour
            if (AcceptanceProbability(currentEnergy, neighbourEnergy, temp) > Random.Range(0f, 1f))
            {
                currentSolution = new World(new List<Vector2>(newSolution.Locations));
            }

            // Keep track of the best solution found
            if (currentSolution.GetDistance() < best.GetDistance())
            {
                best = new World(new List<Vector2>(currentSolution.Locations));
            }

            // Cool system
            temp *= 1 - coolingRate;
            solutions.Add(currentSolution.Locations);
        }
    }
}