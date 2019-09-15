using System;

namespace SqlUpdateManager.Core.Data
{
	public interface IEntity
	{
		string Name { get; set; }
		byte[] Hash { get; set; }

		IEntity Clone();
	}
}
