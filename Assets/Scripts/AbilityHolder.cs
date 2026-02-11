using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    private Image icon;

    private AbilityManager abilityManager;
    [SerializeField] private int abilitySlot;
    [SerializeField] private BaseAbility ability;
    [SerializeField] private GameObject upgradeIcon;

    [SerializeField] private Healthbar progressBar;

    private Button button;

    public bool active = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        icon = GetComponent<Image>();

        abilityManager = FindFirstObjectByType<AbilityManager>();
        if (abilityManager != null)
        {
            abilityManager.abilityChange.AddListener(UpdateHolder);
        }

        button = GetComponent<Button>();

        UpdateHolder();


    }

    private void Update()
    {
        if (ability != null && ability.onCooldown)
        {
            active = false;
            progressBar.gameObject.SetActive(true);

            float cooldownValue = ability.cooldownTime / ability.cooldown;

            progressBar.ChangeValue(cooldownValue);

            button.interactable = false;
        }
        else
        {
            active = true;
            progressBar.gameObject.SetActive(false);

            button.interactable = true;
        }

        

    }
        
    





    public void UpdateHolder()
    {

        Debug.Log("UpdateHolder called");
        if (abilityManager.equippedAbilities.Count > abilitySlot)
        {
            ability = abilityManager.equippedAbilities[abilitySlot];

            if (ability != null)
            {
                icon.sprite = ability.abilityIcon;

                if (ability.upgraded)
                {
                    upgradeIcon.SetActive(true);
                }
                else
                {
                    upgradeIcon.SetActive(false);
                }

            }
            else
            {
                icon.sprite = null;
                upgradeIcon.SetActive(false);
            }
        }

        else
        {
            ability = null;
            icon.sprite = null;
            upgradeIcon.SetActive(false);
        }
        



        
    }



    
}
