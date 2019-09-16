using SqlUpdateManager.Core.Common;

namespace SqlUpdateManager.Core.Data
{
	public class AppConfigEntity : AbstractEntity
	{
		public override string HashPattern => $"{Name}";
		public AppConfig Config { get; set; }
		public override IEntity Clone() =>
			new AppConfigEntity
			{
				Hash = Hash == null ? null : (byte[])Hash.Clone(),
				Name = string.IsNullOrEmpty(Name) ? null : (string)Name.Clone()
			};
	}
}
