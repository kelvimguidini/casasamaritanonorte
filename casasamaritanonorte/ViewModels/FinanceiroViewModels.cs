using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace casasamaritanonorte.ViewModels
{
    public class FinanceiroViewModels
    {

        //A ser pago/doado
        [Display(Name = "Valor")]
        public Decimal Valor { get; set; }

        public int IDPagamento { get; set; }
        
        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [Display(Name = "Total")]
        public Decimal Total { get; set; }

        [Display(Name = "Frete")]
        public Decimal Frete { get; set; }

        public CaptadorViewModels Captador { get; set; }

        public CampanhaViewModels Campanha { get; set; }

        [Display(Name = "Status do pagamento")]
        public string StatusPagamento { get; set; }
        public DateTime Data { get; set; }

        public string FormaPagamento { get; set; }
        public bool ComissaoPaga { get; set; }
    }


}