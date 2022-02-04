using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tools
{
    public enum GameState
    {
        MENU,
        PLAY,
        PAUSE,
        WIN,
        GAMEOVER,
        ENDLVL
    }

    public enum GameScene
    {
        MENUSCENE = 0, 
        FIRSTLEVELSCENE, 
        SECONDLVLSCENE, 
        THIRDLEVELSCENE, 
        FOURTHLEVELSCENE, 
        HELPSCENE, 
        CREDITSCENE
    }

    public static void Log(Component component, string msg)
    {
        Debug.Log(Time.frameCount + " " + component.GetType().Name + " " + msg);
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