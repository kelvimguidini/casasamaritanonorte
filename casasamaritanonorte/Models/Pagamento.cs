namespace casasamaritanonorte.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public class Pagamento
    {

        public int ID { get; set; }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string DDD { get; set; }
        public string CPF { get; set; }

        //A ser pago/doado
        public Decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public Decimal Total { get; set; }
        public Decimal Frete { get; set; }

        public int PrazoEntrega { get; set; }
        public string SituacaoEntrega { get; set; }

        public string CodigoTrasacao { get; set; }

        //FRETE
        public int TipoEntrega { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string UF { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public Captador Captador { get; set; }
        public Campanha Campanha { get; set; }

        public DateTime Data { get; set; }
        public bool PrimeiraVisualizacao { get; set; }
        public bool ComissaoPaga { get; set; }
    }

}
