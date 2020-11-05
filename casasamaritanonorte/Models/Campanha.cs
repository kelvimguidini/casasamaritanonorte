namespace casasamaritanonorte.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web;

    public class Campanha
    {


        public int ID { get; set; }

        [NotMapped]
        public HttpPostedFileBase Video { get; set; }
        public string VideoStr { get; set; }
        [NotMapped]
        public HttpPostedFileBase Foto { get; set; }
        public string fotoStr { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Mensagem { get; set; }
        public bool Doacao { get; set; }

        //A ser pago/doado
        public Decimal Valor { get; set; }
        public Decimal Peso { get; set; }
        public int Altura { get; set; }

        public int Largura { get; set; }

        public int Diametro { get; set; }

        public int Comprimento { get; set; }

        public ICollection<Captador> Captadores { get; set; }

        public bool TelaInicial { get; set; }

        public bool Ativo { get; set; }
    }

}
