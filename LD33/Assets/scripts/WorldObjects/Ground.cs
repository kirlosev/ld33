using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {
    public MeshRenderer mr;
    public MeshFilter mf;
    public Vector3 size = new Vector3(1f, 1f);
    public Material material;
    Mesh mesh;
    public Vector3 targetPosition;

    public bool underGround = false;

    void Awake() {
        if (mesh == null)
            mesh = new Mesh();
        mf.sharedMesh = mesh;
        mr.sharedMaterial = material;
        var verts = new Vector3[4];
        var tris = new int[] {0, 1, 2, 0, 2, 3};
        var uvs = new Vector2[4];
        verts[0] = targetPosition - size;
        verts[1] = targetPosition + Vector3.up * size.y - Vector3.right * size.x;
        verts[2] = targetPosition + size;
        verts[3] = targetPosition - Vector3.up * size.y + Vector3.right * size.x;

        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, size.y);
        uvs[2] = new Vector2(size.x, size.y);
        uvs[3] = new Vector2(size.x, 0);

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        if (!underGround) {
            var bc = gameObject.GetComponent<BoxCollider2D>();
            bc.size = size * 2f;
        }
    }
}
