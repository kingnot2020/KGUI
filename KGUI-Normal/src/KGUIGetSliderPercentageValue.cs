﻿
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace KGUI
{
	public class KGUIGetSliderPercentageValue : MonoBehaviour {

		public Slider  sliderObject;
		private Text textObject;
		
		// Use this for initialization
		void Awake () {
			textObject = this.gameObject.GetComponent<Text>();
		}
		
		// Update is called once per frame
		void Update () {
			textObject.text = ((int)(sliderObject.value*100.0)).ToString()+"%";
		}
	}
}