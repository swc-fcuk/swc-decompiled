using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ClearButtonCircleStoryAction : AbstractStoryAction
	{
		public ClearButtonCircleStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(0);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<UXController>().MiscElementsManager.HideHighlight();
			NavigationCenterUpgradeScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<NavigationCenterUpgradeScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.DeactivateHighlight();
			}
			this.parent.ChildComplete(this);
		}

		protected internal ClearButtonCircleStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ClearButtonCircleStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ClearButtonCircleStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
