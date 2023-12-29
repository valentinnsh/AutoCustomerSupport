using System.Runtime.Serialization;
using Database.Entities;

namespace AutoSupportAPI.Models;

[DataContract]
public class QuestionInfo
{
    [DataMember]
    public long Id { get; set; }
    
    [DataMember] 
    public string Question { get; set; }
    
    [DataMember]
    public List<string> Answers { get; set; }
}