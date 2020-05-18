
using UnityEngine;
using System.Collections;

namespace KGUI
{
	public class KGUIListItem : MonoBehaviour {

		public GameObject thePopupController;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void SelectThisItem(){
			thePopupController.gameObject.SendMessage("SetSelection", this.name);
		}
	}
}
