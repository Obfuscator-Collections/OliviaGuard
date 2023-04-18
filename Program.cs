using OliviaGuard.Protector;
using OliviaGuard.Protector.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OliviaGuard
{
    class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var Target = @"C:\Users\MBA\Desktop\V2\bin\Debug\Mac.exe";//args[0];
                string filename = Path.GetFileNameWithoutExtension(Target).Replace(".exe", "");
                Olivia OLV = new Olivia(Target);
                //OLV.protections.Add(Protections.AntiDebug);
                //OLV.protections.Add(Protections.AntiDebugFirst);
                OLV.protections.Add(Protections.AntiDump);

                OLV.protections.Add(Protections.CallConvertion);
                // OLV.protections.Add(Protections.AntiDebuggerz);
                OLV.protections.Add(Protections.Constants);
                OLV.protections.Add(Protections.PosConstants);
                OLV.protections.Add(Protections.ControlFlow);
                // OLV.protections.Add(Protections.JControlFlow);
                OLV.protections.Add(Protections.MethodHider);
                OLV.protections.Add(Protections.Renamer);
                 //OLV.protections.Add(Protections.ImportantProtection);
                //    OLV.protections.Add(Protections.AntiDe4dot);
                OLV.protections.Add(Protections.AntiDebugFirst);
                  OLV.protections.Add(Protections.ConstantMelt);
                OLV.protections.Add(Protections.StringToArray);
                OLV.protections.Add(Protections.NewMutation);
                OLV.protections.Add(Protections.StackConfusion);
                OLV.protections.Add(Protections.HideNameSpaces);
                //  OLV.protections.Add(Protections.AntiDebug);
                //OLV.protections.Add(Protections.JControlFlow);
                OLV.protections.Add(Protections.OpCodeProtection);

                OLV.protections.Add(Protections.HideMethods);
                OLV.protections.Add(Protections.Renamer2);

                OLV.protections.Add(Protections.InvalidOpcodes);
                OLV.protections.Add(Protections.WaterMark);
                //OLV.protections.Add(Protections.HideMethods);

                //   OLV.protections.Add(Protections.ReferenceProxy);

                OLV.Protect();

                OLV.Save();
                string loka = OLV.GoldName;
                //   MessageBox.Show(Target.Replace(".exe", ".exe_Olivied").Replace("../../", @"c:\").Replace("/", @"\"));

                Olivia OLVX = new Olivia(@"C:\Users\Olivia\source\repos\Olivia Al-Maliki Activation System\Olivia Al-Maliki Activation System\bin\Debug\mac_Olivied.exe");//Target.Replace(".exe", ".exe_Olivied").Replace("../../", @"c:\").Replace("/", @"\"));
                OLVX.GoldName = loka;
                OLVX.GoldKey = OLV.GoldKey;
                OLVX.GoldResource = OLV.GoldResource;
                OLVX.protections.Add(Protections.ProAntiTamper);
                OLVX.Protect();
                OLVX.Save();


                //   OLV.protections.Add(Protections.Renamer);
                var NEWPath = Target.Replace("../../", @"c:\").Replace("/", @"\").Replace(filename + ".exe.bytes", "") + filename + ".exe" + "_" + "Olivied" + Path.GetExtension(Target);
                if (System.IO.File.Exists(NEWPath.Replace(".bytes", "_Olivied.bytes")))
                {
                    // Process.Start(@"C:\Users\Ola\Desktop\OliviaGuardv1\OliviaGuard.exe ", "\"" + NEWPath + "\"");
                    Console.WriteLine(NEWPath.Replace(".bytes", "_Olivied.bytes"));

                }
            }//+ Path.GetFileNameWithoutExtension(Target) + "_" + "Olivied" + Path.GetExtension(Target);
            /*if (System.IO.File.Exists(NEWPath))
            {
                Process.Start(@"C:\Users\Ola\Desktop\OliviaGuardv1\OliviaGuard.exe " , "\"" +NEWPath  + "\"");
                Console.WriteLine(NEWPath.Replace(".exe", "-Olivia.exe"));

            }
            
            Thread.Sleep(3000);*/

            catch (Exception ex) { }
            }//Console.WriteLine(""); } 
           // Process.Start(@"C:\Users\Olivia\Desktop\Debug\OliviaGuard.exe " + Target);
           //   Application.EnableVisualStyles();
           // Application.Run(new Main());
            }
    }

