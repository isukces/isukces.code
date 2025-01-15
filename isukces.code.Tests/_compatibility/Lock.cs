#if NET9_0_OR_GREATER
#else
namespace System.Threading
{
    // fake class
    internal class Lock
    {
        public Scope EnterScope() {
            throw new NotImplementedException();
        }
        public ref struct Scope:IDisposable {
            public void Dispose() {
                throw new NotImplementedException();
            }
        }
    }
}

#endif
