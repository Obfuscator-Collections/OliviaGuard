using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;

namespace OliviaGuard.Protector.Protections
{
    public class HideNameSpace
    {
        public HideNameSpace(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            foreach (TypeDef typeDef in lib.moduleDef.Types)
            {

                typeDef.Namespace = "";
            }
        }
    }
}