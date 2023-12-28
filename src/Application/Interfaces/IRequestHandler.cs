namespace Retryrr.Application;

public interface IRequestHandler
{
   public bool CanHandle(string request);

   public void Handle(string request);
}