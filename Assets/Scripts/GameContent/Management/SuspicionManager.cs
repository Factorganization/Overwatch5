using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameContent.Actors.EnemySystems.Seekers;
using Systems;
using UnityEngine;

namespace GameContent.Management
{
    public class SuspicionManager : MonoBehaviour
    {
        #region properties

        public static SuspicionManager Manager { get; private set; }

        public HeroHealth PlayerHealth => _playerHealth;
        
        public bool IsInvestigating { get; private set; }

        public bool IsTracking { get; private set; }

        public Vector3 TrackedPos { get; private set; }

        public Vector3 StartDebugPos => _debugHoundStartPos;

        public float Range => closestEnemyRange;
        
        public float DetectionTime { get; set; }

        public float TrackTimer { get; set; }
        
        public EnemyCamera DetectedCamera
        {
            get => detectedCamera;
            set => detectedCamera = value;
        }
        
        #endregion

        #region methodes

        private void Awake()
        {
            if (Manager is not null)
            {
                //Debug.Log("There was already a suspicion manager in the scene, duplicate was removed safely");
                //Destroy(gameObject);
                //return;
            }

            Manager = this;
        }

        private async void Start()
        {
            await Task.Delay(3000);
            if (playerTransform == null)
            {
                playerTransform = GameManager.Instance.playerTransform;
            }
            
            _suspicionLevel = 0;
            _playerHealth = playerTransform.GetComponent<HeroHealth>();
            
            listOfHounds = FindObjectsByType<Hound>(FindObjectsSortMode.None).ToList();
        }

        private void Update()
        {
            _suspicionDecreaseTimer -= Time.deltaTime;

            if (DetectionTime > minCameraTimeForSuspicion && !IsTracking)
            {
                DetectionTime = 0;
                IsTracking = true;
                
                if (detectedCamera == null)
                {
                    return;
                }
                
                if (detectedCamera.CameraType == CameraType.Drone)
                {
                    StartTrack(detectedCamera);
                }
                else
                {
                    StartTrack(detectedCamera.NetworkNode.actor);
                }
            }

            if (_needPersistentLocation)
            {
                _persistentLocationCounter += Time.deltaTime;

                if (_persistentLocationCounter > 3)
                {
                    _needPersistentLocation = false;
                    StartTrackPlayer(Hero.Instance);
                }
            }

            if (IsTracking)
            {
                TrackTimer += Time.deltaTime;
                if (TrackTimer > 7.5f)
                {
                    IsTracking = false;
                    TrackTimer = 0;
                }
            }
            //return; 
            if (_suspicionLevel > 0 && _suspicionDecreaseTimer < 0)
            {
                RemoveSuspicion(suspicionDecreasePerSecond);
                _suspicionDecreaseTimer = 1;
            }

            if (_suspicionLevel > investigationLevel && !IsInvestigating)
            {
                IsInvestigating = true;
            }

            if (_suspicionLevel > trackingLevel && !IsTracking)
            {
                IsTracking = true;
            }

            /*if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Starting track on player");
                StartTrackPlayer(Hero.Instance);
            }*/
        }

        public void AddSuspicion(float value) => _suspicionLevel += value;

        public void RemoveSuspicion(float value) => _suspicionLevel -= value;
        
        public void ResetSuspicion()
        {
            _suspicionLevel = 0;
            IsInvestigating = false;
            IsTracking = false;
            TrackedPos = Vector3.zero;
            TrackTimer = 0;
        }
        
        
        public void StartTrack(EnemyCamera cam)
        {
            IsTracking = true;
            TrackedPos = cam.BaitTarget.position;
            closestHounds = GetClosestEnemy(cam.transform.position, closestEnemyRange);
            
            foreach (Hound hound in closestHounds)
            {
                if (!hound || !hound.gameObject.activeInHierarchy || !hound.HasPlayerInZone)
                    continue;

                hound.SetTargetPosition(TrackedPos);
            }
        }

        public void StartTrackPlayer(Hero player)
        {
            _needPersistentLocation = true;
            _persistentLocationCounter = 0;
            IsTracking = true;
            TrackedPos = player.transform.position;
            closestHounds = GetClosestEnemy(player.transform.position, closestEnemyRange);
            
            foreach (Hound hound in closestHounds)
            {
                if (!hound || !hound.gameObject.activeInHierarchy || !hound.HasPlayerInZone)
                    continue;

                hound.SetTargetPosition(player.transform.position);
            }
        }

        private List<Hound> GetClosestEnemy(Vector3 pos, float range)
        {
            List<Hound> closestEnemy = new List<Hound>();
            float closestDistance = range * range;

            foreach (Hound hound in listOfHounds)
            {
                Debug.Log(hound.HasPlayerInZone);
                if (!hound || !hound.gameObject.activeInHierarchy || !hound.HasPlayerInZone)
                    continue;

                float distance = (hound.transform.position - pos).sqrMagnitude;
                
                if (distance < closestDistance)
                {
                    closestEnemy.Add(hound);
                }
            }
            return closestEnemy;
        }
        
        #endregion

        #region fields

        [SerializeField] private float closestEnemyRange;
        
        [SerializeField] private float investigationLevel;

        [SerializeField] private float trackingLevel;
        
        [SerializeField] private float suspicionDecreasePerSecond;

        [SerializeField] private float minCameraTimeForSuspicion;
        
        [SerializeField] private Transform playerTransform;

        [SerializeField] private Drone[] dronesManualPool;

        [SerializeField] private PoolData<Hound> houndPoolData;

        [SerializeField] private MeshRenderer[] suspicionRenderer;
        
        //[SerializeField] private Hound debugHound;
        
        [SerializeField] private List<Hound> listOfHounds = new List<Hound>();

        [SerializeField] private List<Hound> closestHounds = new List<Hound>();
        
        [SerializeField] private EnemyCamera detectedCamera;
        
        private Vector3 _debugHoundStartPos;
        
        private Pool<Hound> _houndPool;

        private HeroHealth _playerHealth;
        
        private float _suspicionLevel; //Game Core

        private float _suspicionDecreaseTimer;

        private bool _needPersistentLocation;

        private float _persistentLocationCounter;

        #endregion
    }
}