using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Static;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers
{
	public class BuildingLookupController
	{
		private EntityController entityController;

		public NodeList<HQNode> HQNodeList
		{
			get;
			private set;
		}

		public NodeList<BarracksNode> BarracksNodeList
		{
			get;
			private set;
		}

		public NodeList<ArmoryNode> ArmoryNodeList
		{
			get;
			private set;
		}

		public NodeList<FactoryNode> FactoryNodeList
		{
			get;
			private set;
		}

		public NodeList<CantinaNode> CantinaNodeList
		{
			get;
			private set;
		}

		public NodeList<FleetCommandNode> FleetCommandNodeList
		{
			get;
			private set;
		}

		public NodeList<TacticalCommandNode> TacticalCommandNodeList
		{
			get;
			private set;
		}

		public NodeList<ChampionPlatformNode> ChampionPlatformNodeList
		{
			get;
			private set;
		}

		public NodeList<StarportNode> StarportNodeList
		{
			get;
			private set;
		}

		public NodeList<DroidHutNode> DroidHutNodeList
		{
			get;
			private set;
		}

		public NodeList<SquadBuildingNode> SquadBuildingNodeList
		{
			get;
			private set;
		}

		public NodeList<NavigationCenterNode> NavigationCenterNodeList
		{
			get;
			private set;
		}

		public NodeList<ScoutTowerNode> ScoutTowerNodeList
		{
			get;
			private set;
		}

		public NodeList<WallNode> WallNodeList
		{
			get;
			private set;
		}

		public NodeList<TurretBuildingNode> TurretBuildingNodeList
		{
			get;
			private set;
		}

		public NodeList<OffenseLabNode> OffenseLabNodeList
		{
			get;
			private set;
		}

		public NodeList<DefenseLabNode> DefenseLabNodeList
		{
			get;
			private set;
		}

		public NodeList<GeneratorNode> GeneratorNodeList
		{
			get;
			private set;
		}

		public NodeList<StorageNode> StorageNodeList
		{
			get;
			private set;
		}

		public NodeList<GeneratorViewNode> GeneratorViewNodeList
		{
			get;
			private set;
		}

		public NodeList<ShieldGeneratorNode> ShieldGeneratorNodeList
		{
			get;
			private set;
		}

		public NodeList<ClearableNode> ClearableNodeList
		{
			get;
			private set;
		}

		public NodeList<TrapNode> TrapNodeList
		{
			get;
			private set;
		}

		public NodeList<TrapBuildingNode> TrapBuildingNodeList
		{
			get;
			private set;
		}

		public NodeList<HousingNode> HousingNodeList
		{
			get;
			private set;
		}

		public BuildingLookupController()
		{
			Service.Set<BuildingLookupController>(this);
			this.entityController = Service.Get<EntityController>();
			this.HQNodeList = this.entityController.GetNodeList<HQNode>();
			this.BarracksNodeList = this.entityController.GetNodeList<BarracksNode>();
			this.ArmoryNodeList = this.entityController.GetNodeList<ArmoryNode>();
			this.FactoryNodeList = this.entityController.GetNodeList<FactoryNode>();
			this.CantinaNodeList = this.entityController.GetNodeList<CantinaNode>();
			this.FleetCommandNodeList = this.entityController.GetNodeList<FleetCommandNode>();
			this.TacticalCommandNodeList = this.entityController.GetNodeList<TacticalCommandNode>();
			this.ChampionPlatformNodeList = this.entityController.GetNodeList<ChampionPlatformNode>();
			this.StarportNodeList = this.entityController.GetNodeList<StarportNode>();
			this.DroidHutNodeList = this.entityController.GetNodeList<DroidHutNode>();
			this.SquadBuildingNodeList = this.entityController.GetNodeList<SquadBuildingNode>();
			this.NavigationCenterNodeList = this.entityController.GetNodeList<NavigationCenterNode>();
			this.ScoutTowerNodeList = this.entityController.GetNodeList<ScoutTowerNode>();
			this.WallNodeList = this.entityController.GetNodeList<WallNode>();
			this.TurretBuildingNodeList = this.entityController.GetNodeList<TurretBuildingNode>();
			this.OffenseLabNodeList = this.entityController.GetNodeList<OffenseLabNode>();
			this.DefenseLabNodeList = this.entityController.GetNodeList<DefenseLabNode>();
			this.GeneratorNodeList = this.entityController.GetNodeList<GeneratorNode>();
			this.StorageNodeList = this.entityController.GetNodeList<StorageNode>();
			this.GeneratorViewNodeList = this.entityController.GetNodeList<GeneratorViewNode>();
			this.ShieldGeneratorNodeList = this.entityController.GetNodeList<ShieldGeneratorNode>();
			this.ClearableNodeList = this.entityController.GetNodeList<ClearableNode>();
			this.TrapNodeList = this.entityController.GetNodeList<TrapNode>();
			this.TrapBuildingNodeList = this.entityController.GetNodeList<TrapBuildingNode>();
			this.HousingNodeList = this.entityController.GetNodeList<HousingNode>();
		}

		public int GetBuildingPurchasedQuantity(BuildingTypeVO building)
		{
			switch (building.Type)
			{
			case BuildingType.Invalid:
				Service.Get<Logger>().Warn("Invalid building type for count: " + building.Uid);
				return 0;
			case BuildingType.HQ:
				return this.HQNodeList.CalculateCount();
			case BuildingType.Barracks:
				return this.BarracksNodeList.CalculateCount();
			case BuildingType.Factory:
				return this.FactoryNodeList.CalculateCount();
			case BuildingType.FleetCommand:
				return this.FleetCommandNodeList.CalculateCount();
			case BuildingType.HeroMobilizer:
				return this.TacticalCommandNodeList.CalculateCount();
			case BuildingType.ChampionPlatform:
			{
				int num = 0;
				for (ChampionPlatformNode championPlatformNode = this.ChampionPlatformNodeList.Head; championPlatformNode != null; championPlatformNode = championPlatformNode.Next)
				{
					if (championPlatformNode.BuildingComp.BuildingType.UpgradeGroup == building.UpgradeGroup)
					{
						num++;
					}
				}
				return num;
			}
			case BuildingType.Housing:
				return this.HousingNodeList.CalculateCount();
			case BuildingType.Squad:
				return this.SquadBuildingNodeList.CalculateCount();
			case BuildingType.Starport:
				return this.StarportNodeList.CalculateCount();
			case BuildingType.DroidHut:
				return this.DroidHutNodeList.CalculateCount();
			case BuildingType.Wall:
				return this.WallNodeList.CalculateCount();
			case BuildingType.Turret:
			{
				int num2 = 0;
				for (TurretBuildingNode turretBuildingNode = this.TurretBuildingNodeList.Head; turretBuildingNode != null; turretBuildingNode = turretBuildingNode.Next)
				{
					if (turretBuildingNode.BuildingComp.BuildingType.UpgradeGroup == building.UpgradeGroup)
					{
						num2++;
					}
				}
				return num2;
			}
			case BuildingType.TroopResearch:
				return this.OffenseLabNodeList.CalculateCount();
			case BuildingType.DefenseResearch:
				return this.DefenseLabNodeList.CalculateCount();
			case BuildingType.Resource:
			{
				int num3 = 0;
				for (GeneratorNode generatorNode = this.GeneratorNodeList.Head; generatorNode != null; generatorNode = generatorNode.Next)
				{
					if (generatorNode.BuildingComp.BuildingType.Currency == building.Currency)
					{
						num3++;
					}
				}
				return num3;
			}
			case BuildingType.Storage:
			{
				int num4 = 0;
				for (StorageNode storageNode = this.StorageNodeList.Head; storageNode != null; storageNode = storageNode.Next)
				{
					if (storageNode.BuildingComp.BuildingType.Currency == building.Currency)
					{
						num4++;
					}
				}
				return num4;
			}
			case BuildingType.ShieldGenerator:
			{
				int num5 = 0;
				NodeList<BuildingNode> nodeList = this.entityController.GetNodeList<BuildingNode>();
				for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
				{
					if (buildingNode.BuildingComp.BuildingType.Type == BuildingType.ShieldGenerator)
					{
						num5++;
					}
				}
				return num5;
			}
			case BuildingType.Rubble:
				return 0;
			case BuildingType.Trap:
			{
				int num6 = 0;
				for (TrapBuildingNode trapBuildingNode = this.TrapBuildingNodeList.Head; trapBuildingNode != null; trapBuildingNode = trapBuildingNode.Next)
				{
					if (trapBuildingNode.BuildingComp.BuildingType.UpgradeGroup == building.UpgradeGroup)
					{
						num6++;
					}
				}
				return num6;
			}
			case BuildingType.Cantina:
				return this.CantinaNodeList.CalculateCount();
			case BuildingType.NavigationCenter:
				return this.NavigationCenterNodeList.CalculateCount();
			case BuildingType.ScoutTower:
				return this.ScoutTowerNodeList.CalculateCount();
			case BuildingType.Armory:
				return this.ArmoryNodeList.CalculateCount();
			}
			Service.Get<Logger>().Warn("Unknown building type for count: " + building.Uid);
			return 0;
		}

		public List<Entity> GetBuildingListByType(BuildingType type)
		{
			List<Entity> list = new List<Entity>();
			this.FillBuildingListByType(list, type);
			return list;
		}

		private void FillBuildingListByType(List<Entity> list, BuildingType type)
		{
			switch (type)
			{
			case BuildingType.Any:
			{
				NodeList<BuildingNode> nodeList = this.entityController.GetNodeList<BuildingNode>();
				for (BuildingNode buildingNode = nodeList.Head; buildingNode != null; buildingNode = buildingNode.Next)
				{
					if (type == BuildingType.Any || type == buildingNode.BuildingComp.BuildingType.Type)
					{
						list.Add(buildingNode.Entity);
					}
				}
				return;
			}
			case BuildingType.HQ:
				for (HQNode hQNode = this.HQNodeList.Head; hQNode != null; hQNode = hQNode.Next)
				{
					list.Add(hQNode.Entity);
				}
				return;
			case BuildingType.Barracks:
				for (BarracksNode barracksNode = this.BarracksNodeList.Head; barracksNode != null; barracksNode = barracksNode.Next)
				{
					list.Add(barracksNode.Entity);
				}
				return;
			case BuildingType.Factory:
				for (FactoryNode factoryNode = this.FactoryNodeList.Head; factoryNode != null; factoryNode = factoryNode.Next)
				{
					list.Add(factoryNode.Entity);
				}
				return;
			case BuildingType.FleetCommand:
				for (FleetCommandNode fleetCommandNode = this.FleetCommandNodeList.Head; fleetCommandNode != null; fleetCommandNode = fleetCommandNode.Next)
				{
					list.Add(fleetCommandNode.Entity);
				}
				return;
			case BuildingType.HeroMobilizer:
				for (TacticalCommandNode tacticalCommandNode = this.TacticalCommandNodeList.Head; tacticalCommandNode != null; tacticalCommandNode = tacticalCommandNode.Next)
				{
					list.Add(tacticalCommandNode.Entity);
				}
				return;
			case BuildingType.ChampionPlatform:
				for (ChampionPlatformNode championPlatformNode = this.ChampionPlatformNodeList.Head; championPlatformNode != null; championPlatformNode = championPlatformNode.Next)
				{
					list.Add(championPlatformNode.Entity);
				}
				return;
			case BuildingType.Housing:
				for (HousingNode housingNode = this.HousingNodeList.Head; housingNode != null; housingNode = housingNode.Next)
				{
					list.Add(housingNode.Entity);
				}
				return;
			case BuildingType.Squad:
				for (SquadBuildingNode squadBuildingNode = this.SquadBuildingNodeList.Head; squadBuildingNode != null; squadBuildingNode = squadBuildingNode.Next)
				{
					list.Add(squadBuildingNode.Entity);
				}
				return;
			case BuildingType.Starport:
				for (StarportNode starportNode = this.StarportNodeList.Head; starportNode != null; starportNode = starportNode.Next)
				{
					list.Add(starportNode.Entity);
				}
				return;
			case BuildingType.DroidHut:
				for (DroidHutNode droidHutNode = this.DroidHutNodeList.Head; droidHutNode != null; droidHutNode = droidHutNode.Next)
				{
					list.Add(droidHutNode.Entity);
				}
				return;
			case BuildingType.Wall:
				for (WallNode wallNode = this.WallNodeList.Head; wallNode != null; wallNode = wallNode.Next)
				{
					list.Add(wallNode.Entity);
				}
				return;
			case BuildingType.Turret:
				for (TurretBuildingNode turretBuildingNode = this.TurretBuildingNodeList.Head; turretBuildingNode != null; turretBuildingNode = turretBuildingNode.Next)
				{
					list.Add(turretBuildingNode.Entity);
				}
				return;
			case BuildingType.TroopResearch:
				for (OffenseLabNode offenseLabNode = this.OffenseLabNodeList.Head; offenseLabNode != null; offenseLabNode = offenseLabNode.Next)
				{
					list.Add(offenseLabNode.Entity);
				}
				return;
			case BuildingType.DefenseResearch:
				for (DefenseLabNode defenseLabNode = this.DefenseLabNodeList.Head; defenseLabNode != null; defenseLabNode = defenseLabNode.Next)
				{
					list.Add(defenseLabNode.Entity);
				}
				return;
			case BuildingType.Resource:
				for (GeneratorNode generatorNode = this.GeneratorNodeList.Head; generatorNode != null; generatorNode = generatorNode.Next)
				{
					list.Add(generatorNode.Entity);
				}
				return;
			case BuildingType.Storage:
				for (StorageNode storageNode = this.StorageNodeList.Head; storageNode != null; storageNode = storageNode.Next)
				{
					list.Add(storageNode.Entity);
				}
				return;
			case BuildingType.ShieldGenerator:
				for (ShieldGeneratorNode shieldGeneratorNode = this.ShieldGeneratorNodeList.Head; shieldGeneratorNode != null; shieldGeneratorNode = shieldGeneratorNode.Next)
				{
					list.Add(shieldGeneratorNode.Entity);
				}
				return;
			case BuildingType.Clearable:
				for (ClearableNode clearableNode = this.ClearableNodeList.Head; clearableNode != null; clearableNode = clearableNode.Next)
				{
					list.Add(clearableNode.Entity);
				}
				return;
			case BuildingType.Trap:
				for (TrapNode trapNode = this.TrapNodeList.Head; trapNode != null; trapNode = trapNode.Next)
				{
					list.Add(trapNode.Entity);
				}
				return;
			case BuildingType.Cantina:
				for (CantinaNode cantinaNode = this.CantinaNodeList.Head; cantinaNode != null; cantinaNode = cantinaNode.Next)
				{
					list.Add(cantinaNode.Entity);
				}
				return;
			case BuildingType.NavigationCenter:
				for (NavigationCenterNode navigationCenterNode = this.NavigationCenterNodeList.Head; navigationCenterNode != null; navigationCenterNode = navigationCenterNode.Next)
				{
					list.Add(navigationCenterNode.Entity);
				}
				return;
			case BuildingType.ScoutTower:
				for (ScoutTowerNode scoutTowerNode = this.ScoutTowerNodeList.Head; scoutTowerNode != null; scoutTowerNode = scoutTowerNode.Next)
				{
					list.Add(scoutTowerNode.Entity);
				}
				return;
			case BuildingType.Armory:
				for (ArmoryNode armoryNode = this.ArmoryNodeList.Head; armoryNode != null; armoryNode = armoryNode.Next)
				{
					list.Add(armoryNode.Entity);
				}
				return;
			}
			Service.Get<Logger>().Warn("Unknown building type " + type);
		}

		public Entity GetCurrentHQ()
		{
			HQNode head = this.HQNodeList.Head;
			return (head != null) ? head.HQComp.Entity : null;
		}

		public Entity GetCurrentSquadBuilding()
		{
			SquadBuildingNode head = this.SquadBuildingNodeList.Head;
			return (head != null) ? head.SquadBuildingComp.Entity : null;
		}

		public Entity GetCurrentNavigationCenter()
		{
			NavigationCenterNode head = this.NavigationCenterNodeList.Head;
			return (head != null) ? head.NavigationCenterComp.Entity : null;
		}

		public Entity GetCurrentArmory()
		{
			ArmoryNode head = this.ArmoryNodeList.Head;
			return (head != null) ? head.ArmoryComp.Entity : null;
		}

		public int GetHighestLevelHQ()
		{
			int num = 0;
			for (HQNode hQNode = this.HQNodeList.Head; hQNode != null; hQNode = hQNode.Next)
			{
				int buildingEffectiveLevel = GameUtils.GetBuildingEffectiveLevel(hQNode.Entity);
				if (buildingEffectiveLevel > num)
				{
					num = buildingEffectiveLevel;
				}
			}
			return num;
		}

		public int GetNumberOfClearables()
		{
			return this.ClearableNodeList.CalculateCount();
		}

		public bool IsTurretSwappingUnlocked()
		{
			HQNode head = this.HQNodeList.Head;
			return head != null && head.BuildingComp.BuildingType.Lvl >= GameConstants.TURRET_SWAP_HQ_UNLOCK;
		}

		public bool IsContrabandUnlocked()
		{
			IDataController dataController = Service.Get<IDataController>();
			foreach (BuildingTypeVO current in dataController.GetAll<BuildingTypeVO>())
			{
				if (current.Type == BuildingType.Storage && current.Currency == CurrencyType.Contraband && current.Lvl == 1 && this.HasConstructedBuilding(current))
				{
					return true;
				}
			}
			return false;
		}

		public int GetBuildingMaxPurchaseQuantity(BuildingTypeVO building, int haveReqBuildingLvl)
		{
			BuildingTypeVO buildingTypeVO = this.GetBuildingConstructionRequirement(building);
			if (buildingTypeVO == null)
			{
				return building.MaxQuantity;
			}
			if (haveReqBuildingLvl >= buildingTypeVO.Lvl || this.HasConstructedBuilding(buildingTypeVO))
			{
				BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
				int lvl = buildingUpgradeCatalog.GetMinLevel(building.UpgradeGroup).Lvl;
				int lvl2 = buildingUpgradeCatalog.GetMaxLevel(building.UpgradeGroup).Lvl;
				for (int i = lvl2; i > lvl; i--)
				{
					BuildingTypeVO byLevel = buildingUpgradeCatalog.GetByLevel(building.UpgradeGroup, i);
					if (byLevel != null)
					{
						buildingTypeVO = this.GetRequiredBuilding(byLevel.BuildingRequirement);
						if (buildingTypeVO == null || haveReqBuildingLvl >= buildingTypeVO.Lvl || this.HasConstructedBuilding(buildingTypeVO))
						{
							return byLevel.MaxQuantity;
						}
					}
				}
				BuildingTypeVO minLevel = buildingUpgradeCatalog.GetMinLevel(building);
				return minLevel.MaxQuantity;
			}
			if (Service.Get<UnlockController>().HasUnlockedBuildingByReward(building))
			{
				return 1;
			}
			return 0;
		}

		public bool HasConstructedBuilding(BuildingTypeVO reqBuilding)
		{
			int lvl = reqBuilding.Lvl;
			switch (reqBuilding.Type)
			{
			case BuildingType.HQ:
				for (HQNode hQNode = this.HQNodeList.Head; hQNode != null; hQNode = hQNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(hQNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Barracks:
				for (BarracksNode barracksNode = this.BarracksNodeList.Head; barracksNode != null; barracksNode = barracksNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(barracksNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Factory:
				for (FactoryNode factoryNode = this.FactoryNodeList.Head; factoryNode != null; factoryNode = factoryNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(factoryNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.FleetCommand:
				for (FleetCommandNode fleetCommandNode = this.FleetCommandNodeList.Head; fleetCommandNode != null; fleetCommandNode = fleetCommandNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(fleetCommandNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.HeroMobilizer:
				for (TacticalCommandNode tacticalCommandNode = this.TacticalCommandNodeList.Head; tacticalCommandNode != null; tacticalCommandNode = tacticalCommandNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(tacticalCommandNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.ChampionPlatform:
				for (ChampionPlatformNode championPlatformNode = this.ChampionPlatformNodeList.Head; championPlatformNode != null; championPlatformNode = championPlatformNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(championPlatformNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Housing:
				for (HousingNode housingNode = this.HousingNodeList.Head; housingNode != null; housingNode = housingNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(housingNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Squad:
				for (SquadBuildingNode squadBuildingNode = this.SquadBuildingNodeList.Head; squadBuildingNode != null; squadBuildingNode = squadBuildingNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(squadBuildingNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Starport:
				for (StarportNode starportNode = this.StarportNodeList.Head; starportNode != null; starportNode = starportNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(starportNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.DroidHut:
				for (DroidHutNode droidHutNode = this.DroidHutNodeList.Head; droidHutNode != null; droidHutNode = droidHutNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(droidHutNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Wall:
				for (WallNode wallNode = this.WallNodeList.Head; wallNode != null; wallNode = wallNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(wallNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Turret:
				for (TurretBuildingNode turretBuildingNode = this.TurretBuildingNodeList.Head; turretBuildingNode != null; turretBuildingNode = turretBuildingNode.Next)
				{
					if (turretBuildingNode.BuildingComp.BuildingType.UpgradeGroup == reqBuilding.UpgradeGroup && GameUtils.GetBuildingEffectiveLevel(turretBuildingNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.TroopResearch:
				for (OffenseLabNode offenseLabNode = this.OffenseLabNodeList.Head; offenseLabNode != null; offenseLabNode = offenseLabNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(offenseLabNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.DefenseResearch:
				for (DefenseLabNode defenseLabNode = this.DefenseLabNodeList.Head; defenseLabNode != null; defenseLabNode = defenseLabNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(defenseLabNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Resource:
				for (GeneratorNode generatorNode = this.GeneratorNodeList.Head; generatorNode != null; generatorNode = generatorNode.Next)
				{
					if (generatorNode.BuildingComp.BuildingType.Currency == reqBuilding.Currency && GameUtils.GetBuildingEffectiveLevel(generatorNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Storage:
				for (StorageNode storageNode = this.StorageNodeList.Head; storageNode != null; storageNode = storageNode.Next)
				{
					if (storageNode.BuildingComp.BuildingType.Currency == reqBuilding.Currency && GameUtils.GetBuildingEffectiveLevel(storageNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Trap:
				for (TrapNode trapNode = this.TrapNodeList.Head; trapNode != null; trapNode = trapNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(trapNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Cantina:
				for (CantinaNode cantinaNode = this.CantinaNodeList.Head; cantinaNode != null; cantinaNode = cantinaNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(cantinaNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.NavigationCenter:
				for (NavigationCenterNode navigationCenterNode = this.NavigationCenterNodeList.Head; navigationCenterNode != null; navigationCenterNode = navigationCenterNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(navigationCenterNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.ScoutTower:
				for (ScoutTowerNode scoutTowerNode = this.ScoutTowerNodeList.Head; scoutTowerNode != null; scoutTowerNode = scoutTowerNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(scoutTowerNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			case BuildingType.Armory:
				for (ArmoryNode armoryNode = this.ArmoryNodeList.Head; armoryNode != null; armoryNode = armoryNode.Next)
				{
					if (GameUtils.GetBuildingEffectiveLevel(armoryNode.BuildingComp.Entity) >= lvl)
					{
						return true;
					}
				}
				return false;
			}
			Service.Get<Logger>().Warn("Unknown reqBuilding type for level: " + reqBuilding.Uid);
			return false;
		}

		public Dictionary<BuildingTypeVO, int> GetBuildingsUnlockedBy(BuildingTypeVO reqBuilding)
		{
			Dictionary<BuildingTypeVO, int> dictionary = new Dictionary<BuildingTypeVO, int>();
			IDataController dataController = Service.Get<IDataController>();
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			foreach (BuildingTypeVO current in dataController.GetAll<BuildingTypeVO>())
			{
				if (current.BuildingRequirement == reqBuilding.Uid || current.BuildingRequirement2 == reqBuilding.Uid)
				{
					BuildingTypeVO minLevel = buildingUpgradeCatalog.GetMinLevel(current);
					BuildingTypeVO buildingTypeVO = (current.Lvl <= minLevel.Lvl) ? null : buildingUpgradeCatalog.GetByLevel(current, current.Lvl - 1);
					int num = (buildingTypeVO != null) ? buildingTypeVO.MaxQuantity : 0;
					int num2 = current.MaxQuantity - num;
					if (num2 > 0)
					{
						if (dictionary.ContainsKey(minLevel))
						{
							Dictionary<BuildingTypeVO, int> dictionary2;
							Dictionary<BuildingTypeVO, int> expr_C1 = dictionary2 = dictionary;
							BuildingTypeVO key;
							BuildingTypeVO expr_C6 = key = minLevel;
							int num3 = dictionary2[key];
							expr_C1[expr_C6] = num3 + num2;
						}
						else
						{
							dictionary.Add(minLevel, num2);
						}
					}
				}
			}
			return dictionary;
		}

		public bool DoesMeetMinBuildingRequirement(IUpgradeableVO vo)
		{
			BuildingTypeVO minBuildingRequirement = Service.Get<BuildingLookupController>().GetMinBuildingRequirement(vo);
			return this.HasConstructedBuilding(minBuildingRequirement);
		}

		public BuildingTypeVO GetMinBuildingRequirement(IUpgradeableVO vo)
		{
			BuildingTypeVO result = null;
			if (vo is TroopTypeVO)
			{
				TroopTypeVO minLevel = Service.Get<TroopUpgradeCatalog>().GetMinLevel(vo as TroopTypeVO);
				result = Service.Get<BuildingLookupController>().GetTroopUnlockRequirement(minLevel);
			}
			else if (vo is SpecialAttackTypeVO)
			{
				SpecialAttackTypeVO minLevel2 = Service.Get<StarshipUpgradeCatalog>().GetMinLevel(vo as SpecialAttackTypeVO);
				result = Service.Get<BuildingLookupController>().GetStarshipUnlockRequirement(minLevel2);
			}
			return result;
		}

		public List<TroopTypeVO> GetTroopsUnlockedByBuilding(string reqBuildingUid)
		{
			TroopUpgradeCatalog catalog = Service.Get<TroopUpgradeCatalog>();
			return this.GetItemsUnlockedByBuilding<TroopTypeVO>(reqBuildingUid, catalog);
		}

		public List<SpecialAttackTypeVO> GetStarshipsUnlockedByBuilding(string reqBuildingUid)
		{
			StarshipUpgradeCatalog catalog = Service.Get<StarshipUpgradeCatalog>();
			return this.GetItemsUnlockedByBuilding<SpecialAttackTypeVO>(reqBuildingUid, catalog);
		}

		private List<T> GetItemsUnlockedByBuilding<T>(string reqBuildingUid, GenericUpgradeCatalog<T> catalog) where T : IUpgradeableVO
		{
			List<T> list = new List<T>();
			IDataController dataController = Service.Get<IDataController>();
			foreach (T current in dataController.GetAll<T>())
			{
				if (current.BuildingRequirement == reqBuildingUid)
				{
					T minLevel = catalog.GetMinLevel(current);
					if (minLevel.Lvl == current.Lvl)
					{
						list.Add(minLevel);
					}
				}
			}
			return list;
		}

		private BuildingTypeVO GetRequiredBuilding(string reqUid)
		{
			if (string.IsNullOrEmpty(reqUid))
			{
				return null;
			}
			IDataController dataController = Service.Get<IDataController>();
			return dataController.Get<BuildingTypeVO>(reqUid);
		}

		private BuildingTypeVO GetBuildingConstructionRequirement(BuildingTypeVO building)
		{
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			BuildingTypeVO minLevel = buildingUpgradeCatalog.GetMinLevel(building);
			return this.GetRequiredBuilding(minLevel.BuildingRequirement);
		}

		public BuildingTypeVO GetTroopUnlockRequirement(TroopTypeVO troop)
		{
			return this.GetRequiredBuilding(troop.BuildingRequirement);
		}

		public BuildingTypeVO GetStarshipUnlockRequirement(SpecialAttackTypeVO starship)
		{
			return this.GetRequiredBuilding(starship.BuildingRequirement);
		}

		public BuildingTypeVO GetEquipmentUnlockRequirement(EquipmentVO equipmentVO)
		{
			return this.GetRequiredBuilding(equipmentVO.BuildingRequirement);
		}

		public BuildingTypeVO GetHeroSlotUnlockRequirement(BuildingTypeVO heroBuilding, int slotNum)
		{
			BuildingUpgradeCatalog buildingUpgradeCatalog = Service.Get<BuildingUpgradeCatalog>();
			List<BuildingTypeVO> upgradeGroupLevels = buildingUpgradeCatalog.GetUpgradeGroupLevels(heroBuilding.UpgradeGroup);
			BuildingTypeVO result = null;
			for (int i = 0; i < upgradeGroupLevels.Count; i++)
			{
				if (upgradeGroupLevels[i].Storage >= slotNum)
				{
					result = upgradeGroupLevels[i];
					break;
				}
			}
			return result;
		}

		public BuildingTypeVO GetHighestAvailableBuildingVOForTroop(TroopTypeVO troop)
		{
			Entity entity = null;
			switch (troop.Type)
			{
			case TroopType.Infantry:
				entity = this.GetHighestAvailableBarracks();
				break;
			case TroopType.Vehicle:
				entity = this.GetHighestAvailableFactory();
				break;
			case TroopType.Mercenary:
			{
				CantinaNode head = this.CantinaNodeList.Head;
				if (head != null)
				{
					entity = head.Entity;
				}
				break;
			}
			case TroopType.Hero:
			{
				TacticalCommandNode head2 = this.TacticalCommandNodeList.Head;
				if (head2 != null)
				{
					entity = head2.Entity;
				}
				break;
			}
			default:
				Service.Get<Logger>().Error("GetHighestAvailableBuildingVOForTroop; Unsupported troop type: " + troop.Type.ToString() + " UID: " + troop.Uid);
				return null;
			}
			BuildingTypeVO buildingTypeVO = null;
			if (entity != null)
			{
				BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
				if (buildingComponent == null)
				{
					Service.Get<Logger>().Error("GetHighestAvailableBuildingVOForTroop; No building component found for : " + buildingTypeVO.Uid);
					return null;
				}
				buildingTypeVO = buildingComponent.BuildingType;
			}
			return buildingTypeVO;
		}

		public Entity GetHighestAvailableBarracks()
		{
			List<BuildingComponent> list = new List<BuildingComponent>();
			for (BarracksNode barracksNode = this.BarracksNodeList.Head; barracksNode != null; barracksNode = barracksNode.Next)
			{
				if (!ContractUtils.IsBuildingConstructing(barracksNode.Entity) && !ContractUtils.IsBuildingUpgrading(barracksNode.Entity))
				{
					list.Add(barracksNode.BuildingComp);
				}
			}
			list.Sort(new Comparison<BuildingComponent>(this.CompareBuildingLevels));
			return (list.Count == 0) ? null : list[0].Entity;
		}

		public Entity GetHighestAvailableFactory()
		{
			List<BuildingComponent> list = new List<BuildingComponent>();
			for (FactoryNode factoryNode = this.FactoryNodeList.Head; factoryNode != null; factoryNode = factoryNode.Next)
			{
				if (!ContractUtils.IsBuildingConstructing(factoryNode.Entity) && !ContractUtils.IsBuildingUpgrading(factoryNode.Entity))
				{
					list.Add(factoryNode.BuildingComp);
				}
			}
			list.Sort(new Comparison<BuildingComponent>(this.CompareBuildingLevels));
			return (list.Count == 0) ? null : list[0].Entity;
		}

		public Entity GetAvailableScoutTower()
		{
			for (ScoutTowerNode scoutTowerNode = this.ScoutTowerNodeList.Head; scoutTowerNode != null; scoutTowerNode = scoutTowerNode.Next)
			{
				if (!ContractUtils.IsBuildingConstructing(scoutTowerNode.Entity) && !ContractUtils.IsBuildingUpgrading(scoutTowerNode.Entity))
				{
					return scoutTowerNode.Entity;
				}
			}
			return null;
		}

		public Entity GetAvailableTroopResearchLab()
		{
			for (OffenseLabNode offenseLabNode = this.OffenseLabNodeList.Head; offenseLabNode != null; offenseLabNode = offenseLabNode.Next)
			{
				if (!ContractUtils.IsBuildingConstructing(offenseLabNode.Entity) && !ContractUtils.IsBuildingUpgrading(offenseLabNode.Entity))
				{
					return offenseLabNode.Entity;
				}
			}
			return null;
		}

		private int CompareBuildingLevels(BuildingComponent a, BuildingComponent b)
		{
			return b.BuildingType.Lvl - a.BuildingType.Lvl;
		}

		public int GetHighestLevelForBarracks()
		{
			int num = 1;
			for (BarracksNode barracksNode = this.BarracksNodeList.Head; barracksNode != null; barracksNode = barracksNode.Next)
			{
				int lvl = barracksNode.BuildingComp.BuildingType.Lvl;
				if (lvl > num)
				{
					num = lvl;
				}
			}
			return num;
		}

		public int GetHighestLevelForFactories()
		{
			int num = 1;
			for (FactoryNode factoryNode = this.FactoryNodeList.Head; factoryNode != null; factoryNode = factoryNode.Next)
			{
				int lvl = factoryNode.BuildingComp.BuildingType.Lvl;
				if (lvl > num)
				{
					num = lvl;
				}
			}
			return num;
		}

		public int GetHighestLevelForCantinas()
		{
			int num = 1;
			for (CantinaNode cantinaNode = this.CantinaNodeList.Head; cantinaNode != null; cantinaNode = cantinaNode.Next)
			{
				int lvl = cantinaNode.BuildingComp.BuildingType.Lvl;
				if (lvl > num)
				{
					num = lvl;
				}
			}
			return num;
		}

		public int GetHighestLevelForHeroCommands()
		{
			int num = 1;
			for (TacticalCommandNode tacticalCommandNode = this.TacticalCommandNodeList.Head; tacticalCommandNode != null; tacticalCommandNode = tacticalCommandNode.Next)
			{
				int lvl = tacticalCommandNode.BuildingComp.BuildingType.Lvl;
				if (lvl > num)
				{
					num = lvl;
				}
			}
			return num;
		}

		public int GetHighestLevelForStarshipCommands()
		{
			int num = 1;
			for (FleetCommandNode fleetCommandNode = this.FleetCommandNodeList.Head; fleetCommandNode != null; fleetCommandNode = fleetCommandNode.Next)
			{
				int lvl = fleetCommandNode.BuildingComp.BuildingType.Lvl;
				if (lvl > num)
				{
					num = lvl;
				}
			}
			return num;
		}

		public bool HasHeroCommand()
		{
			return this.TacticalCommandNodeList.Head != null;
		}

		public bool HasStarshipCommand()
		{
			return this.FleetCommandNodeList.Head != null;
		}

		public bool HasNavigationCenter()
		{
			bool result = false;
			for (NavigationCenterNode navigationCenterNode = this.NavigationCenterNodeList.Head; navigationCenterNode != null; navigationCenterNode = navigationCenterNode.Next)
			{
				result = !ContractUtils.IsBuildingConstructing(navigationCenterNode.BuildingComp.Entity);
			}
			return result;
		}
	}
}
