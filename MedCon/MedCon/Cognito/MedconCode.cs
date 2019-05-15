using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MedCon.Code
{
    public class MedconCode
    {
        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "customerId")]
        public int CustomerId { get; private set; }

        [JsonProperty(PropertyName = "trialId")]
        public int TrialId { get; private set; }

        [JsonProperty(PropertyName = "containerId")]
        public int ContainerId { get; private set; }

        MedconCode(int version, int customer, int trial, int container)
        {
            Version = version;
            CustomerId = customer;
            TrialId = trial;
            ContainerId = container;
        }

        static readonly string PatternVersion = @"MEDCON\/(?<version>\d+)";
        static readonly string PatternVersion0 = @"MEDCON\/(?<version>\d+)\/(?<customer>\d+)\/(?<trial>\d+)\/(?<container>\d+)";

        public static MedconCode FromString(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return null;
            }

            var versionMatch = Regex.Match(data, PatternVersion);

            if (versionMatch.Success)
            {
                var version = Convert.ToInt32(versionMatch.Groups["version"].Captures[0].Value);

                switch (version)
                {
                    case 0:
                        var match = Regex.Match(data, PatternVersion0);
                        if (match.Success)
                        {
                            return new MedconCode(version, Convert.ToInt32(match.Groups["customer"].Value), Convert.ToInt32(match.Groups["trial"].Value), Convert.ToInt32(match.Groups["container"].Value));
                        }
                        break;
                    default:
                        Debug.WriteLine("Unhandled version ({0})", version);
                        break;
                }
            }

            return null;
        }

        public string ToBarcodeString()
        {
            return $"MEDCON/{Version}/{CustomerId}/{TrialId}/{ContainerId}";
        }

        public override string ToString()
        {
            return string.Format("[MedconCode: Version={0}, CustomerId={1}, TrialId={2}, ContainerId={3}]", Version, CustomerId, TrialId, ContainerId);
        }
    }
}
