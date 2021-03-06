using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.TargetedBundleOffers
{
	public class GetTargetedOffersResponse : AbstractResponse
	{
		public TargetedOfferSummary TargetedOffers;

		public List<TargetedBundleVO> TargetedBundleVOs
		{
			get;
			private set;
		}

		public GetTargetedOffersResponse()
		{
		}

		public override ISerializable FromObject(object obj)
		{
			this.TargetedOffers = new TargetedOfferSummary();
			this.TargetedOffers.FromObject(obj);
			return this;
		}

		protected internal GetTargetedOffersResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetTargetedOffersResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetTargetedOffersResponse)GCHandledObjects.GCHandleToObject(instance)).TargetedBundleVOs);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GetTargetedOffersResponse)GCHandledObjects.GCHandleToObject(instance)).TargetedBundleVOs = (List<TargetedBundleVO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
