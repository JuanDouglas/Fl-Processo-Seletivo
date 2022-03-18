using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models.Atributos
{
    public sealed class CPFAttribute : ValidationAttribute
    {
        public CPFAttribute()
        {
            ErrorMessage = "O CPF informado não é válido!";
        }
        public override bool IsValid(object value)
        {
            value = value ?? string.Empty;

            if (value is string)
            {
                return CPFValido(value as string);
            }

            return CPFValido(value.ToString());
        }

        private static bool CPFValido(string cpf)
        {
            int[] numArray1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] numArray2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            for (int index = 0; index < 10; ++index)
            {
                if (index.ToString().PadLeft(11, char.Parse(index.ToString())) == cpf)
                    return false;
            }

            string str1 = cpf.Substring(0, 9);
            int num1 = 0;
            char ch;

            for (int index = 0; index < 9; ++index)
            {
                int num2 = num1;
                ch = str1[index];
                int num3 = int.Parse(ch.ToString()) * numArray1[index];
                num1 = num2 + num3;
            }

            int num4 = num1 % 11;
            string str2 = (num4 >= 2 ? 11 - num4 : 0).ToString();
            string str3 = str1 + str2;
            int num5 = 0;

            for (int index = 0; index < 10; ++index)
            {
                int num6 = num5;
                ch = str3[index];
                int num7 = int.Parse(ch.ToString()) * numArray2[index];
                num5 = num6 + num7;
            }

            int num8 = num5 % 11;
            int num9 = num8 >= 2 ? 11 - num8 : 0;
            string str4 = str2 + num9.ToString();

            return cpf.EndsWith(str4);
        }
    }
}