using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.DTOs;

namespace ToDoListInfrastructure.Mappers
{
    public class AutoMapperConfig : Profile
    {
        public static IMapper Initialize() => new MapperConfiguration(cfg =>
        {

            cfg.CreateMap<ToDoList, ToDoListDto>();
            cfg.CreateMap<ToDoEntry, ToDoEntryDto>();
            cfg.CreateMap<ToDoEntry, ToDoEntryDetailsDto>()
                                .ForMember(x => x.ToDoListTitle, c => c.MapFrom(y => y.ToDoList.Title));
            cfg.CreateMap<NotesTde, NoteDto>();

        }).CreateMapper();
    }
}
