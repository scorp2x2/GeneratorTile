using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnityMeshCreator : MonoBehaviour
{
    public Tile tile;

    public void Draw(Tile tile, float angle)
    {
        this.tile = tile;

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        Vector3[] vertices = tile.points.Select(a => new Vector3(a.x, a.y, 0)).ToArray();

        mesh.vertices = vertices;

        List<int> tris = new List<int>();

        for (int i = 1; i < vertices.Length - 1; i++)
        {
            tris.Add(0);
            tris.Add(i);
            tris.Add(i + 1);
        }

        mesh.triangles = tris.ToArray();

        List<Vector3> normals = new List<Vector3>();
        foreach (var item in vertices)
            normals.Add(-Vector3.forward);

        mesh.normals = normals.ToArray();

        List<Vector2> uv = new List<Vector2>();
        foreach (var item in vertices)
        {
            var p = tile.RotatePoint(item, -angle, new Vector2());
            var sp = tile.RotatePoint(tile.startPoint, -angle, new Vector2());
            uv.Add(new Vector2(Mathf.Abs((p.x - sp.x) / tile.width), Mathf.Abs((p.y - sp.y) / tile.height)));
        }

        mesh.uv = uv.ToArray();

        meshFilter.mesh = mesh;
    }
}
