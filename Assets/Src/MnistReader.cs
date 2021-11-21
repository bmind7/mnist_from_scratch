using System;
using System.Collections.Generic;
using System.IO;

public static class MnistHelper
{
    //-----------------------------------------------------
    public static List<MnistImage> Load( byte[] binaryImages, byte[] binaryLabels )
    {
        BinaryReader labels = new BinaryReader( new MemoryStream( binaryLabels ) );
        BinaryReader images = new BinaryReader( new MemoryStream( binaryImages ) );

        int magicNumber = ReadBigInt32( images );
        int numberOfImages = ReadBigInt32( images );
        int width = ReadBigInt32( images );
        int height = ReadBigInt32( images );

        int magicLabel = ReadBigInt32( labels );
        int numberOfLabels = ReadBigInt32( labels );

        var output = new List<MnistImage>();
        for (int i = 0; i < numberOfImages; i++) {
            var bytes = images.ReadBytes( width * height );
            var arr = new float[ height * width ];

            for (int pixel = 0; pixel < height * width; pixel++) {
                arr[ pixel ] = bytes[ pixel ] / 255.0f;
            }

            var labelArray = new float[ 10 ];
            labelArray[ labels.ReadByte() ] = 1.0f;
            
            output.Add( new MnistImage()
            {
                Pixels = arr,
                Label = labelArray,
                Height = height,
                Width = width
        } );
        }
        return output;
    }
    //-----------------------------------------------------
    private static int ReadBigInt32( BinaryReader binaryReader )
    {
        var bytes = binaryReader.ReadBytes( sizeof( Int32 ) );
        if (BitConverter.IsLittleEndian) Array.Reverse( bytes );
        return BitConverter.ToInt32( bytes, 0 );
    }
    //-----------------------------------------------------
}

