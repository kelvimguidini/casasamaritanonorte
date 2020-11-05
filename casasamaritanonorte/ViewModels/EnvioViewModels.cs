using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace casasamaritanonorte.ViewModels
{
    public class EnvioViewModels
    {
        public int IDPagamento { get; set; }

        [Display(Name = "Status do pagamento")]
        public string StatusPagamento { get; set; }

        [Display(Name = "Status da entrega")]
        public string SituacaoEntrega { get; set; }


        //Destinatario
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Required]
        [Display(Name = "DDD")]
        public string DDD { get; set; }

        [Required]
        [Display(Name = "CPF")]
        public string CPF { get; set; }


        //Produto
        [Required]
        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }


        [Display(Name = "Total")]
        public Decimal Total { get; set; }


        //Frete
        [Display(Name = "Frete")]
        public Decimal Frete { get; set; }


        public int TipoEntrega { get; set; }


        public int PrazoEntrega { get; set; }

        [Display(Name = "Cep")]
        public string Cep { get; set; }

        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; }

        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }


        [Display(Name = "UF")]
        public string UF { get; set; }


        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        [Display(Name = "Cidade")]
        public string Cidade { get; set; }


        public string Campanha { get; set; }
        public int IDCampanha { get; set; }
        
        public string CodigoTrasacao { get; set; }
        public DateTime DataCompra { get; set; }

    }


}