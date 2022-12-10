namespace EmployeeTestApi.Domain
{

    public class Employee : RootEntity<int>
    {

        public virtual string Name  { get; set; }
        public virtual string Email  { get; set; }
        public virtual string Phone  { get; set; }


    }

}
