using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Database.Seeds;

public static class SeedGenerator
{
    public const string DataFilename = "generated_training_data.jsonl";

    public static void GenerateSeedQuestionsSql(string filename)
    {
        var data = new List<Root>();
        using (StreamReader file = File.OpenText(filename))
        {
            string? line;
            while ((line = file.ReadLine()) != null)
            {
                data.Add(JsonConvert.DeserializeObject<Root>(line) ?? throw new InvalidOperationException());
            }
        }

        var questions = data.Select(_ => _.question).Distinct().ToList();
        var answers = data.Select(_ => _.answer).Distinct().ToList();
        Console.WriteLine(GenerateSqlString(questions.Slice(1,10), answers.Slice(1,10)));
    }

    private static string GenerateSqlString(List<string> questions, List<string> answers)
    {
        var sql = new System.Text.StringBuilder();
        sql.AppendLine("START TRANSACTION;");
        
        // Add questions
        sql.AppendLine("INSERT INTO questions (\"Question\")");
        sql.Append("VALUES ");
        foreach (var question in questions)
        {
            sql.AppendLine($"(\'{question.Replace("'", "''")}\'){(questions.IndexOf(question) != questions.Count-1 ? ',' : ';')}");
        }
        
        // Add Answers
        sql.AppendLine("INSERT INTO answers (\"Answer\")");
        sql.Append("VALUES ");
        foreach (var answer in answers)
        {
            sql.AppendLine($"(\'{answer.Replace("'", "''")}\'){(answers.IndexOf(answer) != answers.Count-1 ? ',' : ';')}");
        }

        // Add QuestionAnswers
        var random = new Random();
        
        var ranking = new Dictionary<long, List<long>>();
        foreach (var question in questions)
        {
            var ranks = Enumerable.Range(1, random.Next(1, 7))
                .Select(x =>
                {
                    long value = random.Next(1, answers.Count);
                    return value;
                })
                .ToList();  
            // Add to ranking
            var indexOf = questions.IndexOf(question)+1;
            ranking.Add(indexOf, ranks);
        }


        var keyValuePairs = ranking.ToList();
        foreach (var qa in keyValuePairs)
        {
            foreach (var rank in qa.Value)
            {
                sql.AppendLine("INSERT INTO \"QuestionAnswerEntity\" (\"QuestionId\", \"AnswerId\", \"Rank\")");
                sql.Append("VALUES ");
                sql.AppendLine($"({qa.Key}, {rank}, {qa.Value.IndexOf(rank)+1});");
                // sql.AppendLine($"(\'{answer}\'){(answers.IndexOf(answer) != answers.Count-1 ? ',' : ';')}");
            }
        }
        // Fin
        sql.AppendLine("COMMIT;");
        return sql.ToString();
    }
    
    internal class Metadata
    {
        public int answer_start { get; set; }
        public int answer_end { get; set; }
        public double ae_score { get; set; }
        public string qg_score { get; set; }
    }

    internal class Root
    {
        public string passage_id { get; set; }
        public string answer { get; set; }
        public string question { get; set; }
        public Metadata metadata { get; set; }
        public string question_id { get; set; }
    }
}