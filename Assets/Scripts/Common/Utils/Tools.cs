using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tools
{
    /**
     * <summary>The game state enum</summary> 
     */
    public enum GameState
    {
        PLAY,
        WIN,
        LOOSE,
        LVLFINISH
    }

    /**
     * <summary>Formar a float to int</summary>
     * <param name="floatNumber">A float number</param>
     * <returns>Float number to int</returns>
     */
    public static int FormatFloatToInt(float floatNumber)
    {
        return Mathf.RoundToInt(floatNumber);
    }
}