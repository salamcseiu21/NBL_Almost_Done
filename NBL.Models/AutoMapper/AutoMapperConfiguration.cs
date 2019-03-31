using AutoMapper;

namespace NBL.Models.AutoMapper
{
    public  class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainProfile>();
            });
        }
    }
}
