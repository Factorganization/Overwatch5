using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Systems;
using Systems.Inventory;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] private GameObject radialMenuEntryPrefab;
    [SerializeField] private int numberOfEntries;
    [SerializeField] private float radius = 300f;

    [SerializeField] private Image targetIcon;
    
    List<RadialMenuEntry> entries;

    public bool isOpen;
    
    private void Start()
    {
        entries = new List<RadialMenuEntry>();
        
            numberOfEntries = Inventory.Instance.Controller.Model.Items.Count;

        for (int i = 0; i < numberOfEntries; i++)
        {
            if (Inventory.Instance.Controller.Model.Items[i].details.Name == null) continue;
            
            AddEntry(Inventory.Instance.Controller.Model.Items[i].details.icon,
                     Inventory.Instance.Controller.Model.Items[i].quantity,
                     EquipItem);
            
            entries[i].gameObject.SetActive(false);
        }
        
        Rearrange();
    }
    
    void AddEntry( Sprite pIcon, int pQuantity, RadialMenuEntry.RadialMenuEntryDelegate Callback)
    {
        GameObject entry = Instantiate(radialMenuEntryPrefab, transform);
        RadialMenuEntry rme = entry.GetComponent<RadialMenuEntry>();
        
        rme.SetIcon(pIcon);
        rme.SetCallback(Callback);
        rme.SetNumberOfItems(pQuantity);
        
        entries.Add(rme);
    }
    
    public void RefreshVisual()
    {
        for (int i = 0; i < numberOfEntries; i++)
        {
            if (Inventory.Instance.Controller.Model.Items[i].details.Name == null) continue;
            
            entries[i].Refresh(Inventory.Instance.Controller.Model.Items[i].details.Name,
                               Inventory.Instance.Controller.Model.Items[i].details.icon,
                               Inventory.Instance.Controller.Model.Items[i].quantity);
        }
    }

    public void Open()
    {
        targetIcon.gameObject.SetActive(true);
        isOpen = true;
        
        for (int i = 0; i < entries.Count; i++)
        {
            RectTransform rect = entries[i].Rect;
            var i1 = i;
            rect.DOAnchorPos(Vector3.zero, .01f).SetEase(Ease.OutQuad).onComplete =
                delegate()
                {
                    entries[i1].gameObject.SetActive(true);
                };
        }
        
        Rearrange();
    }

    public void Close()
    {
        targetIcon.gameObject.SetActive(false);
        
        for (int i = 0; i < entries.Count; i++)
        {
            RectTransform rect = entries[i].Rect;

            var i1 = i;
            rect.DOAnchorPos(Vector3.zero, .1f).SetEase(Ease.OutQuad).onComplete =
                delegate()
                {
                    entries[i1].gameObject.SetActive(false);
                };
        }
        
        isOpen = false;
    }

    void Rearrange()
    {
        float radiansOfSeparation = 2f * Mathf.PI / entries.Count;
        for (int i = 0; i < entries.Count; i++)
        {
            float x = Mathf.Sin(radiansOfSeparation * i) * radius;
            float y = Mathf.Cos(radiansOfSeparation * i) * radius;

            entries[i].Rect.localScale = Vector3.zero;
            entries[i].Rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad).SetDelay(.05f * i);
            entries[i].Rect.DOAnchorPos(new Vector3(x, y, 0), .3f).SetEase(Ease.OutQuad).SetDelay(.05f * i);

            switch (i)
            {
                case 1:
                    entries[i].BackgroundImage.transform.rotation = Quaternion.Euler(0, 0, -90f);
                    entries[i].BackgroundImage.transform.position = new Vector3(25, 0, 0);
                    break;
                case 2:
                    entries[i].BackgroundImage.transform.rotation = Quaternion.Euler(0, 0, 90f);
                    entries[i].BackgroundImage.transform.position = new Vector3(-25, 0, 0);
                    break;
            }
        }
    }
    
    void EquipItem(RadialMenuEntry icon)
    {
        Inventory.Instance.EquipItem(Inventory.Instance.Controller.Model.Items[entries.IndexOf(icon)].details);

        if (Hero.Instance.CurrentEquipedItem == Inventory.Instance.Controller.Model.Items[entries.IndexOf(icon)].details)
        {
            entries[entries.IndexOf(icon)].SetIcon(Inventory.Instance.Controller.Model.Items[entries.IndexOf(icon)].details.chosenIcon);
        }
    }
}
