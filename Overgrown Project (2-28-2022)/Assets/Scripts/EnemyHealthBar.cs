using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    public Slider enemySlider;
    public Text enemyHealthText;

    public void SetDefaultHealth(int maxHealth)
    {
        enemySlider.maxValue = maxHealth;
        enemySlider.value = maxHealth;
        TextChangeCurrent(maxHealth, maxHealth);
        //TextChange(maxHealth, maxHealth);
    }

    public void SetMaxHealth(int maxHealth)
    {
        enemySlider.maxValue = maxHealth;
        //TextChange( ,maxHealth);
    }

    public void SetCurrentHealth(int health)
    {
        enemySlider.value = health;
        //(currentHealth, maxHealth);
    }

    public void TextChangeCurrent(int currentHealth, int maxHealth)
    {
        enemyHealthText.text = ("HP(" + currentHealth + "/" + maxHealth + ")");
    }
}
