using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace casasamaritanonorte.ViewModels
{
    public class EventoViewModels
    {
        public int ID { get; set; }

        [Display(Name = "Imagens")]
        public string CaminhoCapa { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data do Evento")]
        public DateTime DataEvento { get; set; }

        [Display(Name = "Texto em Destaque")]
        public string Caption { get; set; }

        [Display(Name = "Texto Formatado")]
        [DataType(DataType.MultilineText)]
        public string Texto { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataPublicacao { get; set; }

    }
}