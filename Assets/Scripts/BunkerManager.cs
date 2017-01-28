using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Machinethoughts
{
    public class BunkerManager : MonoBehaviour
    {
        public GameObject bunker;
        public Material bunkerMaterial;
        Texture2D bunkerTexture;
        Texture2D splatTexture;
        int numberOfBunkers = 4;
        float spacing = 5.0f;
        float height = 0.0f;

        float leftEdge;
        Material[] materials;
        Texture2D[] textures;
        GameObject[] bunkers = new GameObject[0];
        Color[] splatTexArray;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Setup(float playfieldWidth)
        {
            //TODO:these calculations are made up and wrong
            var space = (numberOfBunkers - 1) * spacing;
            leftEdge = -(playfieldWidth / 2) + (playfieldWidth - space) / 2;

            bunkers = new GameObject[numberOfBunkers];
            for (var i = 0; i < numberOfBunkers; i++)
            {
                bunkers[i] = Instantiate(bunker);
                bunkers[i].transform.position = new Vector3(leftEdge + spacing * i, height, 3f);
            }
            //splatTexArray = splatTexture.GetPixels();
        }

        public void SetBunkers()
        {
            foreach (var bunker in bunkers)
            {
                if (bunker != null)
                {
                    bunker.SetActive(true);
                }
            }
        }

        public void DisableBunkers()
        {
            for (var i = 0; i < numberOfBunkers; i++)
            {
                bunkers[i].SetActive(false);
            }
        }

        bool Splat(int id, Vector2 pixelUV)
        {
            var tex = textures[id];
            var uvX = (int)pixelUV.x * tex.width;
            var uvY = (int)pixelUV.y * tex.height;

            // If the hit point is transparent, we didn't hit anything
            if (tex.GetPixel(uvX, uvY).a == 0)
            {
                return false;
            }

            // Otherwise copy the splat texture alpha to an area around that point
            var startX = uvX;
            var dir = (Random.value > 0.5) ? -1 : 1;    // Half the time, overlay the hole alpha upside-down & backwards just for variety
            startX += splatTexture.width / 2 * -dir;
            uvY += splatTexture.height / 2 * -dir;
            // Overlay hole texture alpha onto bunker texture around the hit point
            // GetPixel/SetPixel is automatically clamped if the texture is set to clamp, so we don't have to worry about going out of bounds
            for (var y = 0; y < splatTexture.height; y++)
            {
                uvX = startX;
                for (var x = 0; x < splatTexture.width; x++)
                {
                    var thisPix = tex.GetPixel(uvX, uvY);
                    thisPix.a *= splatTexArray[x + y * splatTexture.width].a;
                    tex.SetPixel(uvX, uvY, thisPix);
                    uvX += dir;
                }
                uvY += dir;
            }
            tex.Apply();

            return true;
        }
    }
}