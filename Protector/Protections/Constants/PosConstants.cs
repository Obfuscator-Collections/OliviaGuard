using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;

namespace OliviaGuard.Protector.Protections.Constants
{
    public class PosConstants
    {
        public PosConstants(OliviaLib lib)
        {
            Main(lib);
        }

        void Main(OliviaLib lib)
        {
            foreach (TypeDef type in lib.moduleDef.Types)
                foreach (MethodDef method in type.Methods)
                {
                    var encodedmethods = Constants.encodedMethods;
                    foreach (var emethod in encodedmethods)
                        if (emethod.eMethod == method)
                        {
                            method.Body.Instructions.Add(OpCodes.Ldstr.ToInstruction(emethod.eNum.ToString()));
                            method.Body.Instructions.Add(OpCodes.Pop.ToInstruction());
                            method.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
                        }
                }
        }
    }
}