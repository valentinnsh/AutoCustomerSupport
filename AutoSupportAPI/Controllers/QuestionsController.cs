using AutoSupportAPI.Models;
using AutoSupportAPI.Services;
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
        [FromServices] IQuestionService service,
        CancellationToken token)
    {
        var question = await service.GetAnswersAsync(questionId, token);
        if (question is null) return new NotFoundResult();
        return new OkObjectResult(question);
    }
}