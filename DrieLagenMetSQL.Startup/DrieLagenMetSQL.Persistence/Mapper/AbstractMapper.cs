namespace DrieLagenMetSQL.Persistence.Mapper
{
    /// <summary>
    /// Basisimplementatie met lijst-helpers.
    /// Doel:
    /// - Vermijdt duplicatie voor lijstmapping (DTO &lt;→ Model).
    /// - Subklassen implementeren enkel de element-naar-element mapping.
    /// Contract:
    /// - MapToDTO/MapToModel mogen nooit null retourneren voor geldige input.
    /// Bijkomend: IEnumerable in / IReadOnlyList uit → flexibel & immutabel.
    /// </summary>

    public abstract class AbstractMapper<TDto, TModel> :IMapper<TDto, TModel>
        where TDto : class
        where TModel : class
    {
        public IReadOnlyList<TDto> MapToDTO(IEnumerable<TModel> models)
        {
            ArgumentNullException.ThrowIfNull(models);

            // Materialiseer één keer en pre-alloceer capaciteit
            var src = models as ICollection<TModel> ?? models.ToList();
            var result = new List<TDto>(src.Count);

            foreach (var model in src)
            {
                ArgumentNullException.ThrowIfNull(model);
                var dto = MapToDTO(model) 
                    ?? throw new InvalidOperationException("MapToDTO() gaf null terug.");
                result.Add(dto);
            }
            return result;
        }

        public IReadOnlyList<TModel> MapToModel(IEnumerable<TDto> dtos)
        {
            ArgumentNullException.ThrowIfNull(dtos);

            var src = dtos as ICollection<TDto> ?? dtos.ToList();
            var result = new List<TModel>(src.Count);

            foreach (var dto in src)
            {
                ArgumentNullException.ThrowIfNull(dto);
                var model = MapToModel(dto);
                if (model is null)
                    throw new InvalidOperationException("MapToModel() gaf null terug.");
                result.Add(model);
            }
            return result;
        }

        public abstract TDto MapToDTO(TModel model);
        public abstract TModel MapToModel(TDto dto);
    }
}
