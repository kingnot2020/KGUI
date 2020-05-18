using UnityEngine;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace KGUI
{
	public class KGUIUtil
	{
		/// <summary>
		/// Convert a decimal value to its hex representation.
		/// </summary>

		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		static public string DecimalToHex8 (int num)
		{
			num &= 0xFF;
			#if UNITY_FLASH
			StringBuilder sb = new StringBuilder();
			sb.Append(DecimalToHexChar((num >> 4) & 0xF));
			sb.Append(DecimalToHexChar(num & 0xF));
			return sb.ToString();
			#else
			return num.ToString("X2");
			#endif
		}

		/// <summary>
		/// The reverse of ParseAlpha -- encodes a color in Aa format.
		/// </summary>

		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		static public string EncodeAlpha (float a)
		{
			int i = Mathf.Clamp(Mathf.RoundToInt(a * 255f), 0, 255);
			return DecimalToHex8(i);
		}

		/// <summary>
		/// Parse an embedded symbol, such as <color=#FFAA00> (set color) or </color> (undo color change). Returns whether the index was adjusted.
		/// </summary>

		static public bool ParseSymbol (string text, ref int index)
		{
			string[] symbols = new string[]{ "b", "i", "size", "color" };
			bool isSymbol = false;
			int symbolStart = index;
			string subText = text.Substring (index);
			for (int i = 0; i < symbols.Length; i++) {
				if (subText.StartsWith ("<" + symbols [i]) 
					|| 
					subText.StartsWith ("</" + symbols [i])) {
					isSymbol = true;
					symbolStart =  i;
					break;
				}
			}
			if (!isSymbol)
				return false;
			int symbolEnd = subText.IndexOf ('>');
			if (symbolEnd < 0)
				return false;

			index += symbolEnd - symbolStart;

			return true;
		}
	}
}

