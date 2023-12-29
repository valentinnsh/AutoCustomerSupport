using Database.Seeds;

namespace AutoSupportAPI.Test;

public class GenerateSeed
{
    [Test]
    public void GenerateSeedSql()
    {
        SeedGenerator.GenerateSeedQuestionsSql("/home/valentine/dev/AutoCustomerSupport/Data/generated_training_data.jsonl");
    }
}