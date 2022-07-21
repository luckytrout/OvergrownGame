using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{

    public Slider playerSlider;
    public TMP_Text playerHealthText;

    public void SetDefaultHealth(int maxHealth)
    {
        playerSlider.maxValue = maxHealth = 100;
        playerSlider.value = maxHealth;
        TextChangeCurrent(maxHealth, maxHealth);
    }

    public void SetMaxHealth(int maxHealth)
    {
        playerSlider.maxValue = maxHealth;
        //TextChange( ,maxHealth);
    }

    public void SetCurrentHealth(int health)
    {
        playerSlider.value = health;
        //(currentHealth, maxHealth);
    }

    public void TextChangeCurrent(int currentHealth, int maxHealth)
    {
        playerHealthText.text = ("HP(" + currentHealth + "/" + maxHealth + ")");
    }
}
