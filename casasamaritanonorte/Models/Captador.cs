namespace casasamaritanonorte.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Captador
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public int tipoConta { get; set; }
        public bool Multirao { get; set; }
        public bool Integral { get; set; }

        public ICollection<Campanha> Campanhas { get; set; }
    }

}
