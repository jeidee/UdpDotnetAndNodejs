using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icet.Message.Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("사용법 :");
                Console.WriteLine("Icet.Message.Compiler.exe C2S.xml ./ cs");
                Console.WriteLine("- 파라미터");
                Console.WriteLine("- 첫번째 : 패킷정의 xml파일명(경로포함)");
                Console.WriteLine("- 두번째 : 출력경로");
                Console.WriteLine("- 세번째 : 출력언어(cs:C#, js:node.js)");
                Environment.Exit(0);
            }

            string protocol_file = args[0];
            string outpath = args[1];
            string outlang = args[2];

            Parser parser = new Parser();
            Protocol protocol = parser.Parse(protocol_file);

            if (outlang == "cs")
            {
                CsGenerator generator = new CsGenerator();
                generator.Generate(protocol, outpath);
            }
            else if (outlang == "js")
            {
                JsGenerator generator = new JsGenerator();
                generator.Generate(protocol, outpath);
            }

            Console.Write("끝!");
        }
    }
}
