using OliviaGuard.Protector.Class;
using System;
using System.Linq;
using dnlib.DotNet;

namespace OliviaGuard.Protector.Protections
{
    public class Renamer2
    {
        public Renamer2(OliviaLib lib) => Main(lib);


        void Main(OliviaLib lib)
        {
            foreach (var type in lib.moduleDef.GetTypes())
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody)
                        continue;
                    if (!method.Body.HasInstructions)
                        continue;

                    foreach (var paramDef in method.ParamDefs)
                        paramDef.Name = GenerateString();
                    foreach (var local in method.Body.Variables)
                        local.Name = GenerateString();

                    var declType = method.DeclaringType;

                    if (CanRename(declType, method) && !method.Name.StartsWith("<OliviaGuard>"))
                        method.Name = GenerateString();
                    if (CanRename(declType) && !declType.Name.StartsWith("<OliviaGuard>"))
                        declType.Name = GenerateString();
                    
                        if (!declType.Namespace.StartsWith("<OliviaGuard>"))
                            declType.Namespace = GenerateString();
                    

                    foreach (var field in declType.Fields)
                        if (CanRename(field))
                            field.Name = GenerateString();
                }

        }


        public static Random rnd = new Random();
        public static string GenerateString()
        {
            // int seed = rnd.Next();
            // return "<OliviaGuard>" +new Random().Next(99999) +(new Random().Next() * 0x19660D + 0x3C6EF35).ToString("X") + "</OliviaGuard>";
            return "<OliviaGuard>" + NewSS() + "</OliviaGuard>";
        }

        public static bool CanRename(FieldDef field)
        {
            if (field.IsSpecialName)
                return false;
            if (field.IsRuntimeSpecialName)
                return false;
            return true;
        }
        public static string GenerateNumbersOnly()
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, rnd.Next(2, 7)).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        public static string NewSS()
        {
            return GenerateNumbersOnly() + (Convert.ToInt64(new Random().Next()) * 0x19660D + 0x3C6EF35).ToString("X");
        }
        public static bool CanRename(TypeDef declType)
        {
            if (declType.IsGlobalModuleType)
                return false;
            if (declType.IsInterface)
                return false;
            if (declType.IsAbstract)
                return false;
            if (declType.IsEnum)
                return false;
            if (declType.IsRuntimeSpecialName)
                return false;
            if (declType.IsSpecialName)
                return false;
            return true;
        }

        public static bool CanRename(TypeDef declType, MethodDef method)
        {
            if (declType.IsInterface)
                return false;
            if (declType.IsDelegate || declType.IsAbstract)
                return false;

            if (method.IsSetter || method.IsGetter)
                return false;
            if (method.IsSpecialName || method.IsRuntimeSpecialName)
                return false;
            if (method.IsConstructor)
                return false;
            if (method.IsVirtual)
                return false;
            if (method.IsNative)
                return false;
            if (method.IsPinvokeImpl || method.IsUnmanaged || method.IsUnmanagedExport)
                return false;
            if (method.HasImplMap)
                return false;

            return true;
        }
    }
}
