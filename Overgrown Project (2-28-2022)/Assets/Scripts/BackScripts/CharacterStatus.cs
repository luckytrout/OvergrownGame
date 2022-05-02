using UnityEngine;
 
[CreateAssetMenu(fileName = "HealthStatusData", menuName = "StatusObjects/Health", order = 1)]
public class CharacterStatus : ScriptableObject
{
    public string charName = "name";
    public float[] position = new float[2];
    public GameObject characterGameObject;
    public int level = 1;
    public float maxHealth = 100;
    public float maxMana = 100;
    public float health = 100;
    public float mana = 100;

}