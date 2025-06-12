using Unity.VisualScripting;
using UnityEngine;

public class OnLight : MonoBehaviour
{
    public LightReference LightReference;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LightReference.light.SetActive(true);
        }
    }
}
