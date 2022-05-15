using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SerializableGame
{
    public float time;

    public float bestTime;

    public int score;

    public int bestScore;

    public Tools.GameScene level;

    public Tools.GameState gameState;

    /// <summary>
    /// SerializableGame copy constructor
    /// </summary>
    /// <param name="serializableGame">The serializableGame to copy</param>
    public SerializableGame(SerializableGame serializableGame): this(serializableGame.time, serializableGame.bestTime, serializableGame.score, serializableGame.bestScore, serializableGame.level,  serializableGame.gameState)
    {
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="time">The time</param>
    /// <param name="level">The level</param>
    /// <param name="bestTime">The best time</param>
    /// <param name="gameState">The game state</param>
    public SerializableGame(float time, float bestTime, int score, int bestScore, Tools.GameScene level, Tools.GameState gameState)
    {
        this.time = time;
        this.bestTime = bestTime;
        this.score = score;
        this.bestScore = bestScore;
        this.level = level;
        this.gameState = gameState;
    }

    /// <summary>
    /// Convert to string the current object
    /// </summary>
    /// <returns>The string form the object</returns>
    public override string ToString()
    {
        return $"Game Time : {this.time}, Best Time : {this.bestTime}, Score : {this.score}, Best Score : {this.bestScore}, Level : {this.level}, GameState: {this.gameState}";
    }
}
