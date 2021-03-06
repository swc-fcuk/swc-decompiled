using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class ScoutTowerNode : Node<ScoutTowerNode>
	{
		public ScoutTowerComponent ScoutTowerComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public ScoutTowerNode()
		{
		}

		protected internal ScoutTowerNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScoutTowerNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScoutTowerNode)GCHandledObjects.GCHandleToObject(instance)).ScoutTowerComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ScoutTowerNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ScoutTowerNode)GCHandledObjects.GCHandleToObject(instance)).ScoutTowerComp = (ScoutTowerComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
