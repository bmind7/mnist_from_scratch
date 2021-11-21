using System.Linq;
using UnityEngine;

public class MnistImage
{
    //-----------------------------------------------------
    public float[] Label;
    public float[] Pixels;
    public int Height;
    public int Width;
    //-----------------------------------------------------
    public Color[] GetColorsForTexture()
    {
        var output = new Color[ Height * Width ];
        for (int h = 0; h < Height; h++) {
            for (int w = 0; w < Width; w++) {
                float intensity = this.Pixels[ h * Width + w ];
                // We need to flip rows horizontally, because Texture2D pixels start from bottom left corner
                output[ (Height - 1 - h) * Width + w ] = new Color( intensity, intensity, intensity );
            }
        }
        return output;
    }
    //-----------------------------------------------------
}