using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SqlUpdateManager.Core.Data
{
	public sealed class StorageCollection<TEntity> : IEnumerable<TEntity>
		where TEntity : IEntity
	{
		private List<TEntity> _entities;
		private readonly JsonSerializer _serailizer;
		private readonly string _storagePath;

		public TEntity this[int index] =>
			_entities[index];

		internal StorageCollection(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("Collection path cannot be null or empty.");

			_storagePath = path;
			_serailizer = new JsonSerializer();
			_entities = new List<TEntity>();

			LoadData();
		}

		public IEnumerator<TEntity> GetEnumerator() =>
			_entities.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();

		public void Add(TEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("Entity cannot be null.");

			_entities.Add(entity);
		}

		public void AddRange(IEnumerable<TEntity> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("Collection cannot be null.");

			_entities.AddRange(collection);
		}

		public IEnumerable<TEntity> AsNoTracking(Func<TEntity, bool> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException("Predicate cannot be null.");

			var result = _entities.Where(e => predicate(e));
			var noTracking = new List<TEntity>();

			foreach (var entity in result)
				noTracking.Add((TEntity)entity.Clone());

			return noTracking;
		}

		public IEnumerable<TEntity> AsNoTracking() =>
			AsNoTracking(e => true);

		internal void SaveChanges()
		{
			if (_entities != null && _entities.Any())
			{
				var content = _serailizer.Serialize(_entities);
				FileManager.Save(content, _storagePath);
			}
		}

		private void LoadData()
		{
			var data = FileManager.Load(_storagePath);

			if (!string.IsNullOrEmpty(data))
			{
				var content = _serailizer.Deserialize<List<TEntity>>(data);

				if (content != null)
					_entities = content;
			}
		}
	}
}
