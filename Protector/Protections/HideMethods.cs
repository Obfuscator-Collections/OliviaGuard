using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;

namespace OliviaGuard.Protector.Protections
{
    public class HideMethods
    {
        public HideMethods(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            TypeRef attrRef = lib.moduleDef.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "CompilerGeneratedAttribute");
            var ctorRef = new MemberRefUser(lib.moduleDef, ".ctor", MethodSig.CreateInstance(lib.moduleDef.CorLibTypes.Void), attrRef);
            var attr = new CustomAttribute(ctorRef);

            TypeRef attrRef2 = lib.moduleDef.CorLibTypes.GetTypeRef("System", "Convert");
            var ctorRef2 = new MemberRefUser(lib.moduleDef, "ToString", MethodSig.CreateInstance(lib.moduleDef.CorLibTypes.String, lib.moduleDef.CorLibTypes.String), attrRef2);

            foreach (var type in lib.moduleDef.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (method.IsRuntimeSpecialName || method.IsSpecialName || method.Name == "Invoke") continue;
                    method.CustomAttributes.Add(attr);
                    //if (type.Name != lib.moduleDef.GlobalType.Name)
                    if (!method.Name.StartsWith("<OliviaGuard>"))
                        method.Name = Renamer.InvisibleName + Renamer.NewSs();//+ method.Name;
                }
            }

            var methImplFlags = MethodImplAttributes.IL | MethodImplAttributes.Managed;
            var methFlags = MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot;
            var meth1 = new MethodDefUser("Main",
                        MethodSig.CreateStatic(lib.moduleDef.CorLibTypes.Void, lib.moduleDef.CorLibTypes.String),
                        methImplFlags, methFlags);
            lib.moduleDef.EntryPoint.DeclaringType.Methods.Add(meth1);
            var body = new CilBody();
            meth1.Body = body;
            meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, "[This assembly protected by Olivia Guard Beta Version - https://t.me/OliviaNetProtector]"));
            meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Newobj, ctorRef2));
            meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Throw));
        }
    }
}