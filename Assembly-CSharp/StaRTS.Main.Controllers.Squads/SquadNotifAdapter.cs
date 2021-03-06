using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Commands.Squads;
using StaRTS.Main.Models.Commands.Squads.Requests;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Squads
{
	public class SquadNotifAdapter : AbstractSquadServerAdapter
	{
		private GetSquadNotifsRequest request;

		public SquadNotifAdapter()
		{
			this.request = new GetSquadNotifsRequest();
			this.request.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.request.Since = 0u;
			this.request.BattleVersion = "21.0";
		}

		public void SetNotifStartDate(uint notifStartDate)
		{
			this.request.Since = notifStartDate;
		}

		protected override void Poll()
		{
			GetSquadNotifsCommand getSquadNotifsCommand = new GetSquadNotifsCommand(this.request);
			getSquadNotifsCommand.AddSuccessCallback(new AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>.OnSuccessCallback(this.OnGetNotifs));
			Service.Get<ServerAPI>().Sync(getSquadNotifsCommand);
		}

		private void OnGetNotifs(SquadNotifsResponse response, object cookie)
		{
			base.OnPollFinished(response);
		}

		protected override void PopulateSquadMsgsReceived(object response)
		{
			SquadNotifsResponse squadNotifsResponse = (SquadNotifsResponse)response;
			List<object> notifs = squadNotifsResponse.Notifs;
			if (notifs != null)
			{
				int i = 0;
				int count = notifs.Count;
				while (i < count)
				{
					SquadMsg squadMsg = SquadMsgUtils.GenerateMessageFromNotifObject(notifs[i]);
					if (squadMsg != null)
					{
						this.list.Add(squadMsg);
						if (squadMsg.TimeSent > this.request.Since)
						{
							this.request.Since = squadMsg.TimeSent;
						}
					}
					i++;
				}
			}
		}

		public void ResetPollTimer(uint since)
		{
			if (since > this.request.Since)
			{
				this.request.Since = since;
			}
			base.ResetPollTimer();
		}

		protected internal SquadNotifAdapter(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadNotifAdapter)GCHandledObjects.GCHandleToObject(instance)).OnGetNotifs((SquadNotifsResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadNotifAdapter)GCHandledObjects.GCHandleToObject(instance)).Poll();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadNotifAdapter)GCHandledObjects.GCHandleToObject(instance)).PopulateSquadMsgsReceived(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
