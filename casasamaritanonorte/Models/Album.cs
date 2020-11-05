namespace casasamaritanonorte.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public class Album
    {
        public int ID { get; set; }
        public string Nome { get; set; }

        public ICollection<Foto> Fotos { get; set; }
    }

}
