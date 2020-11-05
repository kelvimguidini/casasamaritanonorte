using casasamaritanonorte.ViewModels;
using casasamaritanonorte.Models;
using AutoMapper;

namespace casasamaritanonorte.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected void Configure()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CaptadorViewModels, Captador>();
                cfg.CreateMap<CampanhaViewModels, Campanha>();
                cfg.CreateMap<PagamentoViewModels, Pagamento>();

                cfg.CreateMap<EventoViewModels, Evento>();
                cfg.CreateMap<ContatoViewModel, Contato>();

                cfg.ValidateInlineMaps = false;
            });

            IMapper mapper = config.CreateMapper();

        }
    }
}