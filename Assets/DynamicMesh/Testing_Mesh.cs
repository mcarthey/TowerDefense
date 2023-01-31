using UnityEngine;

public class Testing_Mesh : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var mesh = new Mesh();

        var vertices = new Vector3[4]; // vertices and uv are the same
        var uv = new Vector2[4];
        var triangles = new int[6]; // number of vertices required for the triangle

        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, 100);
        vertices[2] = new Vector3(100, 100);
        vertices[3] = new Vector3(100, 0);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        // always set indices for vertex in a clockwise fashion
        // first polygon
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        // second polygon
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
