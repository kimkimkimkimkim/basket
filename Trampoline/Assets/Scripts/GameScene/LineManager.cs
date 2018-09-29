using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

	private float interval = 0.2f;
	private float blinkStartTime = 7;
	private float destroyTime = 10;

	// Use this for initialization
	void Start () {
		StartCoroutine("Blink");
		Invoke("DestroyLine",destroyTime);
	}

	IEnumerator Blink(){
		yield return new WaitForSeconds(blinkStartTime);

		while ( true ) {
      var renderComponent = GetComponent<Renderer>();
      renderComponent.enabled = !renderComponent.enabled;
      yield return new WaitForSeconds(interval);
    }
	}

	private void DestroyLine(){
		Destroy(this.gameObject);
	}

	// Update is called once per frame
	void Update () {

	}
}
