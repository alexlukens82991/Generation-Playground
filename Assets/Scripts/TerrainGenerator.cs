using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject m_GridPixelPrefab;
    [SerializeField] private Texture2D m_ElevationMap;
    [SerializeField] private Transform m_PixelsParent;
    public Color[] m_Pixels;
    public int Width;
    public int Height;
    public float MaxHeight;
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;

    private void Start()
    {
        mesh = new();

        GetComponent<MeshFilter>().mesh = mesh;

        Width = m_ElevationMap.width;
        Height = m_ElevationMap.height;
        StartCoroutine(GenerateMesh());
        //GenerateMesh();
    }

    private IEnumerator GenerateMesh()
    {
        m_Pixels = m_ElevationMap.GetPixels();
        List<Color> newPixels = new();
        int counter = 0;
        for (int i = 0; i < m_Pixels.Length; i++)
        {
            for (int ii = 0; ii < Width; ii++)
            {
                if (counter < m_Pixels.Length)
                {
                    newPixels.Add(m_Pixels[counter]);
                    counter++;
                }
                else
                    break;
            }
            newPixels.Add(Color.white);
        }

        m_Pixels = newPixels.ToArray();

        counter = 0;

        vertices = new Vector3[(Width + 1) * (Height + 1)];

        for (int z = 0; z <= Height; z++)
        {
            for (int x = 0; x <= Width; x++)
            {
                float height;
                if (counter < m_Pixels.Length)
                {
                    height = 1 - (m_Pixels[counter].r) * MaxHeight;
                }
                else
                {
                    height = 0;
                }
                vertices[counter] = new Vector3(x, height, z);

                counter++;
            }
        }

        triangles = new int[Width * Height * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + Width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + Width + 1;
                triangles[tris + 5] = vert + Width + 2;

                vert++;
                tris += 6;

                if(vert % 100 == 0)
                    yield return new WaitForSeconds(0.01f);

                UpdateMesh();
            }

            vert++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
