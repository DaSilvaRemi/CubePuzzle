using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class InvertObjectMiniGames : MonoBehaviour, IEventHandler
{
    [Header("Mini Games properties")]
    [Tooltip("Game Object To Desactivate")]
    [SerializeField] private GameObject m_GameObjectToDesactivate;
    [Tooltip("Spawns positions")]
    [SerializeField] private Transform[] m_SpawnsPosition;
    [Tooltip("GO will be spawn")]
    [SerializeField] private GameObject[] m_GameObjetsToSpawn;

    private List<GameObject> m_GameObjetsSpawned;
    private List<GameObject> m_GameObjetsSelectedToInvert;
    private List<int> m_IndexSpawnPosition;
    private List<int> m_IndexGoodArrangementObjects;
    private bool m_MiniGameIsFinished;

    #region Events Listeners
    private void OnSelectGameObjectToInvertEvent(SelectGameObjectToInvertEvent e)
    {
        this.SelectGameObjectToInvert(e.eGameObjectToInvert);
    }
    #endregion

    #region Events Handlers
    private void SelectGameObjectToInvert(GameObject gameObject)
    {
        if (gameObject != null && !this.m_GameObjetsSelectedToInvert.Contains(gameObject))
        {
            this.m_GameObjetsSelectedToInvert.Add(gameObject);
        }
        this.VerifyGameObjetsSelectedToInvert();
    }
    #endregion

    #region InvertObjectMiniGames Utils Methods
    /// <summary>
    /// Spawn randomly game objects in spawns positions
    /// </summary>
    private void SpawnRandomlyObjectInSpawnPosition()
    {
        for (int i = 0; i < this.m_SpawnsPosition.Length; i++)
        {
            int indexSpawnPosition;
            int indexGameObjectToSpawn;

            do
            {
                indexSpawnPosition = UnityEngine.Random.Range(0, this.m_SpawnsPosition.Length);
            } while (this.m_IndexSpawnPosition.Contains(indexSpawnPosition));

            do
            {
                indexGameObjectToSpawn = UnityEngine.Random.Range(0, this.m_GameObjetsToSpawn.Length);
            } while (this.m_GameObjetsSpawned.Contains(this.m_GameObjetsToSpawn[indexGameObjectToSpawn])) ;

            GameObject gameObjectToSpawn = Instantiate(this.m_GameObjetsSpawned[indexGameObjectToSpawn]);
            gameObjectToSpawn.transform.SetPositionAndRotation(this.m_SpawnsPosition[indexSpawnPosition].position, this.m_SpawnsPosition[indexSpawnPosition].rotation);

            this.m_GameObjetsSpawned.Add(gameObjectToSpawn);
            this.m_IndexSpawnPosition.Add(i);
        }
    }

    /// <summary>
    /// Generate the combinaison to find
    /// </summary>
    private void GenerateCombinaison()
    {
        for (int i = 0; i < this.m_GameObjetsSpawned.Count; i++)
        {
            int randomIndexValue;
            do
            {
                randomIndexValue = UnityEngine.Random.Range(0, this.m_GameObjetsSpawned.Count);
            } while (this.m_IndexGoodArrangementObjects.Contains(randomIndexValue));
            
            this.m_IndexGoodArrangementObjects.Add(randomIndexValue);
        }
    }

    /// <summary>
    /// Invert Twho Game object in Unity world and in tabs
    /// </summary>
    /// <param name="gameObject1">The first game object to invert</param>
    /// <param name="gameObject2">The second game object to invert</param>
    protected void InvertTwoGameObjects(GameObject gameObject1, GameObject gameObject2)
    {
        this.InvertTwoGameObjectPosition(gameObject1, gameObject2);
        this.InvertTwoGameObjectsIndex(this.m_GameObjetsSpawned.IndexOf(gameObject1), this.m_GameObjetsSpawned.IndexOf(gameObject2));
    }

    /// <summary>
    /// Invert Two Game Object position in Unity World and in the list
    /// </summary>
    /// <example>
    ///     InvertTwoGameObjectPosition(Objet1, Objet2) => 
    ///         int indexGameObject1 = 0;
    ///         int indexGameObject2 = 1;
    ///          Object1 =  {1, 0, 0}
    ///          Object2 =  {2, 0, 0}
    ///          m_GameObjetsSpawned[0] = Object2;
    ///          m_GameObjetsSpawned[1] = Object1;
    ///          m_GameObjetsSpawned = {Objet2, Objet1, Objet3} => {{2, 0, 0}, {1, 0, 0}, Objet3};
    ///          m_IndexSpawnPosition => {0, 1, 2};
    ///          m_IndexGoodArrangementObjects => {1, 2, 0};
    /// </example>
    /// <param name="gameObject1">The first game object to invert</param>
    /// <param name="gameObject2">The second game object to invert</param>
    private void InvertTwoGameObjectPosition(GameObject gameObject1, GameObject gameObject2)
    {
        int indexGameObject1 = this.m_GameObjetsSpawned.IndexOf(gameObject1);
        int indexGameObject2 = this.m_GameObjetsSpawned.IndexOf(gameObject2);
        GameObject tmpGameObject = gameObject1;
        gameObject1.transform.SetPositionAndRotation(gameObject2.transform.position, gameObject2.transform.rotation);
        gameObject2.transform.SetPositionAndRotation(tmpGameObject.transform.position, tmpGameObject.transform.rotation);
        this.m_GameObjetsSpawned[indexGameObject1] = gameObject2;
        this.m_GameObjetsSpawned[indexGameObject2] = gameObject1;
    }

    /// <summary>
    /// Invert two game object index only in tabs
    /// </summary>
    /// <example>
    /// InvertTwoGameObjectsIndex(indexObject1 => 0, indexObject2 => 1) =>
    ///     tmpValueIndexGameObject1 = m_IndexSpawnPosition[indexGameObject1] => 0;
    ///     m_IndexSpawnPosition[indexGameObject1] = m_IndexSpawnPosition[indexGameObject2] => 1;
    ///     m_IndexSpawnPosition => {1, 1, 2};
    ///     m_IndexSpawnPosition[indexGameObject1] = tmpValueIndexGameObject1 => 0;
    ///     m_IndexSpawnPosition => {1, 0, 2};
    /// </example>
    /// <param name="indexGameObject1">The first index to invert</param>
    /// <param name="indexGameObject2">The second index to invert</param>
    private void InvertTwoGameObjectsIndex(int indexGameObject1, int indexGameObject2)
    {
        int tmpValueIndexGameObject1 = this.m_IndexSpawnPosition[indexGameObject1];
        this.m_IndexSpawnPosition[indexGameObject1] = this.m_IndexSpawnPosition[indexGameObject2];
        this.m_IndexSpawnPosition[indexGameObject2] = tmpValueIndexGameObject1;
    }
    #endregion

    #region InvertObjectMiniGames Controls Methods
    private void VerifyGameObjetsSelectedToInvert()
    {
        if (this.m_GameObjetsSelectedToInvert.Count == 2)
        {
            this.InvertTwoGameObjects(this.m_GameObjetsSelectedToInvert[0], this.m_GameObjetsSelectedToInvert[1]);
            this.m_GameObjetsSelectedToInvert.Clear();
        }
    }

    private void VerifyGameObjectsHasWellPlaced()
    {
        if (!this.m_MiniGameIsFinished && this.GameObjectsHasWellPlaced())
        {
            this.m_GameObjectToDesactivate.SetActive(false);
            this.m_MiniGameIsFinished = true;
        }
    }

    public bool GameObjectsHasWellPlaced()
    {
        for (int i = 0; i < this.m_IndexSpawnPosition.Count; i++)
        {
            if (this.m_IndexSpawnPosition[i] != this.m_IndexGoodArrangementObjects[i])
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Events Subscription
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<SelectGameObjectToInvertEvent>(OnSelectGameObjectToInvertEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<SelectGameObjectToInvertEvent>(OnSelectGameObjectToInvertEvent);
    }
    #endregion

    #region MonoBehaviour methods
    private void Awake()
    {
        this.m_GameObjetsSpawned = new List<GameObject>();
        this.m_GameObjetsSelectedToInvert = new List<GameObject>();
        this.m_IndexSpawnPosition = new List<int>();
        this.m_IndexGoodArrangementObjects = new List<int>();
        this.SubscribeEvents();
    }

    private void Start()
    {
        this.SpawnRandomlyObjectInSpawnPosition();
        this.GenerateCombinaison();
    }

    private void FixedUpdate()
    {
        this.VerifyGameObjectsHasWellPlaced();
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvents();
    }
    #endregion
}
