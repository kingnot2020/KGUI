
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace KGUI
{
	public class KGUIGetScrollPercentageValue : MonoBehaviour {
		
		public Scrollbar scrollBarObject;
		private Text textObject;

		// Use this for initialization
		void Awake () {
			textObject = this.gameObject.GetComponent<Text>();
		}
		
		// Update is called once per frame
		void Update () {
			textObject.text = ((int)(scrollBarObject.value*100.0)).ToString()+"%";
		}
	}
}
