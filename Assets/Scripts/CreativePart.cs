using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativePart : MonoBehaviour
{

    [SerializeField]
    private Material mat;

    [SerializeField]
    private float initialTemp = 100000f;

    [SerializeField]
    private float coolingRate = 0.003f;

    [SerializeField]
    private int locationCount = 30;

    [SerializeField]
    private float minWorldRadius = 5f;

    [SerializeField]
    private float maxWorldRadius = 50f;

    [SerializeField]
    private int complexifyPower = 5;

    [SerializeField]
    private float stdScale = 0.1f;

    [SerializeField]
    private GameObject targetObj;

    private List<Vector3> paths = new List<Vector3>();

    private SimulatedAnnealing sa;

    void Start()
    {
        World initialWorld = new World();

        for (int i = 0; i < locationCount; i++)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(minWorldRadius, maxWorldRadius);
            initialWorld.Locations.Add(new Vector3(r * Mathf.Sin(angle), r * Mathf.Cos(angle), r * Mathf.Cos(angle)));
        }

        VertexProvider vProvider = new VertexProvider(targetObj.GetComponent<MeshFilter>().mesh, targetObj.transform);
        initialWorld.Locations = vProvider.GetVertexSamples(1000);

        sa = new SimulatedAnnealing(initialTemp, coolingRate, initialWorld);
        sa.Findsolution();

        // Prepare camera for render
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.white;
        StartCoroutine(DontClearCamera());
    }

    private IEnumerator DontClearCamera()
    {
        yield return new WaitForEndOfFrame();
        Camera.main.clearFlags = CameraClearFlags.Nothing;
    }

    /// <summary>
    /// add randomness to the path
    /// </summary>
    /// <param name="pathPoints"> input positions</param>
    /// <returns> noised positions </returns>
    List<Vector3> ComplexifyPath(List<Vector3> pathPoints)
    {
        //create a new path array from the old one by adding new points inbetween the old points
        List<Vector3> newPath = new List<Vector3>();

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Vector3 v1 = pathPoints[i];
            Vector3 v2 = pathPoints[i + 1];
            Vector3 midPoint = (v1 + v2) * 0.5f;
            float distance = Vector3.Distance(v1, v2);

            //the new point is halfway between the old points, with some gaussian variation
            float standardDeviation = stdScale * distance;
            Vector3 v = new Vector3(midPoint.x + Random.Range(-standardDeviation, standardDeviation),
                                    midPoint.y + Random.Range(-standardDeviation, standardDeviation),
                                    midPoint.z + Random.Range(-standardDeviation, standardDeviation));
      
            newPath.Add(v1);
            newPath.Add(v);
        }

        //add the last point
        newPath.Add(pathPoints[pathPoints.Count - 1]);
        return newPath;
    }

    int counter = 0;
    void OnPostRender()
    {
        if (counter < sa.solutions.Count-1)
        {
            counter++;
        }

        counter = sa.solutions.Count - 1;
        paths = sa.solutions[counter].Item1;
        for (var j = 0; j < complexifyPower; j++)
        {
            paths = ComplexifyPath(paths);
        }

        GL.PushMatrix();
        mat.SetPass(0);

        GL.Begin(GL.LINES);

        for (var i = 0; i < paths.Count - 1; i++)
        {
            GL.Vertex(paths[i]);
            GL.Vertex(paths[i+1]);
        }

        GL.End();
        GL.PopMatrix();
    }
}

