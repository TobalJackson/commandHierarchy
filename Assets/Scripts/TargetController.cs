using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetController : MonoBehaviour {
	int hp;
	Slider hpbar;
	public Image Fill;
	Color MaxHealthColor = Color.green;
	Color MinHealthColor = Color.red;
	public bool isAlive;

	// Use this for initialization
	void Start () {
		hp = 100;
		isAlive = true;
		hpbar = GameObject.FindGameObjectWithTag("HPBar").GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		hpbar.value = hp;
		Fill.color = Color.Lerp (MinHealthColor, MaxHealthColor, (float)this.hp / 100);
		if (this.hp < 0) {
			//DestroyImmediate(this.gameObject);
			isAlive = false;
			this.gameObject.SetActive(false);
		}
	}

	public void takeDamage(int damage){
		this.hp -= damage;
	}
	public bool isDead(){
		if (!isAlive) return true;
		else return false;
	}
}
