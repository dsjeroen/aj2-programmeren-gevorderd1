namespace DrieLagenMetSQL.Persistence.Mapper
{
    /// <summary>
    /// Generiek contract voor vertaling tussen transport (DTO) en opslag (Model).
    /// Het zorgt dat we tegen een abstractie kunnen programmeren, en dus mappers 
    /// kunnen wisselen / mocken / injecteren.
    /// Houdt mapping buiten Domain en centraliseert het in Persistence.
    /// Generieke mapper-types beperkt tot type class om alleen referentietypes toe te laten.
    /// </summary>

    public interface IMapper<TDto, TModel>
        where TDto : class
        where TModel : class
    {
        TDto MapToDTO(TModel model);
        TModel MapToModel(TDto dto);

        IReadOnlyList<TDto> MapToDTO(IEnumerable<TModel> models);
        IReadOnlyList<TModel> MapToModel(IEnumerable<TDto> dtos);
    }
}
