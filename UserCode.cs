using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ArchiveTask
{
    public class Archive
    {
        private Dictionary<DateTime, List<Guid>> ArchiveOperatinons;

        public Archive(string[] serializedOperations)
        {
            ArchiveOperatinons = new Dictionary<DateTime, List<Guid>>();
            foreach (var operation in serializedOperations)
            {
                var deserializedOperation = JsonConvert.DeserializeObject<Operation>(operation);
                if (ArchiveOperatinons.ContainsKey(deserializedOperation.Time))
                {
                    ArchiveOperatinons[deserializedOperation.Time].Add(deserializedOperation.OperationId);
                }
                else
                {
                    ArchiveOperatinons[deserializedOperation.Time] = new List<Guid>() { deserializedOperation.OperationId };
                }
            }
        }

        public Guid[] GetOperationIds(string time)
        {
            var formattedTime = DateTime.Parse(time, null,
                                              DateTimeStyles.RoundtripKind);
            if (ArchiveOperatinons.ContainsKey(formattedTime))
            {
                return ArchiveOperatinons[formattedTime].ToArray();
            }
            else
                return new Guid[0];
        }

        private struct Operation
        {
            public Guid OperationId;
            public DateTime Time;
        }
    }
}