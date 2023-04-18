using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System.Linq;

namespace OliviaGuard.Protector.Protections
{
    public class AntiDump
    {
        public AntiDump(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            var module = lib.moduleDef;
            var typeModule = ModuleDefMD.Load(typeof(Runtime.AntiDump).Module);
            var typeDefs = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.AntiDump).MetadataToken));
            var members = InjectHelper.Inject(typeDefs, module.GlobalType, module);

            var init = (MethodDef)members.Single(method => method.Name == "AntiDumpInj");
            MethodDef cctor = lib.ctor;
           // init.Name = Renamer.GenerateString();
            cctor.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(init));
        }
    }
}