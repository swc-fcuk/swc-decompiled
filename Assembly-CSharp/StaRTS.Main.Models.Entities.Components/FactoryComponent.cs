using Net.RichardLord.Ash.Core;
using System;

namespace StaRTS.Main.Models.Entities.Components
{
	public class FactoryComponent : ComponentBase
	{
		public const string SPARK_FX_ID = "effect203";

		public FactoryComponent()
		{
		}

		protected internal FactoryComponent(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
