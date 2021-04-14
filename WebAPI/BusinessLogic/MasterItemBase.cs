namespace BusinessLogic
{
    public class MasterBase
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

    }

    public class Item : MasterBase
    {

    }

    public class Customer : MasterBase
    {
        
    }
}