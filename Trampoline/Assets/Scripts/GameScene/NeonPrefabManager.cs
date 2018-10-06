using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonPrefabManager : MonoBehaviour {

	public float ratio;

	private float minSize = 0.5f;
	private float maxSize = 1.0f;
	private float sizeLen;
	private float colorLen = 360;
	private float iniS = 78;
	private float iniV = 100;

	// Use this for initialization
	void Start () {
		sizeLen = maxSize - minSize;

		ParticleSystem.MainModule par = GetComponent<ParticleSystem>().main;
    par.startColor = UnityEngine.Color.HSVToRGB(colorLen*ratio/colorLen,iniS/100,iniV/100);
		par.startSize = minSize + sizeLen * ratio;

	}

	// Update is called once per frame
	void Update () {
		ratio += Time.deltaTime;
		while(ratio >= 1.0f){
			ratio -= 1.0f;
		}

		ParticleSystem.MainModule par = GetComponent<ParticleSystem>().main;
    par.startColor = UnityEngine.Color.HSVToRGB(colorLen*ratio/colorLen,iniS/100,iniV/100);
		par.startSize = minSize + sizeLen * ratio;
	}
}
