using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RandomColor : MonoBehaviour
{

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 0.5f, 1.0f);
        renderer.material.SetColor("_Color", color);
    }
}
