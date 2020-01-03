
using CryptSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Servicos
{
    public static class Criptografia
    {
        public static string Codifica(string senha)
        {
            return Crypter.Blowfish.Crypt(senha);
        }

        public static bool Compara(string senha, string hash)
        {
            return Crypter.CheckPassword(senha, hash);
        }
    }
}