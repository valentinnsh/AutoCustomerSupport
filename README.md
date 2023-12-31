# AutoCustomerSupport

## Project description
This project is a task that I did as part of the "Non-relational Databases" course to compare SQL and key-value solutions in creating a task-answer system with ranking. I interpreted it as follows - there is a pool of questions and answers in a customer support system. One question (problem) may have one answer (problem solution) with a certain probability. The questions and answers are stored in a knowledge base, and I compared how PostgreSQL and Redis perform in this task.

The application includes a controller with two identical endpoints in theory - one "answers" the user's question by retrieving data from PostgreSQL, and the other from Redis.
### "Ranking"

In PostgreSQL, I implemented a ranking system using an additional table called QuestionAnswers, which has a Rank field. The lower the number, the higher the rank of the answer to a specific question.
In Redis, I had to sacrifice the idea of non-duplicating data, and I simply store everything in a hash table where the key is the question and the value is a JSON string containing a list of answers.
## Data

More-or-less "belivable" data is taken from - https://github.com/maxbartolo/improving-qa-model-robustness. I didn't want to 
generate data as random strings, since it did not look that good. ~450000 unique questions and ~400000 unique answers should be enough.

The data is parsed and processed by the SeedGenerator class, after which the database seed scripts are saved in the Data folder. In the repository, I have included a portion of 1000 questions and answers. If you want to reproduce it in its entirety, you can run the GenerateSeeds unit test and it will create complete files similar to what I tested on.
## Results

On average, Redis performed faster than Postgres as it stores all data in memory and queries to it do not require joins.
The average result (out of 100 runs) for 16 asynchronous requests to endpoints is presented in the table.

| Database | Endpoint response time(ms) |
|----------|----------------------------|
| PG       | 25.4                       |
| Redis    | 15.9                       |