// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUI.Demo.SampleData;

/// <summary>
/// The URL https://api.fda.gov/food/enforcement.json is an endpoint for the openFDA API,
/// specifically for accessing data on food enforcement reports.
/// This API provides information about food product recalls and enforcement actions taken by the FDA.
/// </summary>
public class RemoteFoodProductRecalls
{
    private const string FDA_API = "https://api.fda.gov/food/enforcement.json";
    private static readonly HttpClient _client = new HttpClient();

    /// <summary>
    /// Options for JSON serialization and deserialization.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Returns a list of food product recalls.
    /// </summary>
    /// <param name="skip"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public static async Task<FoodProductRecalls> GetFoodProductRecallsAsync(int skip = 0, int limit = 30)
    {
        var response = await _client.GetAsync($"{FDA_API}?skip={skip}&limit={limit}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FoodProductRecalls>(json, JsonOptions) ?? new();
    }

    /// <summary />
    public class FoodProductRecalls
    {
        /// <summary />
        public Meta Meta { get; set; } = new();

        /// <summary />
        public IEnumerable<FoodProduct> Results { get; set; } = Array.Empty<FoodProduct>();
    }

    /// <summary />
    public class FoodProduct
    {
        /// <summary />
        public string? Country { get; set; }
        /// <summary />
        public string? City { get; set; }
        /// <summary />
        public string? Address_1 { get; set; }
        /// <summary />
        public string? Reason_For_Recall { get; set; }
        /// <summary />
        public string? Address_2 { get; set; }
        /// <summary />
        public string? Product_Quantity { get; set; }
        /// <summary />
        public string? Code_Info { get; set; }
        /// <summary />
        public string? Distribution_Pattern { get; set; }
        /// <summary />
        public string? State { get; set; }
        /// <summary />
        public string? Product_Description { get; set; }
        /// <summary />
        public string? Report_Date { get; set; }
        /// <summary />
        public string? Classification { get; set; }
        /// <summary />
        public string? Recalling_Firm { get; set; }
        /// <summary />
        public string? Recall_Number { get; set; }
        /// <summary />
        public string? Product_Type { get; set; }
        /// <summary />
        public string? Event_Id { get; set; }
        /// <summary />
        public string? Recall_Initiation_Date { get; set; }
        /// <summary />
        public string? Postal_Code { get; set; }
        /// <summary />
        public string? Voluntary_mandated { get; set; }
        /// <summary />
        public string? Status { get; set; }
    }

    /// <summary />
    public class Meta
    {
        /// <summary />
        public string? Last_Updated { get; set; }
        /// <summary />
        public MetaResults Results { get; set; } = new();
    }

    /// <summary />
    public class MetaResults
    {
        /// <summary />
        public int Skip { get; set; }
        /// <summary />
        public int Limit { get; set; }
        /// <summary />
        public int Total { get; set; }
    }
}

#nullable disable
/// <summary>
/// Represents the data returned by https://open.fda.gov/apis/food/enforcement/
/// This is a subset of the fields available on that API
/// </summary>
public class FoodRecall
{
    /// <summary />
    public string Event_Id { get; set; }
    /// <summary />
    public string Status { get; set; }
    /// <summary />
    public string City { get; set; }
    /// <summary />
    public string State { get; set; }
    /// <summary />
    public string Recalling_Firm { get; set; }
    /// <summary />
    public string Termination_Date { get; set; }
}

/// <summary>
/// Represents the result of a food recall query from the FDA API.
/// </summary>
public class FoodRecallQueryResult
{
    /// <summary />
    public Metadata Meta { get; set; }
    /// <summary />
    public FoodRecall[] Results { get; set; }

    /// <summary />
    public class Metadata
    {
        /// <summary />
        public ResultsInfo Results { get; set; }
    }

    /// <summary />
    public class ResultsInfo
    {
        /// <summary />
        public int Total { get; set; }
    }
}

#nullable enable
