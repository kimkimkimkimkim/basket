using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagText : MonoBehaviour {

	public GameObject wall;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Text>().text = wall.gameObject.tag.ToString();
	}

	// Update is called once per frame
	void Update () {

	}
}
