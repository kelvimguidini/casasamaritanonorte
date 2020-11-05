using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace casasamaritanonorte.ViewModels
{
    public class PagamentoViewModels
    {
        public int ID { get; set; }

        //A ser pago/doado
        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:2}")]
        [NumeroBrasil(ErrorMessage = "Valor inválido", DecimalRequerido = true, PontoMilharPermitido = false)]
        public Decimal Valor { get; set; }

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


        [Required]
        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:2}")]
        [NumeroBrasil(ErrorMessage = "Valor inválido", DecimalRequerido = true, PontoMilharPermitido = false)]
        public Decimal Total { get; set; }

        [Display(Name = "Frete")]
        [DisplayFormat(DataFormatString = "{0:2}")]
        [NumeroBrasil(ErrorMessage = "Valor inválido", DecimalRequerido = true, PontoMilharPermitido = false)]
        public Decimal Frete { get; set; }


        public int TipoEntrega { get; set; }


        [Display(Name = "Situação da Entrega")]
        public string SituacaoEntrega { get; set; }

        public int PrazoEntrega { get; set; }
        //FRETE

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

        public CaptadorViewModels Captador { get; set; }

        public CampanhaViewModels Campanha { get; set; }

        public string CodigoTrasacao { get; set; }
        public DateTime Data { get; set; }

        public bool PrimeiraVisualizacao { get; set; }
        public bool ComissaoPaga { get; set; }

    }


}