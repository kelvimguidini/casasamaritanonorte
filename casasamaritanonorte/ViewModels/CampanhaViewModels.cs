using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace casasamaritanonorte.ViewModels
{
    public class CampanhaViewModels
    {
        public int ID { get; set; }


        [Display(Name = "Vídeo")]
        public HttpPostedFileBase Video { get; set; }

        public string VideoStr { get; set; }

        [Display(Name = "Foto")]
        public HttpPostedFileBase Foto { get; set; }

        public string fotoStr { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Mensagem")]
        public string Mensagem { get; set; }

        [Display(Name = "Doação")]
        public bool Doacao { get; set; }

        [Display(Name = "Peso (KG)")]
        public Decimal Peso { get; set; }

        //A ser pago/doado
        [NumeroBrasil(ErrorMessage = "Valor inválido", DecimalRequerido = true, PontoMilharPermitido = false)]
        [Display(Name = "Valor (R$)")]
        public Decimal Valor { get; set; }

        
        [Display(Name = "Altura (CM)")]
        public int Altura { get; set; }

        [Range(11, 1000)]
        [Display(Name = "Largura (CM)")]
        public int Largura { get; set; }

        [Display(Name = "Diametro (CM)")]
        public int Diametro { get; set; }

        [Display(Name = "Comprimento (CM)")]
        public int Comprimento { get; set; }

        [Display(Name = "Exibir na tela inicial (Marcando esse, todos os outros serão desmarcados.")]
        public bool TelaInicial { get; set; }

        [Display(Name = "Ativo (Caso desmarcado não aparecerá para captadores.")]
        public bool Ativo { get; set; }
    }

}