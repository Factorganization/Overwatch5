using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory
{
    public abstract class StorageView : MonoBehaviour
    {
        public Slot[] Slots;

        [SerializeField] protected UIDocument document;
        [SerializeField] protected StyleSheet styleSheet;

        protected static VisualElement ghostIcon;

        private static bool isDragging;
        private static Slot originalSlot;
        
        protected VisualElement root;
        protected VisualElement container;

        public event Action<Slot, Slot> OnDrop;

        void Start()
        {
            ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            foreach (var slot in Slots)
            {
                slot.OnstartDrag += OnPointerDown;
            }
        }

        public abstract IEnumerator InitView(ViewModel viewModel);
        
        private void OnPointerDown(Vector2 pos, Slot slot)
        {
            isDragging = true;
            originalSlot = slot;
            
            SetGhostIconPos(pos);
            
            ghostIcon.style.backgroundImage = originalSlot.BaseSprite.texture;
            originalSlot.Icon.image = null;
            originalSlot.StackLabel.visible = false;

            ghostIcon.style.opacity = 0.8f;
            ghostIcon.style.visibility = Visibility.Visible;
        }
        
        static void SetGhostIconPos(Vector2 pos)
        {
            ghostIcon.style.left = pos.x - ghostIcon.layout.height / 2;
            ghostIcon.style.top = pos.y - ghostIcon.layout.width / 2;
        }
        
        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!isDragging) return;
            
            SetGhostIconPos(evt.position);
        }
        
        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!isDragging) return;
            
            Slot closestSlot = Slots
                .Where(slot => slot.worldBound.Overlaps(ghostIcon.worldBound))
                .OrderBy(slot => Vector2.Distance(slot.worldBound.center, ghostIcon.worldBound.position))
                .FirstOrDefault();

            if (closestSlot != null)
            {
                OnDrop?.Invoke(originalSlot, closestSlot);
            }
            else
            {
                originalSlot.Icon.image = originalSlot.BaseSprite.texture;
            }
            
            isDragging = false;
            originalSlot = null;
            ghostIcon.style.visibility = Visibility.Hidden;
        }
    }
}

