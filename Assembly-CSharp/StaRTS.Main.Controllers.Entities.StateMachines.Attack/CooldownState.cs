using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.StateMachines.Attack
{
	public class CooldownState : AttackFSMState
	{
		public CooldownState(AttackFSM owner) : base(owner, owner.ShooterComp.ShooterVO.CooldownDelay)
		{
		}

		public override void OnEnter()
		{
			base.OnEnter();
			base.AttackFSMOwner.StateComponent.CurState = EntityState.CoolingDown;
		}

		public override void OnExit(IState nextState)
		{
			if (base.AttackFSMOwner.ShooterComp.ShooterVO.ClipRetargeting)
			{
				Service.Get<TargetingController>().ReevaluateTarget(base.AttackFSMOwner.ShooterComp);
			}
			base.OnExit(nextState);
			Service.Get<ShooterController>().OnCooldownExit(base.ShooterComp);
		}

		protected internal CooldownState(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CooldownState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CooldownState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
