using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public float damage;

    public Vector3 offsetPos;
    public Vector3 rotateOffsetPos;


    public abstract void Use();



    


}
