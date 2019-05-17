using Infrastructure.Config.Interface;
using Infrastructure.Serialization.Interface;
using System;
using System.IO;

namespace Infrastructure.Config.Yaml
{
    public class ConfigYaml<TConfigInfo> : IConfig<TConfigInfo>
        where TConfigInfo : class, new()
    {

        static ConfigYaml()
        {
            converter = new Infrastructure.Serialization.YamlDotNet.YamlConverter();
        }

        public ConfigYaml()
        {
            var basePath = $"{ AppConfigHelper.GetCurrentDomainData(ConfigConst.AppDataDirectory)}";

            if (string.IsNullOrEmpty(basePath))
            {
                basePath = Environment.CurrentDirectory;
            }

            this.Path = $@"{basePath}\{typeof(TConfigInfo).Name.Replace("ConfigInfo", string.Empty)}.yml";
        }

        public string Path { get; set; }

        public TConfigInfo Read()
        {
            var result = new TConfigInfo();

            try
            {
                var value = File.ReadAllText(Path);

                result = converter.Deserialize<TConfigInfo>(value);
            }
            catch (Exception e)
            {
                throw new ApplicationException("配置信息读取失败", e);
            }

            return result;
        }

        public void Write(TConfigInfo configInfo)
        {
            try
            {
                var value = converter.Serialize(configInfo);

                File.WriteAllText(Path, value);
            }
            catch (Exception e)
            {
                throw new ApplicationException("配置信息写入失败", e);
            }
        }

        private static IConverter converter;
    }
}
