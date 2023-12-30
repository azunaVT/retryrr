namespace Retryrr.Application;

public interface IRequestHandler : IComparable
{
   public int Priority { get; }

   public bool CanHandle(string request);

   public void Handle(string request);
}