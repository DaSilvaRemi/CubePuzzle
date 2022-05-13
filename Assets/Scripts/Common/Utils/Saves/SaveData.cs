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

    public int Score { get => this.m_SerializableGame.score; set => this.m_SerializableGame.score = value; }

    public int BestScore { get => this.m_SerializableGame.bestScore; set => this.m_SerializableGame.bestScore = value; }

    public Tools.GameScene Level { get => this.m_SerializableGame.level; set => this.m_SerializableGame.level = value; }

    public Tools.GameState GameState { get => this.m_SerializableGame.gameState; set => this.m_SerializableGame.gameState = value; }

    /**
     * <summary>The default constructor</summary> 
     */
    public SaveData() : this(0f, 0, Tools.GameScene.FIRSTLEVELSCENE)
    {
    }

    /// <summary>
    /// Save data constructor
    /// </summary>
    /// <param name="time">The time</param>
    /// <param name="score">The score</param>
    /// <param name="level">The level</param>
    public SaveData(float time, int score, Tools.GameScene level) : this(time, time, score, score, level, Tools.GameState.PLAY)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">The time</param>
    /// <param name="bestTime">The best time</param>
    /// <param name="score">The score</param>
    /// <param name="bestScore">The best score</param>
    /// <param name="level">The level</param>
    /// <param name="gameState">The game State</param>
    public SaveData(float time, float bestTime, int score, int bestScore, Tools.GameScene level, Tools.GameState gameState) : this(new SerializableGame(time, bestTime, score, bestScore, level, gameState))
    {
    }

    /// <summary>
    /// Save data constructor with <see cref="SerializableGame"/>
    /// </summary>
    /// <param name="serializableGame">The serializableGame</param>
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
        if (save.BestTime > 0 && (data.BestTime == 0 || save.BestTime <= data.BestTime))
        {
            data.BestTime = save.BestTime;
        }

        if (save.BestScore >= data.BestScore)
        {
            data.BestScore = save.BestScore;
        }

        data.Level = save.Level;
        data.Time = save.Time;
        data.Score = save.Score;
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
        PlayerPrefs.SetFloat("time", saveGame.Time);
        PlayerPrefs.SetFloat("bestTime", saveGame.BestTime);
        PlayerPrefs.SetInt("score", saveGame.Score);
        PlayerPrefs.SetInt("bestScore", saveGame.BestScore);
        PlayerPrefs.SetInt("level", (int) saveGame.Level);
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
        float loadedGameTime = PlayerPrefs.GetFloat("time");
        float loadedGameBestTime = PlayerPrefs.GetFloat("bestTime");
        int loadedGameScore = PlayerPrefs.GetInt("score");
        int loadedGameBestScore = PlayerPrefs.GetInt("bestScore");
        Tools.GameScene loadedGameScene = (Tools.GameScene) PlayerPrefs.GetInt("level");
        Tools.GameState loadedGameState = (Tools.GameState) PlayerPrefs.GetInt("gameState");
        
        SaveData loadedSave = new SaveData(new SerializableGame(loadedGameTime, loadedGameBestTime, loadedGameScore, loadedGameBestScore, loadedGameScene, loadedGameState));
        Debug.Log("Load");
        Debug.Log(JsonUtility.ToJson(loadedSave.m_SerializableGame));

        return loadedSave;
    }
}
