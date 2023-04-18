using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;

namespace OliviaGuard.Protector.Protections
{
    public class AntiDe4dot
    {
        public AntiDe4dot(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            foreach (ModuleDef module in lib.moduleDef.Assembly.Modules)
            {
                for (int i = 0; i < 15; i++)
                {
                    TypeDefUser typeDefUser = new TypeDefUser(string.Empty, Renamer.GenerateString());
                    InterfaceImplUser item = new InterfaceImplUser(typeDefUser);
                    module.Types.Add(typeDefUser);
                    typeDefUser.Interfaces.Add(item);
                }
            }
            /* foreach (ModuleDef module in context.Module.Assembly.Modules)
             {
                 for (int i = 0; i < 1; i++)
                 {
                     string loo = System.IO.File.ReadAllText(@"C:\Users\Olivia\Desktop\Protections\SugarGuard-main\SugarGuard\SugarGuard\bin\Debug\mytext.txt");
                     TypeDefUser typeDefUser = new TypeDefUser(loo, loo);
                     InterfaceImplUser item = new InterfaceImplUser(typeDefUser);
                     module.Types.Add(typeDefUser);
                     typeDefUser.Interfaces.Add(item);
                 }
             }*/
        }
    }
}