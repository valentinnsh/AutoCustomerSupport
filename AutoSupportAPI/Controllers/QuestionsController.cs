using AutoSupportAPI.Models;
using Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoSupportAPI.Controllers;

[Route("api/questions")]
public class QuestionsController
{
    [HttpGet("{questionId:long}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionByIdAsync([FromRoute] long questionId,
        [FromServices] PostgresDbContext context,
        CancellationToken token)
    {
        var question = await context.Questions.Where(q => q.Id == questionId)
            .Select(record => new QuestionInfo(record))
            .FirstOrDefaultAsync(token);
        if (question is null) return new NotFoundResult();
        return new OkObjectResult(question);
    }
}