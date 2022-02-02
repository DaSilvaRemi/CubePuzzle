using System;
using System.IO;
using UnityEngine;

[Serializable]
/**
 * <summary>Serializable class for save the game </summary>
 */
public class SaveGame : IGameState, ITime { 

    public float Time { get; set; }

    public float BestTime { get; set; }

    public uint Level { get; set; }

    public Tools.GameState GameState { get; set; }

    /**
     * <summary>The default constructor</summary> 
     */
    public SaveGame() : this(0f, 0)
    {
    }

    /**
     * <summary>The constructor</summary>
     * 
     * <param name="time">The time</param>
     * <param name="level">The level</param>
     */
    public SaveGame(float time, uint level) : this(time, level, time, Tools.GameState.PLAY)
    {
    }

    /**
     * <summary>The constructor</summary>
     * 
     * <param name="time">The time</param>
     * <param name="level">The level</param>
     * <param name="bestTime">The best time</param>
     * <param name="gameState">The game state</param>
     */
    public SaveGame(float time, uint level, float bestTime, Tools.GameState gameState)
    {
        Time = time;
        Level = level;
        BestTime = bestTime;
        GameState = gameState;
    }

    /**
     * <summary>Save the game</summary>
     * <param name="game">The game to save</param>
     */
    public static void Save(SaveGame game)
    {

        SaveGame data = null;
        try
        {
            data = Load();
        }
        catch (FileNotFoundException)
        {
            data = new SaveGame();
        }
        finally
        {
            if (game.BestTime > data.BestTime)
            {
                data.Time = game.Time;
            }

            data.Level = game.Level;
            data.Time = game.Time;
            data.GameState = game.GameState;

            SaveFile(data);
        }
    }

    /**
     * <summary>Save data on JSON file</summary> 
     * 
     * <param name="saveGame">The save game</param>
     */
    public static void SaveFile(SaveGame saveGame)
    {
        SaveFile("/savefile.json", JsonUtility.ToJson(saveGame));
    }

    /**
     * <summary>Save data on JSON file</summary>
     * <param name="fileName"></param>
     * <param name="jsonData"></param>
     */
    public static void SaveFile(string fileName, string jsonData)
    {
        SaveFile(Application.persistentDataPath, fileName, jsonData);
    }

    /**
     * <summary>Save data on JSON file</summary>
     * <param name="path"></param>
     * <param name="fileName"></param>
     * <param name="jsonData"></param>
     */
    public static void SaveFile(string path, string fileName, string jsonData)
    {
        File.WriteAllText(path + fileName, jsonData);
    }

    /**
     * <summary>Load the save</summary>
     */
    public static SaveGame Load()
    {
        return Load(Application.persistentDataPath + "/savefile.json");
    }

    /**
     * <summary>Load the save game</summary>
     */
    public static SaveGame Load(string path)
    {
        SaveGame loadedSave = new SaveGame();

        try
        {
            loadedSave = JsonUtility.FromJson<SaveGame>(LoadFile(path));
        }
        catch (FileNotFoundException ex)
        {
            Debug.Log(ex.Message);
        }

        return loadedSave;
    }

    /**
     * <summary>Load a file</summary>
     */
    public static string LoadFile(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("File does not exist: " + path);

        return File.ReadAllText(path);
    }
}
