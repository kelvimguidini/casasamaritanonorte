namespace casasamaritanonorte.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public class Foto
    {
        public int ID { get; set; }
        public string Caminho { get; set; }

    }

}
