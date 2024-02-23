using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string a_z = "abcdefghijklmnopqrstuvwxyz";
        public string Encrypt(string plainText, int key)
        {
            //throw new NotImplementedException();
            string C = "";

            foreach (char P in plainText)
            {
                int P_index = a_z.IndexOf(P);
                int C_index = (P_index + key) % 26;
                C += a_z[C_index];
            }
            return C;
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            string P_T = "";

            foreach (char C in cipherText)
            {
                int C_index = a_z.IndexOf(C);
                int P_index = (C_index - key) % 26;
                if (P_index < 0)
                {
                    P_index += 26;
                }
                P_T += a_z[P_index];
            }
            return P_T;
        }

        public int Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
    }
}
