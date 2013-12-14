﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC
{
    public class FecClasses
    {
        // A disambiguated representation of a person in the FEC data set.
        // Individual records have been linked together to the same contributor
        // ID if their metadata (e.g. contributor name and address) were considered
        // to be similar enough to have been generated by the same person.
        public class DisambiguatedId
        {
            // This is a unique identifier for the constructed ID
            public String id_name;

            // This contains a list of the records that represent a single ID
            public List<ContributionRecord> id_list;
        }

        public class ContributionRecord
        {
            // We require a custom converter to handle exceptions where
            // employer_normalized is sometimes represented in JSON as -1.
            // Some moron gave us data that violated the spec, so we handle
            // it in the converter by setting EmployerNormalized to null
            // if the JSON is -1.
            [JsonConverter(typeof(EmployerNormalizedConverter))]
            public class EmployerNormalized
            {
                public string company;
                public List<int> code;
            }

            public class EmployerNormalizedConverter : JsonConverter
            {
                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    // If we are reading an employer_normalized entry whose value is "-1", then we set the value of the entry
                    // to null. Otherwise we create an employer_normalized entry with the normal values.
                    if (reader.TokenType == JsonToken.Integer)
                    {
                        return null;
                    }

                    // Load JObject from stream
                    JObject jObject = JObject.Load(reader);

                    // Create target object based on JObject
                    EmployerNormalized target = new EmployerNormalized();

                    // Populate the object properties
                    serializer.Populate(jObject.CreateReader(), target);

                    return target;
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }

                public override bool CanConvert(Type objectType)
                {
                    throw new NotImplementedException();
                }
            }

            public class OccupationNormalized
            {
                public string occupation_normalized_string;

                // The FEC specification says this property should be a single string,
                // but forensic analysis of the file shows that the property is sometimes
                // a single string, sometimes an array of strings, and sometimes -1.
                // As such, we need a special converter for this field.
                [JsonConverter(typeof(OccupationNormalizedIscoCodeConverter))]
                public List<string> isco_code;
            }

            public class OccupationNormalizedIscoCodeConverter : JsonConverter
            {
                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    // If we are reading an occupation_normalized entry whose isco_code value is "-1",
                    // then we set the value of the entry to null.
                    if (reader.TokenType == JsonToken.Integer)
                    {
                        return null;
                    }
                    // If we encounter a single string, convert it to a list and return the list
                    if (reader.TokenType == JsonToken.String)
                    {
                        string t = serializer.Deserialize<string>(reader);
                        return new List<string>(new[] { t });
                    }
                    // If we encounter a list of strings, return the list of strings
                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        return serializer.Deserialize<List<string>>(reader);
                    }

                    return null;
                }

                public override bool CanConvert(Type objectType)
                {
                    throw new NotImplementedException();
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }
            }

            public class NameNormalized
            {
                public string last;
                public List<string> names;
                public List<string> middle;
                public List<string> titles;
                public string gender;
            }


            public EmployerNormalized employer_normalized;
            public OccupationNormalized occupation_normalized;
            public NameNormalized name_normalized;

            public string amendment_code;
            public double amount;
            public string committee_id;
            public int cycle;
            public DateTime date;
            public int day;
            public string file;
            public int? file_id;
            public string memo_code;
            public string memo_text;
            public string microfilm_location;
            public int month;
            public string primary_general_indicator;
            public string record_id;
            public string report_type;

            public string transaction_id;
            public string transaction_namespace;
            public string transaction_type;

            public int year;
            public string zip_5;

            public string contributor_employer;
            public string contributor_entity_type;
            public string contributor_first_name;
            public string contributor_last_name;
            public string contributor_middle_name;
            public string contributor_name;
            public string contributor_occupation;
            public string contributor_prefix;
            public string contributor_state;
            public string contributor_city;
            public string contributor_street_one;
            public string contributor_state_two;
            public string contributor_zipcode;
            public string contributor_zipcode_full;
            public string contributor;

            public string contributor_candidate_city;
            public int? contributor_candidate_election_year;
            public string contributor_candidate_id;
            public string contributor_candidate_id_merge;
            public string contributor_candidate_incumbent_challenger_status;
            public string contributor_candidate_name;
            public string contributor_candidate_office;
            public string contributor_candidate_office_district;
            public string contributor_candidate_office_state;
            public string contributor_candidate_party;
            public string contributor_candidate_state;
            public string contributor_candidate_status;
            public string contributor_candidate_street_one;
            public string contributor_candidate_street_two;
            public string contributor_candidate_zipcode;

            public string contributor_committee_city;
            public string contributor_committee_connected_org_name;
            public string contributor_committee_desgination;
            public string contributor_committee_filing_frequency;
            public string contributor_committee_id;
            public string contributor_committee_interest_group_category;
            public string contributor_committee_name;
            public string contributor_committee_party;
            public string contributor_committee_state;
            public string contributor_committee_street_one;
            public string contributor_committee_street_two;
            public string contributor_committee_treasurer;
            public string contributor_committee_type;
            public string contributor_committee_zipcode;

            public string recipient_candidate_city;
            public string recipient_candidate_election_year;
            public string recipient_candidate_id;
            public string recipient_candidate_id_merge;
            public string recipient_candidate_incumbent_challenger_status;
            public string recipient_candidate_name;
            public string recipient_candidate_office;
            public string recipient_candidate_office_distrinct;
            public string recipient_candidate_office_state;
            public string recipient_candidate_party;
            public string recipient_candidate_primary_committee_id;
            public string recipient_candidate_state;
            public string recipient_candidate_status;
            public string recipient_candidate_street_one;
            public string recipient_candidate_street_two;
            public string recipient_candidate_zipcode;

            public string recipient_committee_city;
            public string recipient_committee_connected_org_name;
            public string recipient_committee_designation;
            public string recipient_committee_filing_frequency;
            public string recipient_committee_id;
            public string recipient_committee_name;
            public string recipient_committee_party;
            public string recipient_committee_state;
            public string recipient_committee_street_one;
            public string recipient_committee_street_two;
            public string recipient_committee_treasurer;
            public string recipient_committee_type;
            public string recipient_committee_zipcode;
        }
    }
}

