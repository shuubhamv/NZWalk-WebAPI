namespace NZWalk.Api.Models.EmailConfiguration
{
   
   public record EmailWorkItem(string Email, string Subject, string Message, bool IsHtml = false);
}
