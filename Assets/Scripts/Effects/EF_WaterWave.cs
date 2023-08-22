using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_WaterWave : MonoBehaviour
{
    public Texture[] textures;
    private Material material;
    private int index = 0;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        InvokeRepeating("ChangePicture", 0, 0.04f);
    }

    private void ChangePicture()
    {
        material.mainTexture = textures[index];
        index = (index + 1) % textures.Length;
    }
}
