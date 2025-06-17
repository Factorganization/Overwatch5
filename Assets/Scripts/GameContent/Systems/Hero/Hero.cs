using System;
using FMOD.Studio;
using Systems.Inventory;
using Systems.Inventory.Interface;
using Systems.Persistence;
using UnityEngine;
using Type = Systems.Inventory.Type;

namespace Systems
{
    public class Hero : MonoBehaviour, IBind<PlayerData>
    {
        #region Variables

        public static Hero Instance;
        
        [field: SerializeField]public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        
        [Header("Components")]
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerData _data;
        [SerializeField] private HeroHealth _health;
        [SerializeField] private MultiTool _multiToolObject;
        
        [Header("Modifiable Variables")]
        [SerializeField] private ItemDetails _currentEquippedItem;
        [SerializeField] private float _interactDistance;
        
        [SerializeField] private HackableJunction _currentJunction;
        private IInteractible _currentInteractible;
        private float _currentHackTimer;
        private bool _isHacking;
        
        private EventInstance _hackEventInstance;
        
        [Header("Public Getters/Setters")]
        public HeroHealth Health => _health;
        public MultiTool MultiToolObject => _multiToolObject;
        public Camera Camera => _camera;
        public bool IsHacking => _isHacking;
        public float CurrentHackingTime => _currentHackTimer;
        
        public EventInstance HackEventInstance => _hackEventInstance;
        
        public ItemDetails CurrentEquipedItem
        {
            get => _currentEquippedItem;
            set => _currentEquippedItem = value;
        }

        #endregion

        #region Methods

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _hackEventInstance = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.Scan);
        }

        public void Bind(PlayerData data)
        {
            _data = data;
            _data.Id = Id;
            transform.position = data.position;
            transform.rotation = data.rotation;
        }

        public void CheckInteractible()
        {
            if (_currentEquippedItem == null)
            {
                return;
            }
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactibles"))
                {
                    var interactible = hit.collider.GetComponent<IInteractible>();
                    
                    if (interactible != null && interactible != _currentInteractible)
                    {
                        if (interactible is HackableJunction && _currentEquippedItem.type != Type.MultiTool)
                        {
                            return;
                        }
                        
                        _currentInteractible = interactible;
                        GameUIManager.Instance.UpdateInteractibleUI("", "Right click to interact",true);
                    }
                }
            }
            else
            {
                if (_currentInteractible != null)
                {
                    _currentInteractible = null;
                    _currentJunction = null;
                    GameUIManager.Instance.UpdateInteractibleUI("", "E to pick up",false);
                }
            }
        }

        public void UseEquippedItem()
        {
            if (_currentEquippedItem == null) return;

            _currentEquippedItem.OnAction();
        }

        public void TryInteract()
        {
            if (!_currentEquippedItem) return;
            
            if (_currentEquippedItem.type == Type.MultiTool)
            {
                if (_currentInteractible is HackableJunction junction)
                {
                    if (junction._alrHacked)
                    {
                        return;
                    }
                    _currentJunction = junction;
                    _isHacking = true;
                    _currentHackTimer = 0f;
                        
                    GameUIManager.Instance.HackProgressImage.gameObject.SetActive(true);
                    GameUIManager.Instance.HackProgressImage.fillAmount = 0;
                }
                else
                {
                    _currentInteractible?.OnInteract();
                }
            }
            else
            {
                _currentInteractible?.OnInteract();
            }
        }

        public void ContinueHack()
        {
            if (!_currentJunction) return;

            _currentHackTimer += Time.deltaTime;
            
            float progress = _currentHackTimer / _currentJunction.HackingTime;
            GameUIManager.Instance.HackProgressImage.fillAmount = Mathf.Clamp01(progress);
            
            if (_currentHackTimer >= _currentJunction.HackingTime)
            {
                _currentJunction.OnInteract();
                _currentJunction._alrHacked = true;
                AudioManager.Instance.PlayOneShot(
                    FMODEvents.Instance.ScanJunctionSuccess, 
                    gameObject.transform.position);
                GameUIManager.Instance.ShowNotification($"Hacked {_currentJunction.InteractibleName} successfully!", 2f);
                ResetHack();
            }
        }

        public void CancelHack()
        {
            ResetHack();
        }

        private void ResetHack()
        {
            _isHacking = false;
            _currentHackTimer = 0f;
            _currentJunction = null;
            GameUIManager.Instance.HackProgressImage.fillAmount = 0;
            GameUIManager.Instance.HackProgressImage.gameObject.SetActive(false);
            _hackEventInstance.stop(STOP_MODE.IMMEDIATE);
        }

        #endregion
        
    }

    [Serializable]
    public class PlayerData : ISaveable
    {
        [field: SerializeField] public SerializableGuid Id { get; set; }
        public Vector3 position;
        public Quaternion rotation;
    }
}