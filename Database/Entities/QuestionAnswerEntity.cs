using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class QuestionAnswerEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long QuestionId { get; set; }
    public long AnswerId { get; set; }
    public int Rank { get; set; }
    public QuestionEntity Question { get; set; }
    public AnswerEntity Answer { get; set; }
}