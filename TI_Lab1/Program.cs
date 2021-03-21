using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI_Lab1
{
    class Program
    {
        public static string RailFenceEncrypt(string InputText, int Key)
        {
            string Result = "";
            char[,] CipherMatrix = new char[Key, InputText.Length];

            for (int j = 0; j < InputText.Length;)
            {
                for (int i = 0; i < Key && j < InputText.Length; i++, j++)
                {
                    CipherMatrix[i, j] = InputText[j];
                }
                for (int i = Key - 2; i > 0 && j < InputText.Length; i--, j++)
                {
                    CipherMatrix[i, j] = InputText[j];
                }
            }

            for (int i = 0; i < Key; i++)
                for (int j = 0; j < InputText.Length; j++)
                {
                    if (CipherMatrix[i, j] != '\0')
                    {
                        Result += CipherMatrix[i, j];
                    }
                }
            return Result;
        }

        public static string RailFenceDecrypt(string CipherText, int Key)
        {
            string Result = "";
            char[,] CipherMatrix = new char[Key, CipherText.Length];

            int index = 0;
            for (int i = 0; i < Key; i++)
            {
                bool FirstPart = true;
                int j = i;
                int tmp = 2 * (Key - i) - 2;
                while (j < CipherText.Length)
                {
                    CipherMatrix[i, j] = CipherText[index];
                    if (FirstPart)
                    {
                        tmp = 2 * (Key - i) - 2;
                        if (tmp <= 0)
                            tmp = 2 * Key - 2;
                        j += tmp;
                    }
                    else
                    {
                        tmp = 2 * i;
                        if (tmp <= 0)
                            tmp = 2 * Key - 2;
                        j += tmp;
                    }
                    FirstPart = !FirstPart;
                    index++;
                }
            }

            for (int j = 0; j < CipherText.Length; j++)
                for (int i = 0; i < Key; i++)
                {
                    if (CipherMatrix[i, j] != '\0')
                    {
                        Result += CipherMatrix[i, j];
                    }
                }
            return Result;
        }

        public static string ColumnEncrypt(string InputText, string Key)
        {
            string Result = "";
            int Row = (int)(InputText.Length / Key.Length) + 2;
            int[] IndexMatrix = new int[Key.Length];
            char[,] CipherMatrix = new char[Row, Key.Length];

            for (int j = 0; j < Key.Length; j++)
                CipherMatrix[0, j] = Key[j];

            int index = 0;
            for (int i = 1; i < Row && index < InputText.Length; i++)
                for (int j = 0; j < Key.Length && index < InputText.Length; j++)
                {
                    CipherMatrix[i, j] = InputText[index];
                    index++;
                }

            for (int i = 0; i < Key.Length; i++)
                IndexMatrix[i] = 1;
            for (int i = 0; i < Key.Length; i++)
            {
                for (int j = i + 1; j < Key.Length; j++)
                {
                    if (CipherMatrix[0, i] <= CipherMatrix[0, j])
                        IndexMatrix[j]++;
                    else IndexMatrix[i]++;
                }
            }

            int Min = 1, ind = 0;
            for (int j = 0; j < Key.Length; j++)
            {
                for (ind = 0; ind < Key.Length; ind++)
                    if (IndexMatrix[ind] == Min)
                    {
                        Min++;
                        break;
                    }
                for (int i = 1; i < Row; i++)
                {
                    if (CipherMatrix[i, ind] != '\0')
                    {
                        Result += CipherMatrix[i, ind];
                    }
                }
            }
            return Result;
        }

        public static string ColumnDecrypt(string CipherText, string Key)
        {
            string Result = "";
            int Row = (int)(CipherText.Length / Key.Length) + 2;
            char[,] CipherMatrix = new char[Row, Key.Length];
            int[] IndexMatrix = new int[Key.Length];
            int Mod = CipherText.Length % Key.Length;

            for (int j = 0; j < Key.Length; j++)
                CipherMatrix[0, j] = Key[j];

            for (int i = 0; i < Key.Length; i++)
                IndexMatrix[i] = 1;
            for (int i = 0; i < Key.Length; i++)
            {
                for (int j = i + 1; j < Key.Length; j++)
                {
                    if (CipherMatrix[0, i] <= CipherMatrix[0, j])
                        IndexMatrix[j]++;
                    else IndexMatrix[i]++;
                }
            }

            int index = 0, Min = 1, ind = 0;
            for (int j = 0; j < Key.Length; j++)
            {
                for (ind = 0; ind < Key.Length; ind++)
                    if (IndexMatrix[ind] == Min)
                    {
                        Min++;
                        break;
                    }
                for (int i = 1; i < ((ind < Mod) ? Row : Row - 1); i++)
                {
                    CipherMatrix[i, ind] = CipherText[index];
                    index++;
                }
            }

            for (int i = 1; i < Row; i++)
                for (int j = 0; j < Key.Length; j++)
                    if (CipherMatrix[i, j] != '\0')
                    {
                        Result += CipherMatrix[i, j];
                    }

            return Result;
        }

        public static string RotatingLatticeEncrypt(string InputText, bool [,] Lattice)
        {
            string Result = "";
            int i, j, k, Ind = 0;

            while (InputText.Length % 25 != 0)
                InputText += " ";

            char[,] CipherMatrix = new char[5, 5];
            int NumOfBlocks = InputText.Length / 25;

            for (k = 0; k < NumOfBlocks; k++)
            {
                for (i = 0; i < 5; i++)
                {
                    for (j = 0; j < 5; j++)
                    {
                        if (Lattice[i, j])
                            CipherMatrix[i, j] = InputText[Ind++];
                    }
                }
                Lattice[2, 2] = false;
                for (j = 0; j < 5; j++)
                {
                    for (i = 4; i >= 0; i--)
                    {
                        if (Lattice[i, j])
                            CipherMatrix[j, 4 - i] = InputText[Ind++];
                    }
                }
                for (i = 4; i >= 0; i--)
                {
                    for (j = 4; j >= 0; j--)
                    {
                        if (Lattice[i, j])
                            CipherMatrix[4 - i, 4 - j] = InputText[Ind++];
                    }
                }

                for (j = 4; j >= 0; j--)
                {
                    for (i = 0; i < 5; i++)
                    {
                        if (Lattice[i, j])
                            CipherMatrix[4 - j, i] = InputText[Ind++];
                    }
                }
                Lattice[2, 2] = true;

                for (i = 0; i < 5; i++)
                {
                    for (j = 0; j < 5; j++)
                    {
                        Result += CipherMatrix[i, j];
                    }
                }
            }

            return Result;
        }

        public static string RotatingLatticeDecrypt(string CipherText, bool [,] Lattice)
        {
            string Result = "";
            int i, j, k, Ind = 0;
            char[,] CipherMatrix = new char[5, 5];
            int NumOfBlocks = CipherText.Length / 25;

            for (k = 0; k < NumOfBlocks; k++)
            {
                for (i = 0; i < 5; i++)
                {
                    for (j = 0; j < 5; j++)
                    {
                        CipherMatrix[i, j] = CipherText[Ind++];
                    }
                }
                for (i = 0; i < 5; i++)
                {
                    for (j = 0; j < 5; j++)
                    {
                        if (Lattice[i, j])
                            Result += CipherMatrix[i, j];
                    }
                }
                Lattice[2, 2] = false;
                for (j = 0; j < 5; j++)
                {
                    for (i = 4; i >= 0; i--)
                    {
                        if (Lattice[i, j])
                            Result += CipherMatrix[j, 4 - i];
                    }
                }
                for (i = 4; i >= 0; i--)
                {
                    for (j = 4; j >= 0; j--)
                    {
                        if (Lattice[i, j])
                            Result += CipherMatrix[4 - i, 4 - j];
                    }
                }

                for (j = 4; j >= 0; j--)
                {
                    for (i = 0; i < 5; i++)
                    {
                        if (Lattice[i, j])
                            Result += CipherMatrix[4 - j, i];
                    }
                }
                Lattice[2, 2] = true;
            }

            return Result;
        }

        public static string CaesarEncrypt(string InputText, int key)
        {
            string Result = "";

            for (int i = 0; i < InputText.Length; i++)
            {
                if (InputText[i] >= 'A' && InputText[i] <= 'Z')
                    Result += (char)((InputText[i] + key - 'A') % 26 + 'A');
                else if (InputText[i] >= 'a' && InputText[i] <= 'z')
                    Result += (char)((InputText[i] + key - 'a') % 26 + 'a');
                else if (InputText[i] >= 'А' && InputText[i] <= 'Я')
                    Result += (char)((InputText[i] + key - 'А') % 33 + 'А');
                else if (InputText[i] >= 'а' && InputText[i] <= 'я')
                    Result += (char)((InputText[i] + key - 'а') % 33 + 'а');
                else
                    Result += InputText[i];
            }
            return Result;
        }

        public static string CaesarDecrypt(string CipherText, int key)
        {
            string Result = "";

            for (int i = 0; i < CipherText.Length; i++)
            {
                if (CipherText[i] >= 'A' && CipherText[i] <= 'Z')
                    Result += (char)((26 + ((CipherText[i] - 'A') - (key % 26))) % 26 + 'A');
                else if (CipherText[i] >= 'a' && CipherText[i] <= 'z')
                    Result += (char)((26 + ((CipherText[i] - 'a') - (key % 26))) % 26 + 'a');
                else if (CipherText[i] >= 'А' && CipherText[i] <= 'Я')
                    Result += (char)((33 + ((CipherText[i] - 'А') - (key % 33))) % 33 + 'А');
                else if (CipherText[i] >= 'а' && CipherText[i] <= 'я')
                    Result += (char)((33 + ((CipherText[i] - 'а') - (key % 33))) % 33 + 'а');
                else
                    Result += CipherText[i];
            }
            return Result;
        }
        static void Main(string[] args)
        {
            int method, crypt, RailFenceKey, CaesarKey;
            while (true)
            {
                Console.WriteLine("Select the encryption type: \n 1. Rail fence method \n 2. Column method \n 3. Rotating grid nethod \n 4. Caesar cipher");
                method = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Do you want to encrypt your text (press \"1\") or decrypt (press \"2\")?)");
                crypt = Convert.ToInt32(Console.ReadLine());
                switch (method)
                {
                    case 1:
                        Console.WriteLine("Enter your text: ");
                        string RailFenceText = Console.ReadLine();
                        Console.WriteLine("Enter the key: ");
                        RailFenceKey = Convert.ToInt32(Console.ReadLine());

                        if (crypt == 1)
                        {
                            string EncryptText = RailFenceEncrypt(RailFenceText, RailFenceKey);

                            Console.WriteLine("The result of encryption: ");
                            Console.WriteLine(EncryptText);
                        }
                        else if (crypt == 2)
                        {
                            string DecryptText = RailFenceDecrypt(RailFenceText, RailFenceKey);

                            Console.WriteLine("The result of decryption: ");
                            Console.WriteLine(DecryptText);
                        }
                        else Console.WriteLine("You didn't choose encryption / decryption");
                        break;

                    case 2:
                        Console.WriteLine("Enter your text: ");
                        string ColumnText = Console.ReadLine();
                        Console.WriteLine("Enter the key: ");
                        string ColumnKey = Console.ReadLine();

                        if (crypt == 1)
                        {
                            string EncryptText = ColumnEncrypt(ColumnText, ColumnKey);

                            Console.WriteLine("The result of encryption: ");
                            Console.WriteLine(EncryptText);
                        }
                        else if (crypt == 2)
                        {
                            string DecryptText = ColumnDecrypt(ColumnText, ColumnKey);

                            Console.WriteLine("The result of decryption: ");
                            Console.WriteLine(DecryptText);
                        }
                        else Console.WriteLine("You didn't choose encryption / decryption");
                        break;

                    case 3:
                        Console.WriteLine("Enter your text: ");
                        string RotatingLatticeText = Console.ReadLine();
                        bool IsOK = false;
                        bool[,] RotatingLatticeKey = new bool[5, 5];
                        bool[,] CheckingMatrix = new bool[5, 5];
                        do
                        {
                            Console.WriteLine("Enter the key (using \"1\" and \"0\", 25 symbols): ");
                            string tmp = Console.ReadLine();
                            int index = 0;
                            for (int i = 0; i < 5; i++)
                                for (int j = 0; j < 5; j++)
                                {
                                    if (tmp[index] == '0')
                                        RotatingLatticeKey[i, j] = false;
                                    else if (tmp[index] == '1') 
                                        RotatingLatticeKey[i, j] = true;
                                    index++;
                                }

                            for (int i = 0; i < 5; i++)
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    CheckingMatrix[i, j] = RotatingLatticeKey[i, j];
                                }
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    if (!CheckingMatrix[i, j])
                                        CheckingMatrix[i, j] = RotatingLatticeKey[4 - i, 4 - j];
                                    if (CheckingMatrix[i, j] && RotatingLatticeKey[4 - i, 4 - j])
                                        IsOK = false;
                                }
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                for (int j = 0; j <= 0; j++)
                                {
                                    if (!CheckingMatrix[i, j])
                                        CheckingMatrix[i, j] = RotatingLatticeKey[j, i];
                                    if (CheckingMatrix[i, j] && RotatingLatticeKey[j, i])
                                        IsOK = false;
                                }
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                for (int j = 0; j <= 0; j++)
                                {
                                    if (!CheckingMatrix[i, j])
                                        CheckingMatrix[i, j] = RotatingLatticeKey[4 - j, 4 - i];
                                    if (CheckingMatrix[i, j] && RotatingLatticeKey[4 - j, 4 - i])
                                        IsOK = false;
                                }
                            }

                            for (int i = 0; i < 5; i++)
                                for (int j = 0; j < 5; j++)
                                    if (CheckingMatrix[i, j] == false)
                                        IsOK = false;
                        } while (!IsOK);

                        if (crypt == 1)
                        {
                            string EncryptText = RotatingLatticeEncrypt(RotatingLatticeText, RotatingLatticeKey);

                            Console.WriteLine("The result of encryption: ");
                            Console.WriteLine(EncryptText);
                        }
                        else if (crypt == 2)
                        {
                            string DecryptText = RotatingLatticeDecrypt(RotatingLatticeText, RotatingLatticeKey);

                            Console.WriteLine("The result of decryption: ");
                            Console.WriteLine(DecryptText);
                        }
                        else Console.WriteLine("You didn't choose encryption / decryption");
                        break;

                    case 4:
                        Console.WriteLine("Enter your text: ");
                        string CaesarText = Console.ReadLine();
                        Console.WriteLine("Enter the key: ");
                        CaesarKey = Convert.ToInt32(Console.ReadLine());

                        if (crypt == 1)
                        {
                            string EncryptText = CaesarEncrypt(CaesarText, CaesarKey);

                            Console.WriteLine("The result of encryption: ");
                            Console.WriteLine(EncryptText);
                        }
                        else if (crypt == 2)
                        {
                            string DecryptText = CaesarDecrypt(CaesarText, CaesarKey);

                            Console.WriteLine("The result of decryption: ");
                            Console.WriteLine(DecryptText);
                        }
                        else Console.WriteLine("You didn't choose encryption / decryption");
                        break;

                    default:
                        Console.WriteLine("You didn't choose the method of encryption");
                        break;
                }
            }
            //int SourceKey;

            //Console.WriteLine("Enter your text: ");
            //string SourceText = Console.ReadLine();
            //Console.WriteLine("Enter the key: ");
            //SourceKey = Convert.ToInt32(Console.ReadLine());
            //string EncryptText = RailFenceEncrypt(SourceText, SourceKey);

            //Console.WriteLine("The text: ");
            //Console.WriteLine(EncryptText);

            //string Res = RailFenceDecrypt(EncryptText, SourceKey);

            //Console.WriteLine("The text: ");
            //Console.WriteLine(Res);

            //Console.WriteLine("Enter the key: ");
            //string StrKey = Console.ReadLine();

            //string Res2 = ColumnEncrypt(SourceText, StrKey);
            //Console.WriteLine(Res2);
            //string Res3 = ColumnDecrypt(Res2, StrKey);
            //Console.WriteLine(Res3);

            //Console.ReadKey();
        }
    }
}
