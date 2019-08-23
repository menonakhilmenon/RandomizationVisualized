using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GenerateRandomTexture : MonoBehaviour
{
    public Image image;
    public Texture2D texture;
    public int xWidth;
    public int yWidth;
    public int iterations = 100;

    float2 p1;
    float2 p2;
    float2 p3;
    float2 currentLocation;
    Color[] colors;

    //Barsley's Fern Constants

    float2x2 f1;
    float2 f1_;

    float2x2 f2;
    float2 f2_;

    float2x2 f3;
    float2 f3_;

    float2x2 f4;
    float2 f4_;


    void Start()
    {
        InitializeArray();
        ApplyColor();
    }

    void InitializeArray()
    {
        texture = new Texture2D(xWidth, yWidth);
        p1 = new float2(0f, 0f);
        p2 = new float2(0f, xWidth - 1);
        p3 = new float2(yWidth - 1, xWidth / 2f);
        colors = new Color[xWidth * yWidth];
        Color transparent = new Color(0, 0, 0, 0);
        currentLocation = new float2(0f,0f);
        for (int i = 0; i < xWidth * yWidth; i++)
        {
            colors[i] = transparent;
        }

        f1 = new float2x2(0f, 0f, 0f, 0.16f);
        f2 = new float2x2(0.85f, 0.04f, -0.04f, 0.85f);
        f3 = new float2x2(0.20f, -0.26f, 0.23f, 0.22f);
        f4 = new float2x2(-0.15f, 0.28f, 0.26f, 0.24f);
        f1_ = new float2(0f, 0f);
        f2_ = new float2(0f, 1.6f);
        f3_ = new float2(0f, 1.6f);
        f4_ = new float2(0f, 0.44f);
    }
    public void AddIterations()
    {
        ApplySierpenskiIterations();
        ApplyColor();
    }

    public void AddBarnsleys()
    {
        ApplyBarnsley();
        ApplyColor();
    }
    void ApplyBarnsley()
    {
        float2 nextLocation;
        float2 scale= new float2(xWidth/6f,yWidth/10.1f);
        float2 offSet = new float2(xWidth / 2f, 0f);
        int2 paintCoord;
        nextLocation = currentLocation;
        for (int i = 0; i < iterations; i++)
        {
            int rand = Random.Range(0, 100);
            if (rand < 1)
            {
                nextLocation.x = 0f;
                nextLocation.y = 0.16f * currentLocation.y;
            }
            else if (rand < 86)
            {
                nextLocation.x = 0.85f * currentLocation.x + 0.04f * currentLocation.y;
                nextLocation.y = -0.04f * currentLocation.x + 0.85f * currentLocation.y+1.6f;
            }
            else if(rand < 93)
            {
                nextLocation.x = 0.2f * currentLocation.x + -0.26f * currentLocation.y;
                nextLocation.y = 0.23f * currentLocation.x + 0.22f * currentLocation.y + 1.6f;
            }
            else
            {
                nextLocation.x = -0.15f * currentLocation.x + 0.28f * currentLocation.y;
                nextLocation.y = 0.26f * currentLocation.x + 0.24f * currentLocation.y + 0.44f;
            }

            paintCoord.x = (int)((nextLocation.x * scale.x) + offSet.x);
            paintCoord.y = (int)((nextLocation.y * scale.y) + offSet.y);
            currentLocation = nextLocation;

            if (paintCoord.x >= xWidth || paintCoord.y >= yWidth || paintCoord.x<0 || paintCoord.y <0)
                continue;
            colors[paintCoord.y * xWidth + paintCoord.x] = Color.green;

   
        }
    }
    void ApplySierpenskiIterations()
    {
        float2 nextLocation;
        float2 nT;

        for (int i = 0; i < iterations; i++)
        {
            int rand = Random.Range(0, 3);
            if (rand == 0)
                nT = p1;
            else if (rand == 1)
                nT = p2;
            else
                nT = p3;
            nextLocation = (currentLocation + nT) / 2f;
            colors[(((int)nextLocation.x) * yWidth + (int)nextLocation.y)] = Color.red;
            currentLocation = nextLocation;
        }
    }
    void ApplyColor()
    {
        texture.SetPixels(colors);
        texture.Apply();
        image.material.mainTexture=texture;
    }

    float2 MMultiply(float2x2 m,float2 v)
    {
        return new float2(m.c0.x*v.x + m.c1.x * v.y , m.c0.y * v.x + m.c1.y * v.y);
    }
}
