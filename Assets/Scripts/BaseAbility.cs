using UnityEngine;


[CreateAssetMenu(fileName = "BaseAbility", menuName = "Abilities/Base Ability")]
public abstract class BaseAbility : ScriptableObject
{
    public bool upgraded = false;

    public bool onCooldown = false;

    public Sprite abilityIcon;

    public float cooldown;
    public float cooldownTime;
    public float manaCost;
   

    public void OnActivate() 
    {
        if (onCooldown) return;
        Activate();
        onCooldown = true;
    }

    public abstract void Activate();

    public virtual void OnStart() { }

    public void StartCooldown()
    {
        cooldownTime = 0;
        onCooldown = true;
    }

    public void IncrementCooldown()
    {
        if (!onCooldown) return;
        cooldownTime += Time.deltaTime;
        if (cooldownTime >= cooldown)
        {
            onCooldown = false;
            cooldownTime = 0;
        }
    }
}
