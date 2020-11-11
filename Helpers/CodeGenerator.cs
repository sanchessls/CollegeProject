using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Helpers
{
    public static class CodeGenerator
    {

        // define characters allowed in passcode.  set length so divisible into 256
        static char[] ValidChars = {'2','3','4','5','6','7','8','9',
                   'A','B','C','D','E','F','G','H',
                   'J','K','L','M','N','P','Q',
                   'R','S','T','U','V','W','X','Y','Z'}; // len=32

        const string hashkey = "password"; //key for HMAC function -- change!
        const int codelength = 6; // lenth of passcode

        public static string Generate(string unique)
        {
            string key = unique + DateTime.Now.ToString("yyyyMMddHHmmssfff");


            byte[] hash;
            using (HMACSHA1 sha1 = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(hashkey)))
                hash = sha1.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            int startpos = hash[hash.Length - 1] % (hash.Length - codelength);
            StringBuilder passbuilder = new StringBuilder();
            for (int i = startpos; i < startpos + codelength; i++)
                passbuilder.Append(ValidChars[hash[i] % ValidChars.Length]);
            return passbuilder.ToString();
        }
    }
}
