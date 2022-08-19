using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveRenderer : MonoBehaviour
{
    [SerializeField]
    private float frequence = 5;

    private SpriteRenderer spriteRender;
    private float alphaFactor;

    [SerializeField]
    Color glowColor;

    public Color GlowColor { get => glowColor; set => glowColor = value; }
    public SpriteRenderer SpriteRender { get => spriteRender; set => spriteRender = value; }

    private void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        alphaFactor = (Mathf.Sin(Time.time * frequence)+1) / 2;

        glowColor.a = alphaFactor;
        spriteRender.color = glowColor;
    }
}
