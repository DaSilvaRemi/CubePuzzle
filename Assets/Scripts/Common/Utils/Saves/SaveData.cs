using System;
using System.IO;
using UnityEngine;

/**
 * <summary>Serializable class for save the game </summary>
 */
public class SaveData: IGameState 
{
    private SerializableGame m_SerializableGame;

    public float Time { get => this.m_SerializableGame.time; set => this.m_SerializableGame.time = value; }

    public float BestTime { get => this.m_SerializableGame.bestTime; set => this.m_SerializableGame.bestTime = value; }

    public Tools.GameScene Level { get => this.m_SerializableGame.level; set => this.m_SerializableGame.level = value; }

    public Tools.GameState GameState { get => this.m_SerializableGame.gameState; set => this.m_SerializableGame.gameState = value; }

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
        this.m_SerializableGame = new SerializableGame(serializableGame);
    }

    /**
     * <summary>Save the game</summary>
     * <param name="save">The save</param>
     */
    public static void Save(SaveData save)
    {

        SaveData data = LoadPlayerRefs();
        if (data.BestTime == 0 || save.BestTime <= data.BestTime)
        {
            data.BestTime = save.BestTime;
        }

        data.Level = save.Level;
        data.Time = save.Time;
        data.GameState = save.GameState;

        SaveData.SaveOnPlayerRef(data);
    }

    /**
     * <summary>Save data on Players refs</summary> 
     * 
     * <param name="saveGame">The save game</param>
     */
    public static void SaveOnPlayerRef(SaveData saveGame)
    {
        PlayerPrefs.SetInt("level", (int) saveGame.Level);
        PlayerPrefs.SetFloat("time", saveGame.Time);
        PlayerPrefs.SetFloat("bestTime", saveGame.BestTime);
        PlayerPrefs.SetInt("gameState", (int) saveGame.GameState);
        Debug.Log("Save");
        PlayerPrefs.Save();
        Debug.Log(JsonUtility.ToJson(saveGame.m_SerializableGame));
    }

    /**
     * <summary>Load the save</summary>
     */
    public static SaveData LoadPlayerRefs()
    {
        Tools.GameScene loadedGameScene = (Tools.GameScene) PlayerPrefs.GetInt("level");
        Tools.GameState loadedGameState = (Tools.GameState) PlayerPrefs.GetInt("gameState");
        float loadedGameTime = PlayerPrefs.GetFloat("time");
        float loadedGameBestTime = PlayerPrefs.GetFloat("bestTime");
        
        SaveData loadedSave = new SaveData(new SerializableGame(loadedGameTime, loadedGameScene, loadedGameBestTime, loadedGameState));
        Debug.Log("Load");
        Debug.Log(JsonUtility.ToJson(loadedSave.m_SerializableGame));

        return loadedSave;
    }
}
