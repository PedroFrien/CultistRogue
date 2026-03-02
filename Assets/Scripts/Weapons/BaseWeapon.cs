using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public float damage;

    public Vector3 offsetPos;
    public Vector3 rotateOffsetPos;

    public float noiseRadius;


    public abstract void Use();



    


}
