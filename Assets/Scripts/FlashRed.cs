using UnityEngine;
using System.Collections;

public class FlashRed : MonoBehaviour
{
    public IEnumerator Flash(float time)
    {
        Material baseMat = GetComponent<Material>();

        Material newMat = GetComponent<Material>();
        newMat.color = Color.red;

        GetComponent<Renderer>().material = newMat;

        yield return new WaitForSeconds(time);

        GetComponent<Renderer>().material = baseMat;
    }
}
