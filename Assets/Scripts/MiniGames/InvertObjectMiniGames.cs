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
    [Tooltip("Cooldown duration to invert")]
    [SerializeField] private float m_CooldownDuration;
    [Tooltip("Finsih SFX Sound")]
    [SerializeField]  private AudioClip m_FinishedMiniGamesSFXAudioCLip;

    private List<GameObject> m_GameObjetsSpawned;
    private List<GameObject> m_GameObjetsSelectedToInvert;
    private List<int> m_IndexSpawnPosition;
    private List<int> m_IndexGoodArrangementObjects;
    private bool m_MiniGameIsFinished;
    private float m_NextInvertedTime;

    #region Events Listeners
    /// <summary>
    /// OnSelectGameObjectToInvertEvent we selected the object and put in the list
    /// </summary>
    /// <param name="e">The SelectGameObjectToInvertEvent</param>
    private void OnSelectGameObjectToInvertEvent(SelectGameObjectToInvertEvent e)
    {
        this.SelectGameObjectToInvert(e.eGameObjectToInvert);
    }
    #endregion

    #region Events Handlers
    /// <summary>
    /// SelectGameObjectToInvert with other
    /// </summary>
    /// <param name="gameObject">The gamoe object to invert</param>
    private void SelectGameObjectToInvert(GameObject gameObject)
    {
        if (gameObject != null && !this.m_GameObjetsSelectedToInvert.Contains(gameObject) && Time.time > this.m_NextInvertedTime)
        {
            this.m_GameObjetsSelectedToInvert.Add(gameObject);
            this.m_NextInvertedTime = Time.time + this.m_CooldownDuration;
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
        List<GameObject> copyGameObjetsToSpawn = new List<GameObject>(this.m_GameObjetsToSpawn);

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
                indexGameObjectToSpawn = UnityEngine.Random.Range(0, copyGameObjetsToSpawn.Count);
            } while (this.m_GameObjetsSpawned.Contains(copyGameObjetsToSpawn[indexGameObjectToSpawn])) ;

            GameObject gameObjectToSpawn = Instantiate(copyGameObjetsToSpawn[indexGameObjectToSpawn]);
            gameObjectToSpawn.transform.SetPositionAndRotation(this.m_SpawnsPosition[indexSpawnPosition].position, this.m_SpawnsPosition[indexSpawnPosition].rotation);

            this.m_GameObjetsSpawned.Add(gameObjectToSpawn);
            this.m_IndexSpawnPosition.Add(indexSpawnPosition);
            copyGameObjetsToSpawn.RemoveAt(indexGameObjectToSpawn);
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
                randomIndexValue = UnityEngine.Random.Range(0, this.m_IndexSpawnPosition.Count);
            } while (this.m_IndexGoodArrangementObjects.Contains(this.m_IndexSpawnPosition[randomIndexValue]));
            
            this.m_IndexGoodArrangementObjects.Add(this.m_IndexSpawnPosition[randomIndexValue]);
        }
    }

    /// <summary>
    /// Invert Twho Game object in Unity world and in tabs
    /// </summary>
    /// <param name="gameObject1">The first game object to invert</param>
    /// <param name="gameObject2">The second game object to invert</param>
    protected void InvertTwoGameObjects(GameObject gameObject1, GameObject gameObject2)
    {
        this.InvertTwoGameObjectsIndex(this.m_GameObjetsSpawned.IndexOf(gameObject1), this.m_GameObjetsSpawned.IndexOf(gameObject2));
        this.InvertTwoGameObjectPosition(gameObject1, gameObject2);
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
        Vector3 copyPositionGameObject = new Vector3(gameObject1.transform.position.x, gameObject1.transform.position.y, gameObject1.transform.position.z);
        Quaternion copyRotationGameObject = new Quaternion(gameObject1.transform.rotation.x, gameObject1.transform.rotation.y, gameObject1.transform.rotation.z, gameObject1.transform.rotation.w);
        gameObject1.transform.SetPositionAndRotation(gameObject2.transform.position, gameObject2.transform.rotation);
        gameObject2.transform.SetPositionAndRotation(copyPositionGameObject, copyRotationGameObject);
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
    /// <summary>
    /// Verify the number of selected object to invert, and if it's equal of 2 so we invert the objects
    /// </summary>
    private void VerifyGameObjetsSelectedToInvert()
    {
        if (this.m_GameObjetsSelectedToInvert.Count == 2)
        {
            this.InvertTwoGameObjects(this.m_GameObjetsSelectedToInvert[0], this.m_GameObjetsSelectedToInvert[1]);
            this.m_GameObjetsSelectedToInvert.Clear();
        }
    }

    /// <summary>
    /// Verify if GameObjets Selected To Invert has well placed
    /// </summary>
    private void VerifyGameObjectsHasWellPlaced()
    {
        if (!this.m_MiniGameIsFinished && this.GameObjectsHasWellPlaced())
        {
            if (this.m_GameObjectToDesactivate)
            {
                this.m_GameObjectToDesactivate.SetActive(false);
                EventManager.Instance.Raise(new PlaySFXEvent() { eAudioClip = this.m_FinishedMiniGamesSFXAudioCLip });
            }
            
            this.m_MiniGameIsFinished = true;
        }
    }

    /// <summary>
    /// GameObjectsHasWellPlaced
    /// </summary>
    /// <returns>If the GameObjects Has Well Placed</returns>
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
        this.m_NextInvertedTime = Time.time;
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
