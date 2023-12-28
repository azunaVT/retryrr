namespace Retryrr.Application;

public interface IRetryrrService
{
   public bool RegisterHandler(IRequestHandler handler);

   public Task DoSomething(CancellationToken cancellationToken);
}
