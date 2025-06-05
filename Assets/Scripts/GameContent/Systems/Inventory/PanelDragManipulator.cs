using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory
{
    public class PanelDragManipulator : PointerManipulator
    {
        bool isDragging;
        private Vector2 offset;
        
        public PanelDragManipulator()
        {
            activators.Add(new ManipulatorActivationFilter {button = MouseButton.LeftMouse});
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }
        
        private void OnPointerDown(PointerDownEvent evt)
        {
            if (!CanStartManipulation(evt) || isDragging) return;
            
            offset = evt.localPosition;
            isDragging = true;
            
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }
        
        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!isDragging || !target.HasPointerCapture(evt.pointerId)) return;
            
            Vector3 delta = evt.localPosition - (Vector3)offset;
            target.transform.position += delta;
            evt.StopPropagation();
        }
        
        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!CanStopManipulation(evt) || !isDragging) return;
            
            isDragging = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }
    }
}