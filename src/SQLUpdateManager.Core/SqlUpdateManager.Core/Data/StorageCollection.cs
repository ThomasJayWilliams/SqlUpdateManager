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
		private readonly BsonSerializer _serailizer;
		private readonly string _storagePath;

		public TEntity this[int index] =>
			_entities[index];

		internal StorageCollection(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("Collection path cannot be null or empty.");

			_storagePath = path;
			_serailizer = new BsonSerializer();
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

			entity.Hash = Hasher.GetHash(entity.HashPattern);
			_entities.Add(entity);
		}

		public void AddRange(IEnumerable<TEntity> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("Collection cannot be null.");

			foreach (var item in collection)
				Add(item);
		}

		public IEnumerable<TEntity> AsNoTracking()
		{
			var noTracking = new List<TEntity>();

			foreach (var entity in _entities)
				noTracking.Add((TEntity)entity.Clone());

			return noTracking;
		}

		public void SaveChanges()
		{
			if (_entities != null && _entities.Any())
			{
				foreach (var item in _entities)
				{
					item.Hash = Hasher.GetHash(item.HashPattern);

					if (_entities.Any(e =>
						{
							if (ReferenceEquals(e, item))
								return false;

							return e.Hash.SequenceEqual(item.Hash);
						}))
					{
						throw new DuplicateException("Entity with the same data already exists.");
					}
				}

				FileProvider.Save(_entities, _storagePath);
			}
		}

		private void LoadData()
		{
			var data = FileProvider.Load<IEnumerable<TEntity>>(_storagePath);

			if (data != null)
				_entities = data.ToList();
		}
	}
}
