using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGeneration : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] shape;

    public int xSize = 20,zSize = 20;

    // Start is called before the first frame update
    void Start(){
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(ShapeCreator());
    }

    private void Update() {
        MeshUpdate();
    }

    IEnumerator ShapeCreator(){
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int index = 0;
        for (int z = 0; z <= zSize; z++){
            for (int x = 0; x <= xSize; x++){
                float y = Mathf.PerlinNoise(x * .3f,z * .3f) * 2f;
                vertices[index] = new Vector3(x,y,z);
                index++;
            }
        }

        shape = new int[xSize * zSize * 6];
        int ver = 0;
        int t = 0;

        for (int z = 0; z < zSize; z++){
            for (int x = 0; x < xSize; x++){
                shape[t + 0] = ver + 0;
                shape[t + 1] = ver + xSize + 1;
                shape[t + 2] = ver + 1;
                shape[t + 3] = ver + 1;
                shape[t + 4] = ver + xSize + 1;
                shape[t + 5] = ver + xSize + 2;

                ver ++;
                t += 6;

                yield return new WaitForSeconds(0.01f);
            }
            ver++;
        }
    }

    private void MeshUpdate(){
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = shape;
        mesh.RecalculateNormals();
    }
}
