using AutoMapper;
using casasamaritanonorte.ViewModels;
using casasamaritanonorte.Models;
using System.Web;
using System.Collections.Generic;

namespace casasamaritanonorte.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected void Configure()
        {

            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Campanha, CampanhaViewModels>();
                cfg.CreateMap<Pagamento, PagamentoViewModels>();

                cfg.CreateMap<Captador, CaptadorViewModels>();
                cfg.CreateMap<Evento, EventoViewModels>();
                cfg.CreateMap<Contato, ContatoViewModel>();

            });

            IMapper mapper = config.CreateMapper();

        }
    }
}