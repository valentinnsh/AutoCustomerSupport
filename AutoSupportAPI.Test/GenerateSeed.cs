using Database.Seeds;

namespace AutoSupportAPI.Test;

public class GenerateSeed
{
    [Test]
    public void GenerateSeedSql()
    {
        const string dataJsonl = "/home/valentine/dev/AutoCustomerSupport/Data/generated_training_data.jsonl";
        SeedGenerator.GenerateAndWriteSeedScriptsToFile(dataJsonl, true);
    }
}