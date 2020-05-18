
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

namespace KGUI
{
	[RequireComponent(typeof(Text))]
	//
	public class KGUITypewriterEffect : MonoBehaviour
	{
		static public KGUITypewriterEffect Instance;

		struct fadeData
		{
			public int index;
			public string text;
			public float alpha;
		}

		public int charsPerSecond = 20;
		public float fadeInTime = 0f;
		public float delayOnPeriod = 0f;
		public float delayOnNewLine = 0f;

		public bool keepFullDimensions = false;
		//public List<EventDelegate> onFinished = new List<EventDelegate>();

		private Text mLabel;
		private string mFullText = "";
		private int mCurrentOffset = 0;
		private float mNextChar = 0f;
		private bool mReset = true;
		private bool mActive = false;

		KGUIBetterList<fadeData> mFade = new KGUIBetterList<fadeData>();	
		public bool isActive { get { return mActive; } }

		public void ResetToBeginning ()
		{
			Finish();
			mReset = true;
			mActive = true;
			mNextChar = 0f;
			mCurrentOffset = 0;
			Update();
		}

		public void Finish ()
		{
			if (mActive)
			{
				mActive = false;

				if (!mReset)
				{
					mCurrentOffset = mFullText.Length;
					mFade.Clear();
					mLabel.text = mFullText;
				}

				Instance = this;
				//EventDelegate.Execute(onFinished);
				Instance = null;
			}
		}

		void OnEnable () { mReset = true; mActive = true; }

		void Update ()
		{
			if (!mActive) return;

			if (mReset)
			{
				mCurrentOffset = 0;
				mReset = false;
				mLabel = GetComponent<Text>();
				mFullText = mLabel.text;
				mFade.Clear();
			}

			while (mCurrentOffset < mFullText.Length && mNextChar <= Time.unscaledTime)
			{
				int lastOffset = mCurrentOffset;
				charsPerSecond = Mathf.Max(1, charsPerSecond);

				// FIXME: 对于Rich Text的Text，仅仅Skip符号是不行的，还需要找到对应的结束符号，并把结束符号给添加到尾部，而如果有嵌套存在，还需要把嵌套的都添加到尾部
				while (KGUIUtil.ParseSymbol(mFullText, ref mCurrentOffset)) { }
				++mCurrentOffset;

				if (mCurrentOffset > mFullText.Length) break;

				float delay = 1f / charsPerSecond;
				char c = (lastOffset < mFullText.Length) ? mFullText[lastOffset] : '\n';

				if (c == '\n')
				{
					delay += delayOnNewLine;
				}
				else if (lastOffset + 1 == mFullText.Length || mFullText[lastOffset + 1] <= ' ')
				{
					if (c == '.')
					{
						if (lastOffset + 2 < mFullText.Length && mFullText[lastOffset + 1] == '.' && mFullText[lastOffset + 2] == '.')
						{
							delay += delayOnPeriod * 3f;
							lastOffset += 2;
						}
						else delay += delayOnPeriod;
					}
					else if (c == '!' || c == '?')
					{
						delay += delayOnPeriod;
					}
				}

				if (mNextChar == 0f)
				{
					mNextChar = Time.unscaledTime + delay;
				}
				else mNextChar += delay;

				if (fadeInTime != 0f)
				{
					fadeData tempFade = new fadeData();
					tempFade.index = lastOffset;
					tempFade.alpha = 0f;
					tempFade.text = mFullText.Substring(lastOffset, mCurrentOffset - lastOffset);
					mFade.Add(tempFade);
				}
				else
				{
					mLabel.text = keepFullDimensions ?
						mFullText.Substring(0, mCurrentOffset) + "<color=#00000000>" + mFullText.Substring(mCurrentOffset) + "</color>" :
						mFullText.Substring(0, mCurrentOffset);
				}
			}

			if (mFade.size != 0)
			{
				for (int i = 0; i < mFade.size; )
				{
					fadeData tempFade = mFade[i];
					tempFade.alpha += Time.unscaledDeltaTime / fadeInTime;
					
					if (tempFade.alpha < 1f)
					{
						mFade[i] = tempFade;
						++i;
					}
					else mFade.RemoveAt(i);
				}

				if (mFade.size == 0)
				{
					if (keepFullDimensions) mLabel.text = mFullText.Substring(0, mCurrentOffset) + "<color=#00000000>" + mFullText.Substring(mCurrentOffset) + "</color>";
					else mLabel.text = mFullText.Substring(0, mCurrentOffset);
				}
				else
				{
					StringBuilder sb = new StringBuilder();

					string color = ColorUtility.ToHtmlStringRGB(mLabel.color);
					for (int i = 0; i < mFade.size; ++i)
					{
						fadeData tempFade = mFade[i];

						if (i == 0)
						{
							sb.Append(mFullText.Substring(0, tempFade.index));
						}

						sb.Append("<color=#"+color);
						sb.Append(KGUIUtil.EncodeAlpha(tempFade.alpha));
						sb.Append(">");
						sb.Append(tempFade.text);
						sb.Append("</color>");
					}

					if (keepFullDimensions)
					{
						sb.Append("<color=#00000000>");
						sb.Append(mFullText.Substring(mCurrentOffset));
						sb.Append("</color>");
					}

					mLabel.text = sb.ToString();
				}
			}
			else if (mCurrentOffset == mFullText.Length)
			{
				Instance = this;
				//EventDelegate.Execute(onFinished);
				Instance = null;
				mActive = false;
			}
		}
	}
}
