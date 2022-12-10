namespace EmployeeTestApi.Domain
{

    public class User :RootEntity<int>
    {

        public virtual string UserName  { get; set; }
        public virtual string Password  { get; set; }
        public virtual string Role  { get; set; }


    }

}
