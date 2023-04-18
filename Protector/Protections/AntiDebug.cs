using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System.Linq;

namespace OliviaGuard.Protector.Protections
{
    public class AntiDebug
    {
        public AntiDebug(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            var module = lib.moduleDef;
            var typeModule = ModuleDefMD.Load(typeof(Runtime.AntiDebug).Module);
            var typeDefs = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.AntiDebug).MetadataToken));
            var members = InjectHelper.Inject(typeDefs, module.GlobalType, module);

            var init = (MethodDef)members.Single(method => method.Name == "Initialize");
            var entrypoint = lib.ctor;

            entrypoint.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(init));
        }
    }
}
