using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class SelectedBuildingScreen : ClosableScreen, IEventObserver
	{
		protected const string BUILDING_PERKS_TITLE_GROUP = "TitleGroupPerks";

		protected const string BUILDING_TITLE_GROUP = "TitleGroup";

		protected const string BUILDING_PERKS_BUTTON = "btnPerks";

		protected const string FRAGMENT_ICON = "SpriteIconFragmentUpgrade";

		private const string FRAGMENT_SPRITE_FORMAT = "icoDataFragQ{0}";

		protected Entity selectedBuilding;

		protected BuildingTypeVO buildingInfo;

		protected UXSprite fragmentSprite;

		protected SelectedBuildingScreen(string assetName, Entity selectedBuilding) : base(assetName)
		{
			this.SetSelectedBuilding(selectedBuilding);
		}

		protected override void OnScreenLoaded()
		{
			this.fragmentSprite = base.GetElement<UXSprite>("SpriteIconFragmentUpgrade");
			this.fragmentSprite.Visible = false;
			base.OnScreenLoaded();
		}

		protected void SetupFragmentSprite(int quality)
		{
			UXUtils.SetupFragmentIconSprite(this.fragmentSprite, quality);
		}

		protected override void InitButtons()
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.ActivePerksUpdated, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.EntityDestroyed, EventPriority.Default);
			base.InitButtons();
			this.SetupPerksButton();
		}

		protected virtual void SetupPerksButton()
		{
			UXElement element = base.GetElement<UXElement>("TitleGroup");
			element.Visible = true;
			UXElement element2 = base.GetElement<UXElement>("TitleGroupPerks");
			element2.Visible = false;
			UXButton element3 = base.GetElement<UXButton>("btnPerks");
			element3.Tag = this.buildingInfo;
			element3.OnClicked = new UXButtonClickedDelegate(this.OnPerksButtonClicked);
			if (Service.Get<PerkManager>().IsPerkAppliedToBuilding(this.buildingInfo))
			{
				element2.Visible = true;
				element.Visible = false;
			}
		}

		protected void OnPerksButtonClicked(UXButton button)
		{
			Service.Get<PerkViewController>().ShowActivePerksScreen((BuildingTypeVO)button.Tag);
		}

		public override void OnDestroyElement()
		{
			this.UnregisterEvents();
			base.OnDestroyElement();
		}

		private void UnregisterEvents()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ActivePerksUpdated);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.EntityDestroyed);
		}

		protected virtual void SetSelectedBuilding(Entity newSelectedBuilding)
		{
			this.selectedBuilding = newSelectedBuilding;
			this.buildingInfo = ((this.selectedBuilding == null) ? null : this.selectedBuilding.Get<BuildingComponent>().BuildingType);
		}

		public override EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.EntityDestroyed)
			{
				if (id == EventId.ActivePerksUpdated)
				{
					this.SetupPerksButton();
				}
			}
			else if (this.selectedBuilding != null && this.selectedBuilding.ID == (uint)cookie)
			{
				this.UnregisterEvents();
				this.selectedBuilding = Service.Get<BuildingController>().SelectedBuilding;
				base.CloseNoTransition(null);
			}
			return base.OnEvent(id, cookie);
		}

		protected internal SelectedBuildingScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).InitButtons();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnPerksButtonClicked((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).SetSelectedBuilding((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).SetupFragmentSprite(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).SetupPerksButton();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SelectedBuildingScreen)GCHandledObjects.GCHandleToObject(instance)).UnregisterEvents();
			return -1L;
		}
	}
}
