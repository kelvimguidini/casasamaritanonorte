using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace casasamaritanonorte.ViewModels
{
    public class CaptadorViewModels
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Required]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Required]
        [Display(Name = "Banco")]
        public string Banco { get; set; }

        [Required]
        [Display(Name = "Agência")]
        public string Agencia { get; set; }

        [Required]
        [Display(Name = "Conta")]
        public string Conta { get; set; }

        [Required]
        [Display(Name = "Tipo de Conta")]
        public int tipoConta { get; set; }

        [Display(Name = "Multirão")]
        public bool Multirao { get; set; }

        [Display(Name = "Integral")]
        public bool Integral { get; set; }
    }
}