using Systems.Inventory;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDetails))]
public class ItemIconPreview : UnityEditor.Editor
{
    ItemDetails itemDetails;
    
    private void OnEnable()
    {
        itemDetails = target as ItemDetails;
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (itemDetails.icon == null) return;
        
        Texture2D sprite = AssetPreview.GetAssetPreview(itemDetails.icon);
        
        GUILayout.Label(sprite, GUILayout.Height(100), GUILayout.Width(100));
        
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), sprite);
    }
}
