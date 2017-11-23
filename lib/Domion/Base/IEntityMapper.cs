namespace Domion.Base
{
    public interface IEntityMapper<TData, TEntity> where TData : class where TEntity : class
    {
        TData CreateData(TEntity entity);

        TEntity CreateEntity(TData data);

        TEntity UpdateEntity(TData data, TEntity entity);
    }
}
