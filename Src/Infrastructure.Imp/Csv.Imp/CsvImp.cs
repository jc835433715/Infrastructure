using Infrastructure.Csv.Interface;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Infrastructure.Csv.Imp
{
    public class CsvImp : ICsv
    {
        public void Write(string path, IEnumerable<string> head, IEnumerable<string> records)
        {
            try
            {
                FileSystemHelper.CreateDirectory(path);

                if (!FileSystemHelper.IsFileExists(path))
                {
                    File.WriteAllLines(path, new string[] { ConvertCsvFormat(head) }, Encoding.Default);
                }

                File.AppendAllText(path, ConvertCsvFormat(records) + Environment.NewLine, Encoding.Default);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"保存Csv文件错误", e);
            }
        }

        public IEnumerable<string[]> Read(string path)
        {
            return File.ReadAllLines(path).Skip(1).Select(r => ConvertCsvFormat(r));
        }

        private string ConvertCsvFormat(IEnumerable<string> values)
        {
            var result = new StringBuilder();

            values.ToList().ForEach(value =>
            {
                value = value ?? string.Empty;
                value = value.Replace("\"", "\"\"");

                if (value.Any(c => c == ',' || c == '"' || c == '\n'))
                {
                    result.AppendFormat("\"{0}\"", value);
                }
                else
                {
                    result.Append(value);
                }

                result.Append(',');
            });

            return result.ToString().TrimEnd(',');
        }

        private string[] ConvertCsvFormat(string value)
        {
            //TODO:待完善
            return value.Split(',');
        }
    }
}
