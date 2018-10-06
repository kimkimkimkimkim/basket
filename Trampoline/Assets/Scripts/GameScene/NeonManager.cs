using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonManager : MonoBehaviour {

	public GameObject neonPrefab;
	public Vector3[] basePos = new Vector3[2];
	public float hNum;

	private float wNum;
	private float hDis;
	private float wDis;
	private float loopNum = 5;
	private float loopTime = 1;
	private float totalCount = 0;

	// Use this for initialization
	void Start () {
		hDis = (basePos[1].y - basePos[0].y)/(hNum - 1);
		wNum = (int)(((basePos[1].x - basePos[0].x)/hDis) + 1);
		wDis = (basePos[1].x - basePos[0].x)/(wNum - 1);

		CreateNeon();
	}

	private void CreateNeon(){
		//左縦
		for(int i=0;i<hNum;i++){
			totalCount++;
			GameObject neon = (GameObject)Instantiate(neonPrefab);
			neon.transform.SetParent(this.gameObject.transform);
			neon.transform.localPosition =
				new Vector3(basePos[0].x,basePos[0].y + (hDis*i),0);
			neon.GetComponent<NeonPrefabManager>().ratio = (totalCount % loopNum) / loopNum;
		}

		//上
		for(int i=0;i<wNum-1;i++){
			totalCount++;
			GameObject neon = (GameObject)Instantiate(neonPrefab);
			neon.transform.SetParent(this.gameObject.transform);
			neon.transform.localPosition =
				new Vector3(basePos[0].x + (wDis*(i+1)),basePos[1].y,0);
			neon.GetComponent<NeonPrefabManager>().ratio = (totalCount % loopNum) / loopNum;
		}

		//右縦
		for(int i=0;i<hNum-1;i++){
			totalCount++;
			GameObject neon = (GameObject)Instantiate(neonPrefab);
			neon.transform.SetParent(this.gameObject.transform);
			neon.transform.localPosition =
				new Vector3(basePos[1].x,basePos[1].y - (hDis*(i+1)),0);
			neon.GetComponent<NeonPrefabManager>().ratio = (totalCount % loopNum) / loopNum;
		}

	}
}
