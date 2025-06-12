using UnityEngine;

public class OffLight : MonoBehaviour
{
    public LightReference LightReference;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LightReference.light.SetActive(false);
        }
    }
}
