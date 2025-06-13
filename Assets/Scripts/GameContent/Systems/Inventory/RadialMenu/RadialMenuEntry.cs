using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class RadialMenuEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void RadialMenuEntryDelegate(RadialMenuEntry entry);
    
    [SerializeField] private TextMeshProUGUI NumberOfItems;
    [SerializeField] private Image Icon, BGImage;
    [SerializeField] private RectTransform rect;

    private RadialMenuEntryDelegate Callback;
    
    public Image BackgroundImage => BGImage;

    public RectTransform Rect => rect;
    
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Refresh(string pLabel, Sprite pIcon, int pQuantity)
    {
        SetIcon(pIcon);
        SetNumberOfItems(pQuantity);
    }
    
    public void SetNumberOfItems(int number)
    {
        NumberOfItems.text = number.ToString();
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
        rect.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOComplete();
        rect.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }
}
