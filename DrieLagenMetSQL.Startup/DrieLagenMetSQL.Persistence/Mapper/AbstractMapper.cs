namespace DrieLagenMetSQL.Persistence.Mapper
{
    /// <summary>
    /// Basismapper met lijsthelpers: voorkomt duplicatie voor DTO↔Model lijstmapping.
    /// Subklassen implementeren enkel element-naar-element mapping. (abstract, uitbreidbaar)
    /// </summary>

    public abstract class AbstractMapper<TDto, TModel> :IMapper<TDto, TModel>
        where TDto : class
        where TModel : class
    {
        /// <summary>Mapt een reeks modellen naar DTO's (materialiseert bron éénmalig, read-only lijst).</summary>
        public IReadOnlyList<TDto> MapToDTO(IEnumerable<TModel> models)
        {
            ArgumentNullException.ThrowIfNull(models);

            var src = models as ICollection<TModel> ?? models.ToList();
            var result = new List<TDto>(src.Count);

            foreach (var model in src)
            {
                ArgumentNullException.ThrowIfNull(model);
                var dto = MapToDTO(model)
                          ?? throw new InvalidOperationException("MapToDTO() retourneerde null.");
                result.Add(dto);
            }

            return result;
        }

        /// <summary>Mapt een reeks DTO's naar modellen (materialiseert bron éénmalig, read-only lijst).</summary>
        public IReadOnlyList<TModel> MapToModel(IEnumerable<TDto> dtos)
        {
            ArgumentNullException.ThrowIfNull(dtos);

            var src = dtos as ICollection<TDto> ?? dtos.ToList();
            var result = new List<TModel>(src.Count);

            foreach (var dto in src)
            {
                ArgumentNullException.ThrowIfNull(dto);
                var model = MapToModel(dto)
                            ?? throw new InvalidOperationException("MapToModel() retourneerde null.");
                result.Add(model);
            }

            return result;
        }

        /// <summary>Mapt één model naar één DTO.</summary>
        public abstract TDto MapToDTO(TModel model);

        /// <summary>Mapt één DTO naar één model.</summary>
        public abstract TModel MapToModel(TDto dto);
    }
}