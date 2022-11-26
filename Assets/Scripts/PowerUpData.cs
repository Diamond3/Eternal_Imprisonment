using UnityEngine;

public enum Type { Green, Yellow, Red}
public enum PowerUpType { AttackSpeed, MovementSpeed, JumpHeight, Shooting}

[CreateAssetMenu(menuName = "Power-Up Data")]
public class PowerUpData : ScriptableObject
{
	public Type Type;
	public PowerUpType PowerUp;

	public float FloatValue;
	public int IntValue;
	public bool BoolValue;

    private void OnValidate()
    {

	}
}