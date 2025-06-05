using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class RunTimeUI : MonoBehaviour
{
    [SerializeField] private Camera NetworkMapCamera;
    private UIDocument UIDocument;
    private LayerMask FloorLayerMask;
    
    private VisualElement NetworkMap;
    
    private bool MouseDownOnNetworkMap;
    private bool mapOpen;

    private void Awake()
    {
        UIDocument = GetComponent<UIDocument>();
        NetworkMap = UIDocument.rootVisualElement.Q("NetworkMap");
        NetworkMap.style.display = DisplayStyle.None;
        
        SetupNetworkMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMap();
        }
    }

    private void ToggleMap()
    {
        mapOpen = !mapOpen;
        NetworkMap.style.display = mapOpen ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    public void RevealFromJunction(NetworkNode junction)
    {
        
    }

    private void SetupNetworkMap()
    {
        NetworkMap = UIDocument.rootVisualElement.Q("NetworkMap");
        FloorLayerMask = LayerMask.GetMask("Ground");
        NetworkMap.RegisterCallback<MouseDownEvent>(HandleMouseDown);
        NetworkMap.RegisterCallback<MouseMoveEvent>(HandleMouseMove);
        NetworkMap.RegisterCallback<MouseUpEvent>(HandleMouseUp);
        NetworkMap.RegisterCallback<MouseLeaveEvent>(HandleMouseLeave);
    }
    
    private void HandleMouseDown(MouseDownEvent evt)
    {
       MouseDownOnNetworkMap = true;
       
       MoveNetworkMapCamera(evt.mousePosition);
    }

    private void HandleMouseMove(MouseMoveEvent evt)
    {
        if (MouseDownOnNetworkMap)
        {
            MoveNetworkMapCamera(evt.mousePosition);
        }
    }
    
    private void HandleMouseUp(MouseUpEvent evt)
    {
        MouseDownOnNetworkMap = false;
    }
    
    private void HandleMouseLeave(MouseLeaveEvent evt)
    {
        MouseDownOnNetworkMap = false;
    }

    private void MoveNetworkMapCamera(Vector2 mousePos)
    {
        Vector2 screenPos = new Vector2(mousePos.x, Screen.height - mousePos.y);
        
        Ray ray = NetworkMapCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, FloorLayerMask))
        {
            Debug.Log("Hit: " + hit.collider.name);
            Vector3 targetPosition = hit.point;
            targetPosition.y = NetworkMapCamera.transform.position.y; // Keep the camera at the same height
            NetworkMapCamera.transform.position = targetPosition;
        }
    }
}
