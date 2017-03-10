using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class SquadMemberResponse : AbstractResponse
	{
		public object SquadMemberData
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			if (!(obj is Dictionary<string, object>))
			{
				return this;
			}
			this.SquadMemberData = obj;
			return this;
		}

		public SquadMemberResponse()
		{
		}

		protected internal SquadMemberResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberResponse)GCHandledObjects.GCHandleToObject(instance)).SquadMemberData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadMemberResponse)GCHandledObjects.GCHandleToObject(instance)).SquadMemberData = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
