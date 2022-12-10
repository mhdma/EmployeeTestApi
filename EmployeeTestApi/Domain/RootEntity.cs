namespace EmployeeTestApi.Domain
{


    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class RootEntity<TKey>
    {

        public virtual TKey Id { get; set; }


    }

}
