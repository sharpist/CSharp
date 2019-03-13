using Composite.Component;

namespace Composite.Composite
{
    // только объект Composite реализует этот интерфейс
    public interface IGiftOperations
    {
        void Add(GiftBase gift);
        void Remove(GiftBase gift);
    }
}
