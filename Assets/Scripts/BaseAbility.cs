using UnityEngine;


[CreateAssetMenu(fileName = "BaseAbility", menuName = "Abilities/Base Ability")]
public class BaseAbility : ScriptableObject
{
    public bool upgraded = false;

    public bool onCooldown = false;

    public Sprite abilityIcon;

    public float cooldown;
    public float cooldownTime;
   

    public virtual void OnActivate() { }

    public virtual void OnStart() { }

    public void StartCooldown()
    {
        cooldownTime = 0;
        onCooldown = true;
    }

    public void IncrementCooldown()
    {
        if (!onCooldown) return;
        Debug.Log("Incrementing Cooldown");
        cooldownTime += Time.deltaTime;
        if (cooldownTime >= cooldown)
        {
            onCooldown = false;
            cooldownTime = 0;
        }
    }
}
