using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;

namespace StaRTS.Main.Controllers.World.Transitions
{
	public class WarboardToBaseTransition : AbstractTransition
	{
		private const float WIPE_DIRECTION = 1.57079637f;

		public WarboardToBaseTransition(IState transitionToState, IMapDataLoader mapDataLoader, TransitionCompleteDelegate onTransitionComplete) : base(transitionToState, mapDataLoader, onTransitionComplete, false, false, WipeTransition.FromWarboardToBase, WipeTransition.FromWarboardToBase)
		{
		}

		public override void StartTransition()
		{
			Service.Get<UserInputInhibitor>().DenyAll();
			this.transitioning = true;
			this.worldLoaded = false;
			this.preloadsLoaded = false;
			this.wipeDirection = 1.57079637f;
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.SendEvent(EventId.HideAllHolograms, null);
			eventManager.RegisterObserver(this, EventId.WarBoardDestroyed);
			eventManager.RegisterObserver(this, EventId.WipeCameraSnapshotTaken);
			Service.Get<CameraManager>().WipeCamera.TakeSnapshotForDelayedWipe(this.startWipeTransition, this.wipeDirection, new WipeCompleteDelegate(this.OnTransitionInComplete), null);
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.WipeCameraSnapshotTaken)
			{
				if (id == EventId.WarBoardDestroyed)
				{
					Service.Get<EventManager>().UnregisterObserver(this, EventId.WarBoardDestroyed);
					Service.Get<CameraManager>().WipeCamera.ContinueWipe();
				}
			}
			else
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.WipeCameraSnapshotTaken);
				if (this.transitionToState != null)
				{
					Service.Get<GameStateMachine>().SetState(this.transitionToState);
				}
			}
			return EatResponse.NotEaten;
		}

		protected override void OnTransitionInComplete(object cookie)
		{
			Service.Get<UserInputInhibitor>().AllowAll();
			base.OnTransitionInComplete(cookie);
		}
	}
}
