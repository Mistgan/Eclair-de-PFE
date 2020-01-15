using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public GameObject mainMenu, weaponsMenu, background;

	private Animator mainMenuAnim,weaponsMenuAnim,backgroundAnim;
	private bool isMainMenuOnLeft;

	// Use this for initialization
	void Start () {
		mainMenuAnim = mainMenu.GetComponent<Animator>();
		weaponsMenuAnim = weaponsMenu.GetComponent<Animator>();
		backgroundAnim = background.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// fonction des boutons du menu

	public void OnWeaponsButtonClick(){
		print("OnWeaponsButtonClick");
		if(isMainMenuOnLeft==false){
			mainMenuAnim.SetBool("isMovingLeft",true);
			weaponsMenuAnim.SetBool("isMovingIn",true);
			backgroundAnim.SetBool("isTurning",true);
			isMainMenuOnLeft =true;
		}else{
			mainMenuAnim.SetBool("isMovingLeft",false);
			weaponsMenuAnim.SetBool("isMovingIn",false);
			backgroundAnim.SetBool("isTurning",false);
			isMainMenuOnLeft =false;
		}
	}
}