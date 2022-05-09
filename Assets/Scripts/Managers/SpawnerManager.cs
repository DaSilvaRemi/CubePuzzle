using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class SpawnerManager : Manager<SpawnerManager>, IEventHandler
{
    [Header("Spawner Manager GameObjects properties")]
    [Tooltip("List Game objects to spawn")]
    [SerializeField] private GameObject[] m_GameObjectsToSpawn;
    [Tooltip("Spawns position")]
    [SerializeField] private Transform[] m_SpawnsPosition;

    [Header("Spawner Manager limit properties")]
    [Tooltip("Cooldown duration")]
    [SerializeField] private float m_SpawnCooldownDuration = 0.2f;
    [Tooltip("Spawn Limit")]
    [SerializeField] private float m_SpawnLimit = 30;
    [Tooltip("Is Spawning continously")]
    [SerializeField] private bool m_IsTimedSpawning;

    private List<GameObject> m_GameObjectsSpawned;
    private float m_NextSpawnTime;
    private IEnumerator m_TimedSpawnCoroutine;

    #region SpawnManager properties
    public bool IsTimedSpawnLimit { get => this.m_IsTimedSpawning; }

    public bool HasReachedSpawnLimit { get => this.m_GameObjectsSpawned.Count >= this.m_SpawnLimit; }
    #endregion

    #region SpawnerManager Spawn Methods
    /// <summary>
    /// Spawn an object in spawns
    /// </summary>
    private void Spawn()
    {
        int gameObjectsIndex = UnityEngine.Random.Range(0, this.m_GameObjectsToSpawn.Length);
        GameObject gameObject = Instantiate(this.m_GameObjectsToSpawn[gameObjectsIndex]);
        this.Spawn(gameObject);
    }

    /// <summary>
    /// Spawn a number of objectds
    /// </summary>
    /// <param name="numberGoToSpawn">Number Go to spawn</param>
    private void Spawn(int numberGoToSpawn)
    {
        for (int i = 0; i < numberGoToSpawn; i++)
        {
            this.Spawn();
        }
    }

    /// <summary>
    /// Spawn a list of game objects
    /// </summary>
    /// <param name="gameObjectsToSpawn">List to spawn</param>
    private void Spawn(List<GameObject> gameObjectsToSpawn)
    {
        for (int i = 0; i < gameObjectsToSpawn.Count; i++)
        {
            this.Spawn(gameObjectsToSpawn[i]);
        }
    }

    /// <summary>
    /// Spawn a game object
    /// </summary>
    /// <param name="gameObjectToSpawn">Game objects to spawn</param>
    private void Spawn(GameObject gameObjectToSpawn)
    {
        if(!this.HasReachedSpawnLimit)
        {
            int spawnPositionIndex = UnityEngine.Random.Range(0, this.m_SpawnsPosition.Length);
            gameObjectToSpawn.transform.SetPositionAndRotation(this.m_SpawnsPosition[spawnPositionIndex].position, this.m_SpawnsPosition[spawnPositionIndex].rotation);
            this.m_GameObjectsSpawned.Add(gameObjectToSpawn);
        }
    }

    /// <summary>
    /// Spawn a GameOject at each next cooldown
    /// </summary>
    private void SpawnEachNextCooldown()
    {
        if (this.m_IsTimedSpawning && Time.time > m_NextSpawnTime)
        {
            this.m_NextSpawnTime = this.m_SpawnCooldownDuration + Time.time;
            this.Spawn();
        }
    }

    /// <summary>
    /// Spawn each time a game object
    /// </summary>
    /// <param name="time">The time to spawn other game object</param>
    private void SpawnEachTime(float time)
    {
        this.StopSpawnEachTime();
        this.m_TimedSpawnCoroutine = Tools.MyWaitCoroutine(time, null, () => this.Spawn());
        base.StartCoroutine(this.m_TimedSpawnCoroutine);
    }
    #endregion

    #region SpawnerManager Utils methods
    /// <summary>
    /// Start the cooldown spawn 
    /// </summary>
    private void StartSpawnCooldown()
    {
        this.m_IsTimedSpawning = true;
    }

    /// <summary>
    /// Stop timed spawn <see cref="SpawnEachTime(float)"/>
    /// </summary>
    private void StopSpawnEachTime()
    {
        if (this.m_TimedSpawnCoroutine != null)
        {
            base.StopCoroutine(this.m_TimedSpawnCoroutine);
            this.m_TimedSpawnCoroutine = null;
        }
    }

    /// <summary>
    /// Stop cooldown spawn
    /// </summary>
    private void StopSpawnCooldown()
    {
        this.m_IsTimedSpawning = false;
    }

    /// <summary>
    /// StopTimedSpawns with <see cref="StopSpawnEachTime"/> and <seealso cref="StopSpawnCooldown"/>
    /// </summary>
    private void StopTimedSpawns()
    {
        this.StopSpawnEachTime();
        this.StopSpawnCooldown();
    }

    /// <summary>
    /// Destroy An Game Object Spawned after it is destroy
    /// </summary>
    /// <param name="gameObjectToDestroy">The GameObject</param>
    private void DestroyAnGameObjectSpawned(GameObject gameObjectToDestroy)
    {
        this.m_GameObjectsSpawned.Remove(gameObjectToDestroy);
        GameObject.Destroy(gameObjectToDestroy);
    }

    /// <summary>
    /// Destroy all spawned game objects
    /// </summary>
    private void DestroyAllSpawnedGameObjects()
    {
        this.m_GameObjectsSpawned.ForEach((gameObjectSpawned) => GameObject.Destroy(gameObjectSpawned));
        this.m_GameObjectsSpawned.Clear();
    }
    #endregion

    #region SpawnManagers Own Update Methods
    /// <summary>
    /// Update Cooldown Spawn <see cref="StopSpawnCooldown"/> and <seealso cref="SpawnEachNextCooldown"/>
    /// </summary>
    private void UpdateCooldownSpawn()
    {
        if (this.HasReachedSpawnLimit)
        {
            this.StopSpawnCooldown();
        }

        this.SpawnEachNextCooldown();
    }
    #endregion

    #region Events Listeners
    /// <summary>
    /// OnSpawnEachTimeEvent <see cref="SpawnEachTimeEvent"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnEachTimeEvent(SpawnEachTimeEvent e)
    {
        this.SpawnEachTime(e.eSpawnTime);
    }

    /// <summary>
    /// OnSpawnedGameObjectDestroyedEvent <see cref="SpawnedGameObjectToDestroyEvent"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnedGameObjectDestroyedEvent(SpawnedGameObjectToDestroyEvent e)
    {
        this.DestroyAnGameObjectSpawned(e.eGameObjectToDestroy);
    }

    /// <summary>
    /// OnSpawnNbGOEvent <see cref="SpawnNbGOEvent"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnNbGOEvent(SpawnNbGOEvent e)
    {
        this.Spawn(e.eNbGOToSpawn);
    }

    /// <summary>
    /// OnSpawnGameObjectEvent <see cref="SpawnGameObjectEvent"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnGameObjectEvent(SpawnGameObjectEvent e)
    {
        this.Spawn(e.eGameObjectToSpawn);
    }

    /// <summary>
    /// OnSpawnGameObjectsEvent <see cref="SpawnGameObjectsEvent"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnGameObjectsEvent(SpawnGameObjectsEvent e)
    {
        this.Spawn(e.eGameObjectsToSpawn);
    }

    /// <summary>
    /// OnSpawnGameObjectsEvent <see cref="StartCooldownSpawnEvent"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnGameObjectsEvent(StartCooldownSpawnEvent e)
    {
        this.StartSpawnCooldown();
    }

    /// <summary>
    /// OnStopEachTimeSpawnEvent <see cref="StopEachTimeSpawnEvent"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnStopEachTimeSpawnEvent(StopEachTimeSpawnEvent e)
    {
        this.StopSpawnEachTime();
    }

    /// <summary>
    /// OnStopTimedSpawnEvent <see cref="StopTimedSpawnEvent"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnStopTimedSpawnEvent(StopTimedSpawnEvent e)
    {
        this.StopTimedSpawns();
    }
    #endregion

    #region Events Subscription
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<SpawnEachTimeEvent>(OnSpawnEachTimeEvent);
        EventManager.Instance.AddListener<SpawnedGameObjectToDestroyEvent>(OnSpawnedGameObjectDestroyedEvent);
        EventManager.Instance.AddListener<SpawnNbGOEvent>(OnSpawnNbGOEvent);
        EventManager.Instance.AddListener<SpawnGameObjectEvent>(OnSpawnGameObjectEvent);
        EventManager.Instance.AddListener<SpawnGameObjectsEvent>(OnSpawnGameObjectsEvent);
        EventManager.Instance.AddListener<StartCooldownSpawnEvent>(OnSpawnGameObjectsEvent);
        EventManager.Instance.AddListener<StopEachTimeSpawnEvent>(OnStopEachTimeSpawnEvent);
        EventManager.Instance.AddListener<StopTimedSpawnEvent>(OnStopTimedSpawnEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<SpawnEachTimeEvent>(OnSpawnEachTimeEvent);
        EventManager.Instance.RemoveListener<SpawnedGameObjectToDestroyEvent>(OnSpawnedGameObjectDestroyedEvent);
        EventManager.Instance.RemoveListener<SpawnNbGOEvent>(OnSpawnNbGOEvent);
        EventManager.Instance.RemoveListener<SpawnGameObjectEvent>(OnSpawnGameObjectEvent);
        EventManager.Instance.RemoveListener<SpawnGameObjectsEvent>(OnSpawnGameObjectsEvent);
        EventManager.Instance.RemoveListener<StartCooldownSpawnEvent>(OnSpawnGameObjectsEvent);
        EventManager.Instance.RemoveListener<StopEachTimeSpawnEvent>(OnStopEachTimeSpawnEvent);
        EventManager.Instance.RemoveListener<StopTimedSpawnEvent>(OnStopTimedSpawnEvent);
    }
    #endregion

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        this.SubscribeEvents();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvents();
    }

    private void Awake()
    {
        this.m_GameObjectsSpawned = new List<GameObject>();
        base.InitManager();
    }

    private void Start()
    {
        this.m_NextSpawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        this.UpdateCooldownSpawn();
    }

    private void OnDestroy()
    {
        this.DestroyAllSpawnedGameObjects();
    }
    #endregion
}
