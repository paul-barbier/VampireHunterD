using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using Unity.Mathematics;

[ExecuteInEditMode]
public class Bezier_136 : MonoBehaviour
{
    public SplineContainer splineContainer; // Reference to the SplineContainer
    public List<Vector3> knotPositions = new List<Vector3>(); // List to store knot positions
    public List<Vector3> tangentInPositions = new List<Vector3>(); // List to store TangentIn positions
    public List<Vector3> tangentOutPositions = new List<Vector3>(); // List to store TangentOut positions

    public List<GameObject> knotObjects = new List<GameObject>(); // List to store GameObjects for knots
    public List<GameObject> tangentInObjects = new List<GameObject>(); // List to store GameObjects for TangentIn
    public List<GameObject> tangentOutObjects = new List<GameObject>(); // List to store GameObjects for TangentOut

    private void Update()
    {
        if (splineContainer == null) return;
        ReadSplinePositions();
        UpdateGameObjectPositions();
    }

    void ReadSplinePositions()
    {
        Spline spline = splineContainer.Spline;
        knotPositions.Clear();
        tangentInPositions.Clear();
        tangentOutPositions.Clear();

        foreach (var knot in spline.Knots)
        {

            Vector3 position = knot.Position;
            position += splineContainer.transform.position;
            Quaternion rotation = knot.Rotation;

            Vector3 tangentIn = position + (rotation * knot.TangentIn);
            Vector3 tangentOut = position + (rotation * knot.TangentOut);

            knotPositions.Add(position);
            tangentInPositions.Add(tangentIn);
            tangentOutPositions.Add(tangentOut);
        }
    }

    void UpdateGameObjectPositions()
    {
        // Update positions of knot GameObjects
        for (int i = 0; i < knotObjects.Count && i < knotPositions.Count; i++)
        {
            if (knotObjects[i] != null)
            {
                knotObjects[i].transform.position = knotPositions[i];
            }
        }

        // Update positions of TangentIn GameObjects
        for (int i = 0; i < tangentInObjects.Count && i < tangentInPositions.Count; i++)
        {
            if (tangentInObjects[i] != null)
            {
                tangentInObjects[i].transform.position = tangentInPositions[i];
            }
        }

        // Update positions of TangentOut GameObjects
        for (int i = 0; i < tangentOutObjects.Count && i < tangentOutPositions.Count; i++)
        {
            if (tangentOutObjects[i] != null)
            {
                tangentOutObjects[i].transform.position = tangentOutPositions[i];
            }
        }
    }
}