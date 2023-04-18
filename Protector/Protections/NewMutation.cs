using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using OliviaGuard.Protector.Protections.Mutation.Stages;
using System.Linq;

namespace OliviaGuard.Protector.Protections
{
    public class NewMutation
    {
        public NewMutation(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            ModuleDef module = lib.moduleDef;
            foreach (TypeDef type in module.GetTypes())
            {
               // if (type.IsGlobalModuleType) continue;
                foreach (MethodDef method in type.Methods)
                {
                    if (!canMutateMethod(method)) continue;
                    //var locals = new LocalsToCustomLocal(method);
                    // var IntsReplacer = new IntsReplacer(method);
                    var maths = new IntsToMath(method);
                    // var alloc = new IntsToStackalloc(method);
                    var randoms = new IntsToRandom(method);
                    var IntsConversions = new IntsConversions(method);
                   // var IntsToMethodPointer= new IntsToMethodPointer(method);
                   //var IntsToArray = new IntsToArray(method);   
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i].IsLdcI4() && MutationHelper.CanObfuscate(method.Body.Instructions, i))
                        {
                           // randoms.Execute(ref i);
                            maths.Execute(ref i);
                          //  IntsConversions.Execute();
                            //  randoms.Execute(ref i);
                            //  IntsReplacer.Execute();
                        }
                    }
                }
            }
        }
        public bool canMutateMethod(MethodDef method)
        {
            return method.HasBody && method.Body.HasInstructions;
        }
    }
}
