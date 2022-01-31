using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
/**
 * <summary>Serializable class for save the game </summary>
 */
public class SaveGame : IScore, IGameState { 

    public int Score { get; set; }

    public uint Level { get; set; }

    public Tools.GameState GameState { get; set; }

    /**
     * <summary>The default constructor</summary> 
     */
    public SaveGame() : this(0, 0)
    {
    }

    /**
     * <summary>The constructor</summary>
     * 
     * <param name="score"></param>
     * <param name="level"></param>
     */
    public SaveGame(int score, int level)
    {
        Score = score;
        Level = level;
        Save(this);
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
            data = new SaveGame(0, 0);
        }
        finally
        {
            if (game.Score > data.Score)
            {
                data.Score = game.Score;
            }

            if (game.Level != data.Level)
            {
                data.Level = game.Level;
            }

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
        SaveGame loadedSave = new SaveGame(0, 0);

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
