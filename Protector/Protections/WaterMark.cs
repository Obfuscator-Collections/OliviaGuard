using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;

namespace OliviaGuard.Protector.Protections
{
    public class WaterMark
    {
        public WaterMark(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            foreach (var moduleDef in lib.moduleDef.Assembly.Modules)
            {
                var module = (ModuleDefMD)moduleDef;
                var attrRef = module.CorLibTypes.GetTypeRef("System", "Attribute");
                var attrType = new TypeDefUser("", "#OliviaGuard", attrRef);
                module.Types.Add(attrType);

                var ctor = new MethodDefUser(
                    ".ctor",
                    MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.String),
                    MethodImplAttributes.Managed,
                    MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName)
                {
                    Body = new CilBody()
                };
                TypeRef attrRef2 = lib.moduleDef.CorLibTypes.GetTypeRef("System", "Convert");
                var ctorRef2 = new MemberRefUser(lib.moduleDef, "ToString", MethodSig.CreateInstance(lib.moduleDef.CorLibTypes.String, lib.moduleDef.CorLibTypes.String), attrRef2);
                ctor.Body.MaxStack = 1;
                ctor.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
                ctor.Body.Instructions.Add(OpCodes.Call.ToInstruction(new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), attrRef)));
                ctor.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, "[This assembly protected by Olivia Guard Beta Version v1.3 - https://t.me/OliviaNetProtector]"));
                ctor.Body.Instructions.Add(Instruction.Create(OpCodes.Newobj, ctorRef2));
                ctor.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
                attrType.Methods.Add(ctor);
            }
            lib.moduleDef.Assembly.Name = "Olivia Guard";
            lib.moduleDef.Name = System.IO.File.ReadAllText("mytext.txt");
        }
    }
}