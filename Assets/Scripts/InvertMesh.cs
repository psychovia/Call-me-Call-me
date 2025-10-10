using UnityEngine;
using System.Linq; // Required for .Reverse() and .Select()

public class InvertMesh : MonoBehaviour
{
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter found on this GameObject.");
            return;
        }

        Mesh mesh = meshFilter.mesh;

        // Invert normals
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        mesh.normals = normals;

        // Reverse triangle winding order (important for correct culling and collision detection)
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // Swap the first and third vertex of each triangle
            int temp = triangles[i + 0];
            triangles[i + 0] = triangles[i + 2];
            triangles[i + 2] = temp;
        }
        mesh.triangles = triangles;

        // Ensure the MeshCollider is updated
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh; // Reassign the modified mesh
            meshCollider.convex = false; // Important for inner collision detection
        }
    }
}