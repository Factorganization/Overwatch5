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
        
        public float DetectionTime { get; set; }

        public float TrackTimer { get; set; }
        
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
            _debugHoundStartPos = debugHound.transform.position;
            _playerHealth = playerTransform.GetComponent<HeroHealth>();
        }

        private void Update()
        {
            _suspicionDecreaseTimer -= Time.deltaTime;

            if (DetectionTime > minCameraTimeForSuspicion)
            {
                DetectionTime = 0;
                StartTrackPlayer(Hero.Instance);
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
            
            debugHound.SetTargetPosition(_debugHoundStartPos);
        }

        public void StartTrack(EnemyCamera cam)
        {
            IsTracking = true;
            TrackedPos = cam.BaitTarget.position;
            
            debugHound.SetTargetPosition(cam.BaitTarget.position);
        }

        public void StartTrackPlayer(Hero player)
        {
            IsTracking = true;
            TrackedPos = player.transform.position;
            
            debugHound.SetTargetPosition(player.transform.position);
        }
        
        #endregion

        #region fields

        [SerializeField] private float investigationLevel;

        [SerializeField] private float trackingLevel;
        
        [SerializeField] private float suspicionDecreasePerSecond;

        [SerializeField] private float minCameraTimeForSuspicion;
        
        [SerializeField] private Transform playerTransform;

        [SerializeField] private Drone[] dronesManualPool;

        [SerializeField] private PoolData<Hound> houndPoolData;

        [SerializeField] private MeshRenderer[] suspicionRenderer;
        
        [SerializeField] private Hound debugHound;

        private Vector3 _debugHoundStartPos;
        
        private Pool<Hound> _houndPool;

        private HeroHealth _playerHealth;
        
        private float _suspicionLevel; //Game Core

        private float _suspicionDecreaseTimer;

        #endregion
    }
}