using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class Bullet
	{
		public SmartEntity Owner;

		private int beamSegment;

		private int beamOriginX;

		private int beamOriginZ;

		private int beamDirectionX;

		private int beamDirectionZ;

		private int beamX;

		private int beamZ;

		private int beamRadius;

		private int beamTrail;

		private const float STARSHIP_TARGET_OFFSET = 1f;

		public HealthFragment HealthFrag
		{
			get;
			protected set;
		}

		public ISplashVO SplashVO
		{
			get;
			protected set;
		}

		public TeamType OwnerTeam
		{
			get;
			protected set;
		}

		public int SpawnBoardX
		{
			get;
			protected set;
		}

		public int SpawnBoardZ
		{
			get;
			protected set;
		}

		public Vector3 SpawnWorldLocation
		{
			get;
			protected set;
		}

		public GameObject GunLocator
		{
			get;
			protected set;
		}

		public SmartEntity Target
		{
			get;
			protected set;
		}

		public bool FlashTarget
		{
			get;
			set;
		}

		public int TargetBoardX
		{
			get;
			protected set;
		}

		public int TargetBoardZ
		{
			get;
			protected set;
		}

		public Vector3 TargetWorldLocation
		{
			get;
			protected set;
		}

		public object Cookie
		{
			get;
			set;
		}

		public uint TravelTime
		{
			get;
			protected set;
		}

		public bool IsDeflection
		{
			get;
			protected set;
		}

		public Dictionary<uint, BeamTarget> BeamTargets
		{
			get;
			private set;
		}

		public bool AppliedBeamHitFXThisSegment
		{
			get;
			set;
		}

		public ProjectileTypeVO ProjectileType
		{
			get;
			protected set;
		}

		public List<Buff> AppliedBuffs
		{
			get;
			private set;
		}

		public FactionType OwnerFaction
		{
			get;
			private set;
		}

		public bool InitWithTarget(int spawnBoardX, int spawnBoardZ, Vector3 spawnWorldLocation, Target target, TeamType ownerTeam, Entity attacker, HealthFragment healthFrag, ProjectileTypeVO projectileTypeVO, List<Buff> appliedBuffs, FactionType faction, GameObject gunLocator)
		{
			if (target == null)
			{
				return false;
			}
			this.OwnerFaction = faction;
			this.AppliedBuffs = appliedBuffs;
			this.InitWithEverything(0u, spawnBoardX, spawnBoardZ, spawnWorldLocation, target.TargetEntity, target.TargetBoardX, target.TargetBoardZ, ownerTeam, attacker, healthFrag, projectileTypeVO, gunLocator);
			if (this.ProjectileType.IsBeam)
			{
				this.SetupBeam();
			}
			else
			{
				this.SetTargetWorldLocation(target.TargetWorldLocation);
			}
			return true;
		}

		public void InitWithTargetPositionAndTravelTime(uint travelTime, Vector3 spawnWorldLocation, int targetBoardX, int targetBoardZ, TeamType ownerTeam, Entity attacker, HealthFragment healthFrag, ProjectileTypeVO projectileType, List<Buff> appliedBuffs, FactionType faction)
		{
			this.InitWithEverything(travelTime, 0, 0, spawnWorldLocation, null, targetBoardX, targetBoardZ, ownerTeam, attacker, healthFrag, projectileType, null);
			this.OwnerFaction = faction;
			this.AppliedBuffs = appliedBuffs;
			this.SetDefaultTargetWorldLocation(0f);
		}

		private void InitWithEverything(uint travelTime, int spawnBoardX, int spawnBoardZ, Vector3 spawnWorldLocation, SmartEntity target, int targetBoardX, int targetBoardZ, TeamType ownerTeam, Entity attacker, HealthFragment healthFrag, ProjectileTypeVO projectileType, GameObject gunLocator)
		{
			this.IsDeflection = false;
			this.SetTravelTime(travelTime);
			this.SpawnBoardX = spawnBoardX;
			this.SpawnBoardZ = spawnBoardZ;
			this.SpawnWorldLocation = spawnWorldLocation;
			this.GunLocator = gunLocator;
			this.SetTarget(target);
			this.TargetBoardX = targetBoardX;
			this.TargetBoardZ = targetBoardZ;
			this.OwnerTeam = ownerTeam;
			this.Owner = (SmartEntity)attacker;
			this.HealthFrag = healthFrag;
			this.ProjectileType = projectileType;
		}

		public void ChangeToDeflection(uint travelTime, Vector3 spawnWorldLocation, Vector3 targetWorldLocation)
		{
			this.IsDeflection = true;
			this.SetTarget(null);
			this.SetTravelTime(travelTime);
			this.SpawnWorldLocation = spawnWorldLocation;
			this.SetTargetWorldLocation(targetWorldLocation);
		}

		private void SetDefaultTargetWorldLocation(float targetWorldY)
		{
			float num = 0f;
			if (this.Target == null)
			{
				num = 1f;
			}
			this.SetTargetWorldLocation(new Vector3(Units.BoardToWorldX(this.TargetBoardX) + num, targetWorldY, Units.BoardToWorldZ(this.TargetBoardZ) + num));
		}

		public void SetTravelTime(uint travelTime)
		{
			this.TravelTime = ((travelTime == 0u) ? 1u : travelTime);
		}

		public void AddSplash(ISplashVO splashVO)
		{
			this.SplashVO = splashVO;
		}

		public bool IsSplash()
		{
			return this.SplashVO != null;
		}

		public void SetTarget(SmartEntity target)
		{
			this.Target = target;
		}

		public void SetTargetWorldLocation(Vector3 worldLoc)
		{
			this.TargetWorldLocation = worldLoc;
		}

		public List<BoardCell<Entity>> GetBeamNearbyCells()
		{
			if (this.beamSegment >= this.ProjectileType.BeamDamageLength)
			{
				return null;
			}
			int num = this.beamX - this.beamTrail * this.beamDirectionX / 2;
			int num2 = this.beamZ - this.beamTrail * this.beamDirectionZ / 2;
			return Service.Get<BoardController>().Board.GetCellsInSquare(this.beamRadius, num / 1024, num2 / 1024);
		}

		public int GetBeamDamagePercent(int x, int z)
		{
			if (this.beamSegment >= this.ProjectileType.BeamDamageLength)
			{
				return 0;
			}
			int num = 512;
			x = x * 1024 + num;
			z = z * 1024 + num;
			int num2 = x - this.beamX;
			int num3 = z - this.beamZ;
			int num4 = (num2 * this.beamDirectionX + num3 * this.beamDirectionZ) / 1024;
			int num5 = num;
			int num6 = -num - this.beamTrail * 1024;
			if (num6 < num4 && num4 <= num5)
			{
				int x2 = this.beamX + num4 * this.beamDirectionX / 1024;
				int y = this.beamZ + num4 * this.beamDirectionZ / 1024;
				int num7 = 2 * IntMath.FastDist(x2, y, x, z) / 1048576;
				if (num7 < this.ProjectileType.BeamWidthSegments.Length)
				{
					return this.ProjectileType.BeamLengthSegments[this.beamSegment] * this.ProjectileType.BeamWidthSegments[num7] / 100;
				}
			}
			return 0;
		}

		public void ApplyBeamDamagePercent(SmartEntity target, int beamPercent)
		{
			if (this.BeamTargets == null)
			{
				return;
			}
			uint iD = target.ID;
			BeamTarget beamTarget;
			if (this.BeamTargets.ContainsKey(iD))
			{
				beamTarget = this.BeamTargets[iD];
			}
			else
			{
				beamTarget = new BeamTarget(target);
				this.BeamTargets.Add(iD, beamTarget);
			}
			beamTarget.ApplyBeamDamage(beamPercent);
		}

		public bool IsBeamFirstHit(SmartEntity target)
		{
			uint iD = target.ID;
			return this.BeamTargets.ContainsKey(iD) && this.BeamTargets[iD].IsFirstHit;
		}

		public void AdvanceBeam()
		{
			this.InternalSetBeamSegment(this.beamSegment + 1);
		}

		private void InternalSetBeamSegment(int segment)
		{
			this.beamSegment = segment;
			this.AppliedBeamHitFXThisSegment = false;
			this.beamX = this.beamOriginX + this.beamDirectionX * this.beamSegment;
			this.beamZ = this.beamOriginZ + this.beamDirectionZ * this.beamSegment;
			int beamInitialZeroes = this.ProjectileType.BeamInitialZeroes;
			if (this.beamSegment > beamInitialZeroes)
			{
				int num = this.beamSegment - beamInitialZeroes;
				int beamBulletLength = this.ProjectileType.BeamBulletLength;
				this.beamTrail = ((beamBulletLength < num) ? beamBulletLength : num);
			}
			else
			{
				this.beamTrail = 0;
			}
			int num2 = this.ProjectileType.BeamWidthSegments.Length / 2 + 1;
			int num3 = (this.beamTrail + 3) / 2;
			this.beamRadius = ((num2 > num3) ? num2 : num3);
		}

		private void SetupBeam()
		{
			this.InternalSetBeamSegment(0);
			this.BeamTargets = new Dictionary<uint, BeamTarget>();
			this.beamOriginX = this.SpawnBoardX;
			this.beamOriginZ = this.SpawnBoardZ;
			this.GetAccurateBoardLocation(this.Owner, ref this.beamOriginX, ref this.beamOriginZ);
			this.beamX = this.beamOriginX;
			this.beamZ = this.beamOriginZ;
			int num = this.TargetBoardX;
			int num2 = this.TargetBoardZ;
			this.GetAccurateBoardLocation(this.Target, ref num, ref num2);
			int num3 = IntMath.FastDist(this.beamOriginX, this.beamOriginZ, num, num2) / 1024;
			if (num3 == 0)
			{
				this.beamDirectionX = 1024;
				this.beamDirectionZ = 0;
			}
			else
			{
				this.beamDirectionX = (num - this.beamOriginX) * 1024 / num3;
				this.beamDirectionZ = (num2 - this.beamOriginZ) * 1024 / num3;
			}
			int beamLifeLength = this.ProjectileType.BeamLifeLength;
			num = this.beamOriginX + this.beamDirectionX * beamLifeLength;
			num2 = this.beamOriginZ + this.beamDirectionZ * beamLifeLength;
			float num4 = 0.0029296875f;
			this.SetTargetWorldLocation(new Vector3((float)num * num4, 0f, (float)num2 * num4));
			this.SpawnBoardX = this.beamOriginX / 1024;
			this.SpawnBoardZ = this.beamOriginZ / 1024;
			this.TargetBoardX = num / 1024;
			this.TargetBoardZ = num2 / 1024;
			this.SetTarget(null);
			for (int i = 0; i < this.ProjectileType.BeamInitialZeroes; i++)
			{
				this.AdvanceBeam();
			}
		}

		private void GetAccurateBoardLocation(SmartEntity entity, ref int x, ref int z)
		{
			x *= 1024;
			z *= 1024;
			if (entity != null && entity.SizeComp != null)
			{
				x += 1024 * entity.SizeComp.Width / 2;
				z += 1024 * entity.SizeComp.Depth / 2;
				return;
			}
			x += 512;
			z += 512;
		}

		public Bullet()
		{
		}

		protected internal Bullet(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).AddSplash((ISplashVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).AdvanceBeam();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).ApplyBeamDamagePercent((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).AppliedBeamHitFXThisSegment);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).AppliedBuffs);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).Cookie);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).FlashTarget);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).GunLocator);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).HealthFrag);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).IsDeflection);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).OwnerFaction);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).OwnerTeam);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).ProjectileType);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).SpawnBoardX);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).SpawnBoardZ);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).SpawnWorldLocation);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).SplashVO);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).TargetBoardX);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).TargetBoardZ);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).TargetWorldLocation);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).GetBeamDamagePercent(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).GetBeamNearbyCells());
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).InitWithTarget(*(int*)args, *(int*)(args + 1), *(*(IntPtr*)(args + 2)), (Target)GCHandledObjects.GCHandleToObject(args[3]), (TeamType)(*(int*)(args + 4)), (Entity)GCHandledObjects.GCHandleToObject(args[5]), (HealthFragment)GCHandledObjects.GCHandleToObject(args[6]), (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(args[7]), (List<Buff>)GCHandledObjects.GCHandleToObject(args[8]), (FactionType)(*(int*)(args + 9)), (GameObject)GCHandledObjects.GCHandleToObject(args[10])));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).InternalSetBeamSegment(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).IsBeamFirstHit((SmartEntity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Bullet)GCHandledObjects.GCHandleToObject(instance)).IsSplash());
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).AppliedBeamHitFXThisSegment = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).AppliedBuffs = (List<Buff>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).Cookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).FlashTarget = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).GunLocator = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).HealthFrag = (HealthFragment)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).IsDeflection = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).OwnerFaction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).OwnerTeam = (TeamType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).ProjectileType = (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).SpawnBoardX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).SpawnBoardZ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).SpawnWorldLocation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).SplashVO = (ISplashVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).Target = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).TargetBoardX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).TargetBoardZ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).TargetWorldLocation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).SetDefaultTargetWorldLocation(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).SetTarget((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).SetTargetWorldLocation(*(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((Bullet)GCHandledObjects.GCHandleToObject(instance)).SetupBeam();
			return -1L;
		}
	}
}
