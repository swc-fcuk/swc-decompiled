using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX
{
	public class LabelFader : IViewFrameTimeObserver
	{
		private float remainingTime;

		private UXLabel label;

		private UXFactory uxFactory;

		private float fadeTime;

		private LabelFaderCompleteDelegate onComplete;

		private UXElement objectToDestroy;

		private Color tempColor;

		public int LineCount
		{
			get;
			private set;
		}

		public UXLabel Label
		{
			get
			{
				return this.label;
			}
		}

		public LabelFader(UXLabel label, UXFactory uxFactory, float showTime, float fadeTime, LabelFaderCompleteDelegate onComplete, int lineCount, UXElement objectToDestroy)
		{
			this.remainingTime = showTime + fadeTime;
			this.label = label;
			this.uxFactory = uxFactory;
			this.fadeTime = fadeTime;
			this.onComplete = onComplete;
			this.objectToDestroy = objectToDestroy;
			this.LineCount = lineCount;
			if (fadeTime > 0f || showTime > 0f)
			{
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			}
		}

		public void OnViewFrameTime(float dt)
		{
			this.remainingTime -= dt;
			if (this.remainingTime <= 0f || this.label == null)
			{
				this.Destroy();
				return;
			}
			if (this.remainingTime < this.fadeTime)
			{
				this.tempColor = this.label.TextColor;
				this.tempColor.a = this.remainingTime / this.fadeTime;
				this.label.TextColor = this.tempColor;
			}
		}

		public void MoveUp(int linesToMove)
		{
			Vector3 localPosition = this.label.LocalPosition;
			if (localPosition.y > (float)Screen.height * 0.5f)
			{
				this.Destroy();
				return;
			}
			localPosition.y += this.label.LineHeight * (float)linesToMove;
			this.label.LocalPosition = localPosition;
		}

		public void Destroy()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			if (this.uxFactory != null)
			{
				this.uxFactory.DestroyElement(this.objectToDestroy);
			}
			if (this.onComplete != null)
			{
				this.onComplete(this);
			}
			this.onComplete = null;
			this.objectToDestroy = null;
			this.uxFactory = null;
			this.label = null;
		}

		protected internal LabelFader(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((LabelFader)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LabelFader)GCHandledObjects.GCHandleToObject(instance)).Label);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LabelFader)GCHandledObjects.GCHandleToObject(instance)).LineCount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LabelFader)GCHandledObjects.GCHandleToObject(instance)).MoveUp(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LabelFader)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LabelFader)GCHandledObjects.GCHandleToObject(instance)).LineCount = *(int*)args;
			return -1L;
		}
	}
}
