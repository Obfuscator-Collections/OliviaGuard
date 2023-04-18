using OliviaGuard.Protector.Class;
using System;
using System.Linq;
using dnlib.DotNet;

namespace OliviaGuard.Protector.Protections
{
    public class Renamer
    {
        public Renamer(OliviaLib lib) => Main(lib);
        public static string InvisibleName { get { return "<OliviaGuard>"+ NewSs()+ "</OliviaGuard>"; } }
        public static string InvisibleX { get { return "<XXX>" + NewSs() + "</XXX>"; } }

        void Main(OliviaLib lib)
        {
            string old_type = null;
            foreach (ModuleDef module in lib.assembly.Modules)
            {
                foreach (TypeDef type in module.Types)
                {
                    if (type.IsPublic)
                        old_type = type.Name;

                    if (CanRename(type))
                        type.Name = InvisibleName;


                        foreach (MethodDef method in type.Methods)
                        {
                            if (CanRename(method))
                            {
                                if (!method.Name.StartsWith("<OliviaGuard>"))
                                {
                                    TypeRef attrRef = lib.moduleDef.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "CompilerGeneratedAttribute");
                                    var ctorRef = new MemberRefUser(lib.moduleDef, ".ctor", MethodSig.CreateInstance(lib.moduleDef.Import(typeof(void)).ToTypeSig()), attrRef);
                                    var attr = new CustomAttribute(ctorRef);
                                    method.CustomAttributes.Add(attr);
                                    method.Name = InvisibleName;
                                }
                            }

                            foreach (var param in method.Parameters)
                                param.Name = InvisibleName;
                        }
                    foreach (FieldDef field in type.Fields)
                        if (CanRename(field))
                            field.Name = InvisibleName;

                    foreach (EventDef eventf in type.Events)
                        if (CanRename(eventf))
                            eventf.Name = InvisibleName;
                    if (type.IsPublic)
                    {
                        foreach (Resource xxx in lib.moduleDef.Resources)
                        {
                            if (xxx.Name.Contains(old_type))
                                xxx.Name = xxx.Name.Replace(old_type, type.Name);
                        }
                    }
                }
            }
        }

        static Random rnd = new Random();

        public static string GenerateName()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz19242104215129503252351";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        public static string GenerateNumbersOnly()
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, rnd.Next(2,7)).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        public static string GenerateAnother()
        {
            return string.Format("<{0}>OliviaGuard</{1}>", GenerateName(), GenerateName());
        }
        public static string NewSs()
        {
            return GenerateNumbersOnly() + (Convert.ToInt64(new Random().Next(999999)) * 0x19660D + 0x3C6EF35).ToString("X");
        }
        public static string GenerateString()
        {
            int seed = rnd.Next();
            return "<OliviaGuard>" + (seed * 0x19660D + 0x3C6EF35).ToString("X") + "</OliviaGuard>";
        }
        #region CanRename
        static bool CanRename(TypeDef type)
        {
            if (type.IsGlobalModuleType)
                return false;
            try
            {
                if (type.Namespace.Contains("My"))
                    return false;
            }
            catch { }
            if (type.Interfaces.Count > 0)
                return false;
            if (type.IsSpecialName)
                return false;
            if (type.IsRuntimeSpecialName)
                return false;
            if (type.Name.Contains("Olivia"))
                return false;
            else
                return true;
        }

        static bool CanRename(MethodDef method)
        {
            if (method.IsConstructor)
                return false;
            if (method.DeclaringType.IsForwarder)
                return false;
            if (method.IsFamily)
                return false;
            if (method.IsConstructor || method.IsStaticConstructor)
                return false;
            if (method.IsRuntimeSpecialName)
                return false;
            if (method.DeclaringType.IsForwarder)
                return false;
            if (method.DeclaringType.IsGlobalModuleType)
                return false;
            if (method.Name.Contains("Olivia"))
                return false;
            else
                return true;
        }

        static bool CanRename(FieldDef field)
        {
            if (field.IsLiteral && field.DeclaringType.IsEnum)
                return false;
            if (field.DeclaringType.IsForwarder)
                return false;
            if (field.IsRuntimeSpecialName)
                return false;
            if (field.IsLiteral && field.DeclaringType.IsEnum)
                return false;
            if (field.Name.Contains("Olivia"))
                return false;
            else
                return true;
        }

        static bool CanRename(EventDef ev)
        {
            if (ev.DeclaringType.IsForwarder)
                return false;
            if (ev.IsRuntimeSpecialName)
                return false;
            else
                return true;
        }
        #endregion
    }
}
