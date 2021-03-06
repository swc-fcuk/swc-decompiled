using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.GameBoard;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class SpatialIndexController
	{
		private SpatialIndex[,] spatialIndices;

		private int width;

		private int depth;

		private int maxSquaredDistance;

		public SpatialIndexController()
		{
			Service.Set<SpatialIndexController>(this);
			this.Initialize();
		}

		private void Initialize()
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			this.width = board.BoardSize;
			this.depth = board.BoardSize;
			this.maxSquaredDistance = board.GetMaxSquaredDistance();
			this.spatialIndices = new SpatialIndex[this.width, this.depth];
			this.Reset();
		}

		public void Reset()
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.depth; j++)
				{
					this.spatialIndices[i, j] = null;
				}
			}
		}

		public void ResetTurretScannedFlagForBoard()
		{
			for (int i = 0; i < this.width; i++)
			{
				for (int j = 0; j < this.depth; j++)
				{
					if (this.spatialIndices[i, j] != null)
					{
						this.spatialIndices[i, j].ResetTurretScanedFlag();
					}
				}
			}
		}

		private void SetBuildingsToAttack(int x, int z, SpatialIndex spatialIndex)
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			BoardCell<Entity> cellAt = board.GetCellAt(x, z, true);
			NodeList<BuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<BuildingNode>();
			spatialIndex.AlreadyScannedBuildingsToAttack = true;
			for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
			{
				SmartEntity smartEntity = (SmartEntity)buildingNode.Entity;
				if (smartEntity.DamageableComp != null && this.IsAliveHealthNode(smartEntity))
				{
					int squaredDistance = this.CalcSquredDistanceFromTransformToCell(smartEntity.TransformComp, cellAt);
					int nearness = this.CalcNearness(squaredDistance);
					spatialIndex.AddBuildingsToAttack(smartEntity, nearness);
				}
			}
		}

		private void SetTurretsInRangeOf(int x, int z, SpatialIndex spatialIndex)
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			BoardCell<Entity> cellAt = board.GetCellAt(x, z, true);
			NodeList<TurretNode> nodeList = Service.Get<EntityController>().GetNodeList<TurretNode>();
			spatialIndex.AlreadyScannedTurretsInRange = true;
			for (TurretNode turretNode = nodeList.Head; turretNode != null; turretNode = turretNode.Next)
			{
				SmartEntity smartEntity = (SmartEntity)turretNode.Entity;
				if (this.IsAliveHealthNode(smartEntity))
				{
					TransformComponent transformComp = smartEntity.TransformComp;
					int num = this.CalcSquredDistanceFromTransformToCell(transformComp, cellAt);
					int nearness = this.CalcNearness(num);
					spatialIndex.AddTurretsInRangeOf(smartEntity, num, nearness);
				}
			}
		}

		private void SetAreaTriggerBuildingsInRangeOf(int x, int z, SpatialIndex spatialIndex)
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			BoardCell<Entity> cellAt = board.GetCellAt(x, z, true);
			spatialIndex.AlreadyScannedAreaTriggerBuildingsInRange = true;
			NodeList<AreaTriggerBuildingNode> nodeList = Service.Get<EntityController>().GetNodeList<AreaTriggerBuildingNode>();
			for (AreaTriggerBuildingNode areaTriggerBuildingNode = nodeList.Head; areaTriggerBuildingNode != null; areaTriggerBuildingNode = areaTriggerBuildingNode.Next)
			{
				Entity entity = areaTriggerBuildingNode.Entity;
				int num = this.CalcSquredDistanceFromTransformToCell(areaTriggerBuildingNode.TransformComp, cellAt);
				int nearness = this.CalcNearness(num);
				spatialIndex.AddAreaTriggerBuildingsInRangeOf(entity, num, nearness);
			}
		}

		private SpatialIndex EnsureSpatialIndex(int i, int j)
		{
			SpatialIndex spatialIndex = this.spatialIndices[i, j];
			if (spatialIndex == null)
			{
				spatialIndex = new SpatialIndex();
				this.spatialIndices[i, j] = spatialIndex;
			}
			return spatialIndex;
		}

		public PriorityList<SmartEntity> GetBuildingsToAttack(int x, int z)
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			board.MakeCoordinatesAbsolute(ref x, ref z);
			if (this.IsPositionInvalid(x, z))
			{
				return null;
			}
			SpatialIndex spatialIndex = this.EnsureSpatialIndex(x, z);
			if (!spatialIndex.AlreadyScannedBuildingsToAttack)
			{
				this.SetBuildingsToAttack(x, z, spatialIndex);
			}
			return spatialIndex.GetBuildingsToAttack();
		}

		public List<ElementPriorityPair<Entity>> GetTurretsInRangeOf(int x, int z)
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			board.MakeCoordinatesAbsolute(ref x, ref z);
			if (this.IsPositionInvalid(x, z))
			{
				return null;
			}
			SpatialIndex spatialIndex = this.EnsureSpatialIndex(x, z);
			if (!spatialIndex.AlreadyScannedTurretsInRange)
			{
				this.SetTurretsInRangeOf(x, z, spatialIndex);
			}
			return spatialIndex.GetTurretsInRangeOf();
		}

		public List<ElementPriorityPair<Entity>> GetAreaTriggerBuildingsInRangeOf(int x, int z)
		{
			Board<Entity> board = Service.Get<BoardController>().Board;
			board.MakeCoordinatesAbsolute(ref x, ref z);
			if (this.IsPositionInvalid(x, z))
			{
				return null;
			}
			SpatialIndex spatialIndex = this.EnsureSpatialIndex(x, z);
			if (!spatialIndex.AlreadyScannedAreaTriggerBuildingsInRange)
			{
				this.SetAreaTriggerBuildingsInRangeOf(x, z, spatialIndex);
			}
			return spatialIndex.GetArareaTriggerBuildingsInRange();
		}

		public bool IsPositionInvalid(int x, int z)
		{
			return x < 0 || x >= this.width || z < 0 || z >= this.depth;
		}

		private bool IsAliveHealthNode(SmartEntity entity)
		{
			return entity.HealthComp != null && !entity.HealthComp.IsDead();
		}

		private int CalcSquredDistanceFromTransformToCell(TransformComponent transform, BoardCell<Entity> cell)
		{
			return GameUtils.SquaredDistance(transform.CenterGridX(), transform.CenterGridZ(), cell.X, cell.Z);
		}

		public int CalcNearness(int squaredDistance)
		{
			return (this.maxSquaredDistance - squaredDistance) * 10000 / this.maxSquaredDistance;
		}

		protected internal SpatialIndexController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).CalcNearness(*(int*)args));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).CalcSquredDistanceFromTransformToCell((TransformComponent)GCHandledObjects.GCHandleToObject(*args), (BoardCell<Entity>)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).EnsureSpatialIndex(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).GetAreaTriggerBuildingsInRangeOf(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).GetBuildingsToAttack(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).GetTurretsInRangeOf(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).Initialize();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).IsAliveHealthNode((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).IsPositionInvalid(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).ResetTurretScannedFlagForBoard();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).SetAreaTriggerBuildingsInRangeOf(*(int*)args, *(int*)(args + 1), (SpatialIndex)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).SetBuildingsToAttack(*(int*)args, *(int*)(args + 1), (SpatialIndex)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SpatialIndexController)GCHandledObjects.GCHandleToObject(instance)).SetTurretsInRangeOf(*(int*)args, *(int*)(args + 1), (SpatialIndex)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}
	}
}
