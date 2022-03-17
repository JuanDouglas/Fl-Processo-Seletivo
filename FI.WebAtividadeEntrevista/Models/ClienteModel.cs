using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Cliente
    /// </summary>
    public class ClienteModel
    {
        public long Id { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        public string CEP { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public string Cidade { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
        public string Email { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string Estado { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        [Required]
        public string Logradouro { get; set; }

        /// <summary>
        /// Nacionalidade
        /// </summary>
        [Required]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [CPF]
        [Required]
        [StringLength(14)]
        public string CPF { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        [Required]
        public string Sobrenome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

    }

    public class CPFAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
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