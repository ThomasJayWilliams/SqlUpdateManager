using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace SQLUpdateManager.CLI.Common
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IDataRepository _repository;

        public ConfigurationManager(IDataRepository repository)
        {
            _repository = repository;
        }

        public AppConfig GetConfig(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} does not exist.");

            return _repository.GetData<AppConfig>(path);
        }

        public void UpdateConfig(AppConfig config, string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} does not exist.");

            _repository.WriteData(path, config);
        }

        public void UpdateConfig(string category, string property, string value, string path)
        {
            var config = _repository.GetData<AppConfig>(path);
            var categories = GetCategoryNames();
            var properties = GetPropertyNames();

            if (!categories.Contains(category))
                throw new InvalidArgumentException(ErrorCodes.InvalidArgument,
                    $"{category} category is not found. Use {Constants.ConfigCommand} command with {Constants.ListParameter} parameter to get available properties.");

            if (!properties.Contains(property))
                throw new InvalidArgumentException(ErrorCodes.InvalidArgument,
                    $"{property} property is not found. Use {Constants.ConfigCommand} command with {Constants.ListParameter} parameter to get available properties.");

            UpdateInstance(config, category, property, value);

            _repository.WriteData(path, config);
        }

        public IEnumerable<string> GetStringConfig(string path)
        {
            var config = _repository.GetData<AppConfig>(path);

            return new List<string>
            {
                $"{GetAttributeValue<AppConfig, CoreConfig>(c => c.Core)}.{GetAttributeValue<CoreConfig, string>(c => c.FileEncoding)}={config.Core.FileEncoding}"
            };
        }

        private IEnumerable<string> GetCategoryNames() =>
            new List<string>
            {
                GetAttributeValue<AppConfig, CoreConfig>(c => c.Core)
            };

        private IEnumerable<string> GetPropertyNames() =>
            new List<string>
            {
                GetAttributeValue<CoreConfig, string>(c => c.FileEncoding)
            };

        private string GetAttributeValue<TObj, TProperty>(Expression<Func<TObj, TProperty>> value)
        {
            var memberExpression = value.Body as MemberExpression;
            var attr = memberExpression.Member.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
            return ((JsonPropertyAttribute)attr[0]).PropertyName;
        }

        private void UpdateInstance(AppConfig config, string category, string property, string value)
        {
            if (category == GetAttributeValue<AppConfig, CoreConfig>(c => c.Core))
            {
                if (property == GetAttributeValue<CoreConfig, string>(c => c.FileEncoding))
                    config.Core.FileEncoding = value;
            }
        }
    }
}
