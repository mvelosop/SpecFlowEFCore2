using AutoMapper;
using Budget.Core.Model;
using Domion.Base;

namespace Budget.Specs.Helpers
{
    // 11-2. Add BudgetClassData helper class
    //---------------------------------------
    public class BudgetClassMapper : IEntityMapper<BudgetClassData, BudgetClass>
    {
        private static readonly MapperConfiguration Config = new MapperConfiguration(config =>
        {
            config.CreateMap<BudgetClassData, BudgetClass>();
            config.CreateMap<BudgetClass, BudgetClassData>();
        });

        private readonly IMapper _mapper;

        public BudgetClassMapper()
        {
            _mapper = Config.CreateMapper();
        }

        public BudgetClassData CreateData(BudgetClass entity)
        {
            return _mapper.Map<BudgetClassData>(entity);
        }

        public BudgetClass CreateEntity(BudgetClassData data)
        {
            return _mapper.Map<BudgetClass>(data);
        }

        public BudgetClass UpdateEntity(BudgetClassData data, BudgetClass entity)
        {
            return _mapper.Map(data, entity);
        }
    }
}
