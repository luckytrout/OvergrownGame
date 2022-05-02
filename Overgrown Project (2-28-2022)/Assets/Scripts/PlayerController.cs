using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float moveSpeed = 0f;
	public float speedModifier = 1f;
	private bool upDirection = false;
	private bool downDirection = false;
	public float rotationSpeed = 10f;

	//player stats
	/*public int currentHP = 100;
	public int maxHP = 100;
	public int XP = 0;
	public float speed = 0;
	public int damage = 10;
	public int level = 1;*/


	private float rotation;
	private Rigidbody rb;
	public bool isBattling = false;
	public GameObject battlingEnemy = null;
	private GameManager gameManager;
	void Start()
	{
		isBattling = false;
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
			rotation = Input.GetAxisRaw("Horizontal");

			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			{
				upDirection = true;

			}
			else
			{
				upDirection = false;
			}

			if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			{
				downDirection = true;
			}
			else
			{
				downDirection = false;
			}
	}

	void FixedUpdate()
	{
		
			if (upDirection)
			{
				moveSpeed = 5 * speedModifier;
			}
			else if (downDirection)
			{
				moveSpeed = -5 * speedModifier;
			}
			else
			{
				moveSpeed = 0;
			}

			/*if (GameManager.Instance.panelPlay.activeSelf == false)
			{
				moveSpeed = 0;
				rotationSpeed = 0;
			}*/

			/*if (gameManager._state != GameManager.State.PLAY)
			{
				moveSpeed = 0;
				rotationSpeed = 0;
			}*/


			rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
			Vector3 yRotation = Vector3.up * rotation * rotationSpeed * Time.fixedDeltaTime;
			Quaternion deltaRotation = Quaternion.Euler(yRotation);
			Quaternion targetRotation = rb.rotation * deltaRotation;
			rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.deltaTime));
			//transform.Rotate(0f, rotation * rotationSpeed * Time.fixedDeltaTime, 0f, Space.Self);
	}

	void OnCollisionEnter(Collision col)
	{
		//commence battle
		if (col.gameObject.tag == "EnemyClone") //&& ((col.gameObject).GetComponent<EnemyBehavior>().enemyCurrentHP) > 0
		{
			//Debug.Log("enemy");
			//Debug.Log(isBattling);
            if (!isBattling)
            {
				battlingEnemy = col.gameObject;
				isBattling = true;
				//Debug.Log("ENEMY DETECTED");
			}
		}
	}
}
