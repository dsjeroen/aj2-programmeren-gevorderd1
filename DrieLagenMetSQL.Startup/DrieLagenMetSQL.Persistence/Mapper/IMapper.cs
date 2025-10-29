namespace DrieLagenMetSQL.Persistence.Mapper
{
    /// <summary>
    /// Generiek contract voor mapping tussen transport (DTO) en opslag (Model).
    /// Maakt injectie, mocking en vervanging van mappers mogelijk.
    /// Interface beperkt tot referentietypes (class) voor consistente null-handling.
    /// </summary>

    public interface IMapper<TDto, TModel>
        where TDto : class
        where TModel : class
    {
        /// <summary>Mapt één opslagmodel naar een DTO (readrichting).</summary>
        TDto MapToDTO(TModel model);

        /// <summary>Mapt één DTO naar een opslagmodel (writerichting).</summary>
        TModel MapToModel(TDto dto);

        /// <summary>Mapt een reeks modellen naar DTO's (readrichting, read-only lijst).</summary>
        IReadOnlyList<TDto> MapToDTO(IEnumerable<TModel> models);

        /// <summary>Mapt een reeks DTO's naar modellen (writerichting, read-only lijst).</summary>
        IReadOnlyList<TModel> MapToModel(IEnumerable<TDto> dtos);
    }
}
