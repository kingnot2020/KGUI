using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace KGUI
{
	[AddComponentMenu("KGUI/Interaction/Toggled Components")]
	public class KGUIToggledComponents : MonoBehaviour
	{
		public List<MonoBehaviour> activate;
		public List<MonoBehaviour> deactivate;

		// Deprecated functionality
		[HideInInspector][SerializeField] MonoBehaviour target;
		[HideInInspector][SerializeField] bool inverse = false;

		void Awake ()
		{
			// Legacy functionality -- auto-upgrade
			if (target != null)
			{
				if (activate.Count == 0 && deactivate.Count == 0)
				{
					if (inverse) deactivate.Add(target);
					else activate.Add(target);
				}
				else target = null;

				//#if UNITY_EDITOR
				//NGUITools.SetDirty(this);
				//#endif
			}

			#if UNITY_EDITOR
			if (!Application.isPlaying) return;
			#endif
			Toggle toggle = GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(Toggle);
		}

		public void Toggle (bool check)
		{
			if (enabled)
			{
				for (int i = 0; i < activate.Count; ++i)
				{
					MonoBehaviour comp = activate[i];
					comp.enabled = check;
				}

				for (int i = 0; i < deactivate.Count; ++i)
				{
					MonoBehaviour comp = deactivate[i];
					comp.enabled = !check;
				}
			}
		}
	}
}

