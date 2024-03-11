using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
      
        public List<int> Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            // throw new NotImplementedException();
            int Len_key = key.Count;
            int Len_CT = cipherText.Length;
            int Num_of_columns = Len_CT / Len_key;

            if (Len_CT % Len_key != 0)
            {
                Len_CT+= Len_key;
            }

            int k = 0,index = 0;
            char[,] P_matrix = new char[Num_of_columns, Len_key];

            for (int i = 0; i <Len_key ; i++)
            {
                k = key.IndexOf(i + 1);
                for (int j = 0; j < Num_of_columns ; j++)
                {
                    if (index < Len_CT)
                    {
                        P_matrix[j, k] = cipherText[index];
                        index++;
                    }
                }
            }
            string PT = "";
            for (int i = 0; i < Num_of_columns; i++)
            {
                for (int j = 0; j <Len_key; j++)
                {
                    PT += P_matrix[i, j];
                }
            }
            return PT;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            //throw new NotImplementedException();
            int Len_key =key.Count;
            int Len_PT = plainText.Length;
            int Num_of_rows = Len_PT / Len_key;

            if (Len_PT % Len_key != 0)
            {
                Num_of_rows += 1;
            }

            int size_of_matrix = Num_of_rows * Len_key;
            if (Len_PT != size_of_matrix)
            {
                int x = size_of_matrix - Len_PT;
                for (int i = 0; i < x; i++)
                {
                    plainText += "x";
                }
            }

            char[,] C_matrix = new char[Num_of_rows, Len_key]; ;

            int index = 0;
            for (int i = 0; i < Num_of_rows; i++)
            {
                for (int j = 0; j < Len_key; j++)
                {
                    C_matrix[i, j] = plainText[index];
                    index++;
                }
            }

            Dictionary<int, string> CT_order = new Dictionary<int, string>();
            string s;
            foreach (int k in key)
            {
                s = "";
                for (int j = 0; j < Num_of_rows; j++)
                {
                    s += C_matrix[j, k - 1];
                }
                CT_order[key[k - 1]] = s;
            }

            string CT = "";
            for (int i = 1; i <= CT_order.Count; i++)
            {
                CT += CT_order[i];
            }
            return CT;
        }
    }
}
