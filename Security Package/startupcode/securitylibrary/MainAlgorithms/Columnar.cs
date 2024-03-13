using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        static List<List<int>> Permutations(List<int> keys)
        {
            List<List<int>> result = new List<List<int>>();
            Permute(keys, 0, keys.Count - 1, result);
            return result;
        }

        static void Permute(List<int> keys, int left, int right, List<List<int>> result)
        {
            if (left == right)
            {
                result.Add(new List<int>(keys));
            }
            else
            {
                for (int i = left; i <= right; i++)
                {
                    int temp = keys[left];
                    keys[left] = keys[i];
                    keys[i] = temp;
                    Permute(keys, left + 1, right, result);
                    temp = keys[left];
                    keys[left] = keys[i];
                    keys[i] = temp;
                }
            }
        }


        public List<int> Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            int Len_CT = cipherText.Length;

            int Num_of_rows, Num_of_columns = 0;
            for (int i = 2; i < 8; i++)
            {
                if (Len_CT % i == 0)
                {
                    Num_of_columns = i;
                }
            }

            Num_of_rows = cipherText.Length / Num_of_columns;
            char[,] plain = new char[Num_of_rows, Num_of_columns];
            char[,] cipher = new char[Num_of_rows, Num_of_columns];
            List<int> key = new List<int>(Num_of_columns);

            int counter = 0;
            for (int i = 0; i < Num_of_rows; i++)
            {
                for (int j = 0; j < Num_of_columns; j++)
                {
                    if (counter < plainText.Length)
                    {
                        plain[i, j] = plainText[counter];
                        counter++;
                    }

                }
            }

            counter = 0;
            for (int i = 0; i < Num_of_columns; i++)
            {
                for (int j = 0; j < Num_of_rows; j++)
                {
                    if (counter < cipherText.Length)
                    {
                        cipher[j, i] = cipherText[counter];
                        counter++;
                    }
                }
            }

            int index = 0;
            for (int i = 0; i < Num_of_columns; i++)
            {
                for (int k = 0; k < Num_of_columns; k++)
                {
                    for (int j = 0; j < Num_of_rows; j++)
                    {
                        if (plain[j, i] == cipher[j, k])
                        {
                            index++;
                        }
                        if (index == Num_of_rows)
                        {
                            key.Add(k + 1);
                        }
                    }
                    index = 0;
                }
            }
            if (key.Count == 0) //using bruteforce
            {
                List<int> keys = new List<int>();
                for (int range = 1; range < plainText.Length; range++)
                {
                    int keyRange = range;
                    keys = new List<int>();
                    for (int i = 1; i <= keyRange; i++)
                    {
                        keys.Add(i);
                    }

                    List<List<int>> permutations = Permutations(keys);
                    //1, 4, 3, 2
                    //int count = 0;
                    //foreach (List<int> permutation in permutations)
                    //{
                    //    if (permutation[0] == 1 &&
                    //        permutation[1] == 4 &&
                    //        permutation[2] == 3 &&
                    //        permutation[3] == 2 
                    //      )
                    //    {
                    //        break;
                    //    }
                    //    count++;
                    //}
                    for (int p = 0; p < permutations.Count; p++)
                    {

                        string testCipher = Encrypt(plainText, permutations[p]);

                        //string finaTest="";
                        //analysis 1
                        //"ttnaaptmtsuoaodwcoixknlxpetx"
                        //"ttnaaptmtsuoaodwcoiknlpet"
                        //foreach (char tt in testCipher)
                        //{
                        //    if (tt != 'x')
                        //        finaTest += tt;
                        //}
                        string testCipherUpper = testCipher.ToUpper();
                        bool isequal = true;

                        for (int i = 0; i < cipherText.Length; i++)
                        {
                            if (cipherText[i] != testCipher[i])
                            {
                                //
                                //C.T="cusnpremeieotcc"
                                isequal = false;
                                break;
                            }
                        }
                        if (isequal)
                        {
                            return permutations[p];
                        }
                        for (int i = 0; i < cipherText.Length; i++)
                        {

                            if (cipherText[i] != testCipherUpper[i])
                            {
                                isequal = false;
                                break;
                            }

                        }
                        if (isequal)
                        {
                            return permutations[p];
                        }
                    }
                }
            }
            //if (key.Count == 0)
            //{
            //    for (int i = 0; i < Num_of_columns; i++)
            //    {
            //        key.Add(0);
            //    }
            //}
            return key;
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
//int count = 0;
//foreach (List<int> permutation in permutations)
//{
//    if (permutation[0] == 3 &&
//        permutation[1] == 2 &&
//        permutation[2] == 6 &&
//        permutation[3] == 4 &&
//        permutation[4] == 1 &&
//        permutation[5] == 5)
//    {
//        break;
//    }
//    count++;
//}