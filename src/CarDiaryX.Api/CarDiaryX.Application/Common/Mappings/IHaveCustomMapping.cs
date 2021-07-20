using AutoMapper;

namespace CarDiaryX.Application.Common.Mappings
{
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile mapper);
    }
}
