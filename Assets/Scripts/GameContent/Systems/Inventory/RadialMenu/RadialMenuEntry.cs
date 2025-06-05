using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class RadialMenuEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void RadialMenuEntryDelegate(RadialMenuEntry entry);
    
    [SerializeField] private TextMeshProUGUI Label, NumberOfItems;
    [SerializeField] private Image Icon;
    [SerializeField] private RectTransform rect;

    private RadialMenuEntryDelegate Callback;

    public RectTransform Rect => rect;
    
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Refresh(string pLabel, Sprite pIcon, int pQuantity)
    {
        SetLabel(pLabel);
        SetIcon(pIcon);
        SetNumberOfItems(pQuantity);
    }
    
    public void SetNumberOfItems(int number)
    {
        NumberOfItems.text = number.ToString();
    }

    public void SetLabel(string label)
    {
        Label.text = label;
    }

    public void SetIcon(Sprite icon)
    {
        Icon.sprite = icon;
    }

    public Image GetIcon()
    {
        return Icon;
    }
    
    public void SetCallback(RadialMenuEntryDelegate callback)
    {
        Callback = callback;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Callback?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOComplete();
        rect.DOScale(Vector3.one * 1.5f, .3f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOComplete();
        rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
    }
}
