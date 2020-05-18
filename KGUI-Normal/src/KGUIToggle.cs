
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace KGUI
{
	public class KGUIToggle : MonoBehaviour {
		private Toggle _toggle;

		public int Group;
		public bool StateOfNone;
		public bool startingState;

		public Sprite m_Sprite;
		public Animation m_Animation;

		public Toggle Toggle {
			get { return _toggle; }
			set {
				if(value==null && _toggle!=null)
					_toggle.onValueChanged.RemoveListener(OnValueChanged);
				_toggle = value;
				_toggle.onValueChanged.AddListener(OnValueChanged);
			}
		}


		void OnValueChanged(bool check)
		{
			Debug.Log(check);
		}
	}
}
