using UnityEngine;
using System.Collections;

public class CharacterCamera : MonoBehaviour 
{
	public Transform target;
	Quaternion rotation;

	void Awake()
	{
		rotation = transform.rotation;
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//this.transform.LookAt (target.position);
	}

	void LateUpdate()
	{
		transform.rotation = rotation;
		Debug.Log (rotation.eulerAngles);
	}

	public void SetRotation()
	{
		transform.rotation = rotation;
	}


}