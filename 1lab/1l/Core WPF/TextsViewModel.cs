using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _1l
{
    internal class TextsViewModel
    {
        public static string ShowEncrypted(string text,string key)
        {
            return Texts.ParallelEncrypt(text, key);
        }
        public static string ShowDecrypted(string text,string key)
        {
            return Texts.ParallelDecrypt(text, key);
        }
    }
}
