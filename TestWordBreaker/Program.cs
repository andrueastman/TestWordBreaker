using System.Runtime.InteropServices;

namespace TestWordBreaker
{
    internal partial class Program
    {
        static class WordBreaker
        {
            [DllImport("RustWordBreaker", EntryPoint = "tokenize", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr tokenize([MarshalAs(UnmanagedType.LPUTF8Str)] string input);
        }
        static void Main(string[] args)
        {
            var cases = new string[] { "Hello, World!", "결정하겠다", "아버지가방에들어가신다" };
            var expected = new int[] { 4, 4, 6 };
            for(int i = 0; i < cases.Length; i++)
            {
                var tokenPtr = WordBreaker.tokenize(cases[i]);
                var strings = PtrToStringArray(tokenPtr);
                Console.WriteLine($"Case: {i+1}");
                Console.WriteLine($"Expected: {expected[i]}");
                Console.WriteLine($"Actual: {strings.Length}");
                Console.WriteLine();
            }
        }

        private static string[] PtrToStringArray(IntPtr ptr)
        {
            var strings = new System.Collections.Generic.List<string>();
            while (true)
            {
                IntPtr strPtr = Marshal.ReadIntPtr(ptr);
                if (strPtr == IntPtr.Zero)
                    break;
                strings.Add(Marshal.PtrToStringAnsi(strPtr));
                ptr = IntPtr.Add(ptr, IntPtr.Size);
            }
            return strings.ToArray();
        }
    }
}
