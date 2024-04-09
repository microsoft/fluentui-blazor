#nullable disable
namespace FluentUI.Demo.Shared.SampleData;

// Represents the data returned by https://open.fda.gov/apis/food/enforcement/
// This is a subset of the fields available on that API
public class FoodRecall
{
    public string Event_Id { get; set; }
    public string Status { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Recalling_Firm { get; set; }
}

public class FoodRecallQueryResult
{
    public Metadata Meta { get; set; }
    public FoodRecall[] Results { get; set; }

    public class Metadata
    {
        public ResultsInfo Results { get; set; }
    }

    public class ResultsInfo
    {
        public int Total { get; set; }
    }
}

#nullable enable
