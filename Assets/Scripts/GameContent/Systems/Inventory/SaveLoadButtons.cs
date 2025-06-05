using Systems.Persistence;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveLoadButtons : MonoBehaviour
{
    [SerializeField] private UIDocument doc;

    private Button saveButton, newGameButton, loadButton;

    private void Awake()
    {
        saveButton = doc.rootVisualElement.Q<Button>("saveButton");
        saveButton.RegisterCallback<ClickEvent>(SaveGame);
        
        newGameButton = doc.rootVisualElement.Q<Button>("newGameButton");
        newGameButton.RegisterCallback<ClickEvent>(NewGame);
        
        loadButton = doc.rootVisualElement.Q<Button>("loadButton");
        loadButton.RegisterCallback<ClickEvent>(LoadGame);
    }
    
    public void SaveGame(ClickEvent evt)
    {
        SaveLoadSystem.Instance.SaveGame();
    }
    
    public void NewGame(ClickEvent evt)
    {
        SaveLoadSystem.Instance.NewGame();
    }
    
    public void LoadGame(ClickEvent evt)
    {
        SaveLoadSystem.Instance.LoadGame("NewGame");
    }
}
