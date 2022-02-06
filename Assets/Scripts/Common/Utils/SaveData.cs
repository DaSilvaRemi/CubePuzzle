using System;
using System.IO;
using UnityEngine;

/**
 * <summary>Serializable class for save the game </summary>
 */
public class SaveData: ITime, IGameState 
{
    private SerializableGame m_SerializableGame;

    public float Time { get => m_SerializableGame.time; set => m_SerializableGame.time = value; }

    public float BestTime { get => m_SerializableGame.bestTime; set => m_SerializableGame.bestTime = value; }

    public Tools.GameScene Level { get => m_SerializableGame.level; set => m_SerializableGame.level = value; }

    public Tools.GameState GameState { get => m_SerializableGame.gameState; set => m_SerializableGame.gameState = value; }

    /**
     * <summary>The default constructor</summary> 
     */
    public SaveData() : this(0f, Tools.GameScene.FIRSTLEVELSCENE)
    {
    }

    /**
     * <summary>The constructor</summary>
     * 
     * <param name="time">The time</param>
     * <param name="level">The level</param>
     */
    public SaveData(float time, Tools.GameScene level) : this(time, level, time, Tools.GameState.PLAY)
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
    public SaveData(float time, Tools.GameScene level, float bestTime, Tools.GameState gameState) : this(new SerializableGame(time, level, bestTime, gameState))
    {
    }

    public SaveData(SerializableGame serializableGame)
    {
        m_SerializableGame = new SerializableGame(serializableGame);
    }

    /**
     * <summary>Save the game</summary>
     * <param name="save">The save</param>
     */
    public static void Save(SaveData save)
    {

        SaveData data = null;
        try
        {
            data = Load();
        }
        catch (FileNotFoundException)
        {
            data = new SaveData();
        }
        finally
        {
            if (data.BestTime == 0 || save.BestTime <= data.BestTime)
            {
                data.BestTime = save.BestTime;
            }

            data.Level = save.Level;
            data.Time = save.Time;
            data.GameState = save.GameState;

            SaveFile(data);
        }
    }

    /**
     * <summary>Save data on JSON file</summary> 
     * 
     * <param name="saveGame">The save game</param>
     */
    public static void SaveFile(SaveData saveGame)
    {
        SaveFile("/savefile.json", JsonUtility.ToJson(saveGame.m_SerializableGame));
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
        Debug.Log("Save");
        Debug.Log(jsonData);
        File.WriteAllText(path + fileName, jsonData);
    }

    /**
     * <summary>Load the save</summary>
     */
    public static SaveData Load()
    {
        return Load(Application.persistentDataPath + "/savefile.json");
    }

    /**
     * <summary>Load the save game</summary>
     */
    public static SaveData Load(string path)
    {
        SaveData loadedSave = new SaveData();

        try
        {
            loadedSave.m_SerializableGame = JsonUtility.FromJson<SerializableGame>(LoadFile(path));
        }
        catch (FileNotFoundException ex)
        {
            Debug.Log(ex.Message);
        }

        Debug.Log("Load");
        Debug.Log(loadedSave.m_SerializableGame.ToString());

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
