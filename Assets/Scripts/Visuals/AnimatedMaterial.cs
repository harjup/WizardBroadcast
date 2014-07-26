using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

//Apparently this is poor for performance WHOOOOOPS!!!
//Also this is pretty terrible oh welllll
public class AnimatedMaterial : MonoBehaviour
{
    public enum AnimTexture
    {
        Undefined, 
        Teleporter
    }

    public AnimTexture textureSet;


    private Texture2D textureAtlas;
    private Texture2D[] textures;
    private int textureWidth = 1024;
    private int length = 5;
    private Material _material;
    
    // Use this for initialization
    void Start()
    {
        switch (textureSet)
        {
            case AnimTexture.Undefined:
                break;
            case AnimTexture.Teleporter:
                textures = new Texture2D[4];
                for (int i = 0; i < 4; i++)
                {
                    textures[i] = Resources.Load(String.Format("Teleporter/Teleporter Texture Frame {0}", i+1)) as Texture2D;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        _material = renderer.material;
        StartCoroutine(AnimateTexture());
    }

    private int index = 0;
    private int direction = 1;
    IEnumerator AnimateTexture()
    {
        while (true)
        {
            _material.mainTexture = textures[index];
            index += direction;
            if (index >= textures.Length - 1)
            {
                direction = -1;
            }
            if (index <= 0)
            {
                direction = 1;
            }
            yield return new WaitForSeconds(.1f);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
