using CsvHelper;
using CsvHelper.Configuration;
using Infrastructure.Csv.Interface;
using Infrastructure.Utils;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Infrastructure.Csv.CsvHelper
{
    public class CsvHelperImp<TRecord, TMap> : ICsv<TRecord>
        where TMap : ClassMap<TRecord>
    {

        public CsvHelperImp() : this(Encoding.Default) { }

        public CsvHelperImp(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public void Write(string path, IEnumerable<TRecord> records, bool hasHeaderRecord = true)
        {
            FileSystemHelper.CreateDirectory(path);

            using (var csv = new CsvWriter(new StreamWriter(path, true, encoding)))
            {
                csv.Configuration.HasHeaderRecord = hasHeaderRecord;
                csv.Configuration.RegisterClassMap<TMap>();

                csv.WriteRecords(records);

                csv.Flush();
            }
        }

        public IEnumerable<TRecord> Read(string path)
        {
            using (var csv = new CsvReader(new StreamReader(path, encoding)))
            {
                csv.Configuration.RegisterClassMap<TMap>();

                return csv.GetRecords<TRecord>().ToArray();
            }
        }

        private Encoding encoding;
    }
}
