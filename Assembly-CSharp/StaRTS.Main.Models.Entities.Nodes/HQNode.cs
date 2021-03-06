using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class HQNode : Node<HQNode>
	{
		public HQComponent HQComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public HQNode()
		{
		}

		protected internal HQNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HQNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HQNode)GCHandledObjects.GCHandleToObject(instance)).HQComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HQNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((HQNode)GCHandledObjects.GCHandleToObject(instance)).HQComp = (HQComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
