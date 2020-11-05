namespace casasamaritanonorte.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public class Evento
    {
        public int ID { get; set; }
        public string CaminhoCapa { get; set; }
        public string Titulo { get; set; }
        public DateTime DataEvento { get; set; }
        public string Caption { get; set; }
        public string Texto { get; set; }
        public DateTime DataPublicacao { get; set; }

        public Album Album { get; set; }
    }

}
