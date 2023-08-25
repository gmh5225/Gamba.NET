using LLVMSharp.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gamba.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public class TokenOutput
    {
        [FieldOffset(0)]
        public byte type; //0x0000
        [FieldOffset(1)]
        public IntPtr str; //0x0001
        [FieldOffset(9)]
        public int precedence; //0x0009
        [FieldOffset(13)]
        public bool rightAssociative; //0x000D
        [FieldOffset(14)]
        public bool unary; //0x000E
        [FieldOffset(15)]
        public int argIndex; //0x000F
    }

    public class ShuttingYardParser
    {
        public unsafe static IReadOnlyList<TokenOutput> ShuttingYard(string input)
        {
            var sw = Stopwatch.StartNew();
            nint tokensPtr = 11111;
            nint varNamesPtr = 11111;

            ShuttingYardExport(new MarshaledString(input), new MarshaledString("x,y,z"), ref tokensPtr);
            var tokens = new ManagedVector<TokenOutput>(tokensPtr, (x => Marshal.PtrToStructure<TokenOutput>(x)));
            sw.Stop();

            Console.WriteLine($"Took {sw.ElapsedMilliseconds}ms.");
            return tokens.Items;
            
            //var inputS = new MarshaledString(input);
           // ShuttingYardExport(inputS, 0, 0);
            //return (null, null);
        }

        [DllImport("Gamba.Native")]
        public unsafe static extern void ShuttingYardExport(sbyte* strExpr, sbyte* names, ref nint tokensPtr);
    }
}
