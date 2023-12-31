using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Database.Seeds;

public static class SeedGenerator
{
    public static void GenerateAndWriteSeedScriptsToFile(string filename, bool test = true)
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

        var (sql, redis) = GenerateSeedStrings(test ? data.Slice(1,1000) : data);
        const string sqlFilepath = $"/home/valentine/dev/AutoCustomerSupport/Data/seed_postgres_test.sql";
        using (var writer = new StreamWriter(sqlFilepath)) writer.Write(sql);
        
        const string redisFilepath = $"/home/valentine/dev/AutoCustomerSupport/Data/seed_redis_test.txt";
        using (var writer = new StreamWriter(redisFilepath)) writer.Write(redis);
    }

    private static (string,string) GenerateSeedStrings(List<Root> data)
    {
        var sql = new System.Text.StringBuilder();
        var redis = new System.Text.StringBuilder();
        sql.AppendLine("START TRANSACTION;");
        
        var questions = new Dictionary<long, string>();
        var answers = new Dictionary<long, string>();

        foreach (var record in data)
        {
            if (questions.TryAdd(data.IndexOf(record), record.question))
                sql.AppendLine($"INSERT INTO questions (\"Question\") VALUES (\'{record.question.Replace("'", "''")}\');");
            if (answers.TryAdd(data.IndexOf(record), record.answer))
                sql.AppendLine($"INSERT INTO answers (\"Answer\") VALUES (\'{record.answer.Replace("'", "''")}\');");

        }

        // Add QuestionAnswers
        var random = new Random();
        
        var ranking = new Dictionary<(long, string), List<long>>();
        foreach (var question in questions)
        {
            var ranks = Enumerable.Range(1, random.Next(1, 7))
                .Select(x =>
                {
                    long value = random.Next(1, answers.Count);
                    return value;
                })
                .ToList();
            ranking.Add((question.Key, question.Value), ranks);
        }


        var keyValuePairs = ranking.ToList();
        foreach (var qa in keyValuePairs)
        {
            foreach (var rank in qa.Value)
            {
                sql.AppendLine("INSERT INTO \"QuestionAnswerEntity\" (\"QuestionId\", \"AnswerId\", \"Rank\")");
                sql.Append("VALUES ");
                sql.AppendLine($"({qa.Key.Item1+1}, {rank}, {qa.Value.IndexOf(rank)+1});");
            }

            var ans = answers
                .Where(kv => qa.Value.Contains(kv.Key))
                .OrderBy(kv => qa.Value.IndexOf(kv.Key))
                .Select(kv => $"\"{kv.Value.Replace("'", "")}\"")
                .ToList();
            redis.AppendLine($"hset questions '{qa.Key.Item2.Replace("'", "")}' '[{string.Join(", ", ans)}]'");
        }
        // Fin
        sql.AppendLine("COMMIT;");
        return (sql.ToString(), redis.ToString());
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