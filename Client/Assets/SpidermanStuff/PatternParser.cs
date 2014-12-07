using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternParser {

    public static Pattern CreatePattern(Texture2D texture)
    {
        Pattern result;
        List<Vector3> tempPositions = new List<Vector3>();
        Vector3 middle = new Vector3(texture.width / 2, texture.height / 2);
        Vector3 position = Vector3.zero;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (texture.GetPixel(x, y) == Color.black)
                {
                    position.x = x;
                    position.y = y;

                    Vector3 radialCoords = position - middle;

                    Vector3 finalPosition = new Vector3(radialCoords.x / texture.width, radialCoords.y / texture.height);

                    tempPositions.Add(finalPosition);
                }
            }
        }
        result = new Pattern(tempPositions);
        return result;
    }
    public static Pattern CreatePattern(string patternName)
    {
        Pattern result;
        Texture2D tempTexture = Resources.Load<Texture2D>("Magic/SpellPatterns/" + patternName);
        return PatternParser.CreatePattern(tempTexture);
    }

}
