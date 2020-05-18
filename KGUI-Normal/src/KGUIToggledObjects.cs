using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace KGUI
{
	[AddComponentMenu("KGUI/Interaction/Toggled Objects")]
	public class KGUIToggledObjects : MonoBehaviour
	{
		public List<GameObject> activate;
		public List<GameObject> deactivate;

		private Toggle _toggle;

		void Awake ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying) return;
			#endif
			_toggle = GetComponent<Toggle>();
			_toggle.onValueChanged.AddListener(Toggle);
		}

		public void Toggle (bool check)
		{
			if (enabled)
			{
				for (int i = 0; i < activate.Count; ++i)
					Set(activate[i], check);

				for (int i = 0; i < deactivate.Count; ++i)
					Set(deactivate[i], !check);
			}
		}

		void Set (GameObject go, bool state)
		{
			if (go != null)
			{
				go.SetActive(state);
			}
		}
	}
}
