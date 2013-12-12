using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour 
{
	public GameObject testCube;
	public float speed;
	public float rotationSpeed;
	public Quaternion currentRotation;
	private bool charging;
	private Vector3 ChargeLocation;
	public float chargeMultiplier;
	public ParticleSystem chargeEffect;
	// Use this for initialization
	void Start () 
	{
		chargeEffect.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit hit;

		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(r, out hit))
			{
				// left click registered on terrain
				// CHARGE!
				ChargeLocation = new Vector3(hit.point.x, 0f, hit.point.z);
				charging = true;
				chargeEffect.enableEmission = true;
			}
		}


		bool running = false;
		Vector3 directionVector = Vector3.zero;
		bool sprint = false;

		if (!charging)
		{
			if (Input.GetKey (KeyCode.W))
			{
				// move forward some amount
				directionVector += new Vector3(0f, 0f, 1);
				running = true;
			}
			if (Input.GetKey(KeyCode.A))
			{
				directionVector += new Vector3(-1, 0f, 0f);
				running = true;

			}
			if (Input.GetKey(KeyCode.S))
			{
				directionVector += new Vector3(0f, 0f, -1);
				running = true;
			}
			if (Input.GetKey(KeyCode.D))
			{
				directionVector += new Vector3(1, 0f, 0f);
				running = true;
			}

			if (Input.GetKey(KeyCode.LeftShift))
			{
				sprint = true;
			}

			if (running)
			{
				animation.Play ("run");

				// if we're moving, lets rotate towards that direction
				var targetPoint = transform.position + directionVector.normalized;
				var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
			}
			else
			{
				// if we're not, play idle animation and do not rotate, this lets us keep our currently facing direction
				animation.Play("idle");
			}

			float sprintMultiplier = 1f;
			if (sprint)
				sprintMultiplier = 2.5f;

			// move along
			transform.position += (directionVector.normalized * speed * Time.deltaTime * sprintMultiplier);
		}
		else
		{
			// we are charging, move towards charge location at high rate of speed
			transform.position = Vector3.MoveTowards(transform.position, ChargeLocation, speed * Time.deltaTime * chargeMultiplier);
			animation.Play ("run");
			transform.LookAt (ChargeLocation);
			animation["run"].speed = 1.5f;
			//Debug.Log (animation["run"].speed);
			if (transform.position == ChargeLocation)
			{
				charging = false;
				chargeEffect.enableEmission = false;
				animation["run"].speed = 1f;
			}
		}

		// always clamp to ground...
		transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Trigger Entered");

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Rock")
		{
			// we hit a rock, stop moving
			Debug.Log (collision.gameObject.name);
			charging = false;
			animation["run"].speed = 1f;
			chargeEffect.enableEmission = false;
		}

	}
}
