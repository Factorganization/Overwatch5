using System;
using GameContent.Actors;
using GameContent.Actors.EnemySystems.Seekers;
using Systems.Inventory;
using Systems.Inventory.Interface;
using Systems.Persistence;
using UnityEngine;
using UnityEngine.UI;
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
        
        private HackableJunction _currentJunction;
        private IInteractible _currentInteractible;
        private float _currentHackTimer;
        private bool _isHacking;
        
        [Header("Public Getters/Setters")]
        public HeroHealth Health => _health;
        public MultiTool MultiToolObject => _multiToolObject;
        public Camera Camera => _camera;
        public bool IsHacking => _isHacking;
        public float CurrentHackingTime => _currentHackTimer;
        
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
        
        public void Bind(PlayerData data)
        {
            _data = data;
            _data.Id = Id;
            transform.position = data.position;
            transform.rotation = data.rotation;
        }

        private void Update()
        {
            _data.position = transform.position;
            _data.rotation = transform.rotation;
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactible"))
                {
                    var interactible = hit.collider.GetComponent<IInteractible>();

                    if (interactible != null && interactible != _currentInteractible)
                    {
                        _currentInteractible = interactible;
                        GameUIManager.Instance.UpdateInteractibleUI(interactible.InteractibleName, true);
                    }
                    return;
                }

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Device"))
                {
                    var device = hit.collider.GetComponent<Actor>();
                    
                    if (device != _multiToolObject.CurrentDevice && device is EnemyCamera enemyCamera)
                    {
                        _multiToolObject.CurrentDevice = enemyCamera;
                        GameUIManager.Instance.UpdateInteractibleUI(enemyCamera.NetworkNode.nodeId, true);
                    }
                }
            }
            
            if (_currentInteractible != null)
            {
                _currentInteractible = null;
                GameUIManager.Instance.UpdateInteractibleUI("", false);
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
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance))
            {
                if (_currentEquippedItem.type == Type.MultiTool)
                {
                    if (_currentInteractible is HackableJunction junction && junction._alrHacked == false)
                    {
                        _currentJunction = junction;
                        _isHacking = true;
                        _currentHackTimer = 0f;
                        
                        GameUIManager.Instance.hackProgressImage.gameObject.SetActive(true);
                        GameUIManager.Instance.hackProgressImage.fillAmount = 0;
                        return;
                    }
                    _currentInteractible?.OnInteract();
                    return;
                }
                
                _currentInteractible?.OnInteract();
            }
        }

        public void ContinueHack()
        {
            if (!_currentJunction) return;

            _currentHackTimer += Time.deltaTime;
            
            float progress = _currentHackTimer / _currentJunction.HackingTime;
            GameUIManager.Instance.hackProgressImage.fillAmount = Mathf.Clamp01(progress);

            if (_currentHackTimer >= _currentJunction.HackingTime)
            {
                _currentJunction.OnInteract();
                _currentJunction._alrHacked = true;
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
            GameUIManager.Instance.hackProgressImage.fillAmount = 0;
            GameUIManager.Instance.hackProgressImage.gameObject.SetActive(false);
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