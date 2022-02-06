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

    public Tools.GameScene level;

    public Tools.GameState gameState;

    public SerializableGame(SerializableGame serializableGame): this(serializableGame.time, serializableGame.level, serializableGame.bestTime, serializableGame.gameState)
    {
    }

    public SerializableGame(float time, Tools.GameScene level, float bestTime, Tools.GameState gameState)
    {
        this.time = time;
        this.level = level;
        this.bestTime = bestTime;
        this.gameState = gameState;
    }

    public override string ToString()
    {
        return $"Game Time : {this.time}, Best Time : {this.bestTime}, Level : {this.level}, GameState: {this.gameState}";
    }
}
